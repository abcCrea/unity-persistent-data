using System.Collections.Generic;
using abcCrea.PersistentData.Samples.Data;
using abcCrea.PersistentData.Samples.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace abcCrea.PersistentData.Samples.Controllers
{
    public class RewardsHUDController : MonoBehaviour
    {
        private class RewardItemView
        {
            private readonly GameObject _root;
            private readonly Image _icon;
            private readonly TMP_Text _amountText;
            private readonly TMP_Text _nameText;

            public RewardItemView(GameObject root)
            {
                _root = root;
                Image[] images = _root.GetComponentsInChildren<Image>(true);
                TMP_Text[] texts = _root.GetComponentsInChildren<TMP_Text>(true);

                _icon = images.Length > 0 ? images[^1] : null;
                _amountText = texts.Length > 0 ? texts[0] : null;
                _nameText = texts.Length > 1 ? texts[1] : null;
            }

            public void SetReward(RewardData reward)
            {
                if (_icon != null)
                {
                    _icon.sprite = reward.Icon;
                    _icon.enabled = reward.Icon != null;
                }

                if (_nameText != null)
                    _nameText.text = reward.DisplayName;
            }

            public void SetAmount(int amount)
            {
                if (_amountText != null)
                    _amountText.text = amount.ToString();
            }
        }

        [SerializeField] private RectTransform _rewardItemsContainer;
        [SerializeField] private GameObject _rewardItemTemplate;

        [Header("Dependencies")]
        [SerializeField] private RewardManager _rewardManager;
        [SerializeField] private TreasureController _treasureController;

        private readonly Dictionary<string, RewardItemView> _rewardViews = new();

        private void Awake()
        {
            _rewardManager = FindFirstObjectByType<RewardManager>();
            if (_rewardManager == null)
            {
               Debug.LogWarning("RewardManager not found in scene");
               return;
            }
            _rewardManager.RewardsLoaded += RebuildRewardViews;
            _rewardManager.RewardAmountChanged += UpdateRewardAmount;

            if (_rewardManager.IsLoaded)
                RebuildRewardViews();

            _treasureController = FindFirstObjectByType<TreasureController>();
            if (_rewardManager == null)
            {
               Debug.LogWarning("TreasureController not found in scene");
               return;
            }
            _treasureController.RewardGranted += OnTreasureRewardGranted;
        }

        private void OnDestroy()
        {
            if (_rewardManager != null)
            {
                _rewardManager.RewardsLoaded -= RebuildRewardViews;
                _rewardManager.RewardAmountChanged -= UpdateRewardAmount;
            }

            if (_treasureController != null)
                _treasureController.RewardGranted -= OnTreasureRewardGranted;
        }

        private void Reset()
        {
            _rewardManager = FindAnyObjectByType<RewardManager>();
            _treasureController = FindAnyObjectByType<TreasureController>();
        }

        private void RebuildRewardViews()
        {
            _rewardViews.Clear();

            for (int i = _rewardItemsContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = _rewardItemsContainer.GetChild(i);
                if (child.gameObject == _rewardItemTemplate)
                    continue;

                Destroy(child.gameObject);
            }

            _rewardItemTemplate.SetActive(false);

            foreach (RewardData reward in _rewardManager.AvailableRewards)
            {
                if (reward == null || string.IsNullOrWhiteSpace(reward.Id))
                    continue;

                GameObject item = Instantiate(_rewardItemTemplate, _rewardItemsContainer);
                item.name = $"{reward.DisplayName} Reward Item";
                item.SetActive(true);

                RewardItemView view = new(item);
                view.SetReward(reward);
                view.SetAmount(_rewardManager.GetAmount(reward));
                _rewardViews[reward.Id] = view;
            }
        }

        private void OnTreasureRewardGranted(RewardData reward, int amount)
        {
            _rewardManager.AddReward(reward, amount);
        }

        private void UpdateRewardAmount(RewardData reward, int totalAmount)
        {
            if (reward == null || !_rewardViews.TryGetValue(reward.Id, out RewardItemView view))
                return;

            view.SetAmount(totalAmount);
        }
    }
}
