using System;
using System.Collections.Generic;
using abcCrea.PersistentData;
using abcCrea.PersistentData.Samples.Data;
using UnityEngine;

namespace abcCrea.PersistentData.Samples.Managers
{
    public class RewardManager : MonoBehaviour
    {
        public event Action RewardsLoaded;
        public event Action<RewardData, int> RewardAmountChanged;

        public IReadOnlyList<RewardData> AvailableRewards => _availableRewards;
        public bool IsLoaded { get; private set; }

        [SerializeField] private List<RewardData> _availableRewards = new();

        private readonly Dictionary<string, int> _rewardAmounts = new();
        private const string DefaultSaveFileName = "persistent-data-demo-rewards.json";
        private string _saveFileName = DefaultSaveFileName;

        private void Awake()
        {
            LoadRewards();
        }

        public int GetAmount(RewardData reward)
        {
            if (reward == null)
                return 0;

            return _rewardAmounts.TryGetValue(reward.Id, out int amount) ? amount : 0;
        }

        public void AddReward(RewardData reward, int amount)
        {
            if (reward == null || amount <= 0) return;

            int newAmount = GetAmount(reward) + amount;
            _rewardAmounts[reward.Id] = newAmount;

            SaveRewards();
            RewardAmountChanged?.Invoke(reward, newAmount);
        }

        public void LoadRewards()
        {
            _rewardAmounts.Clear();
            Dictionary<string, int> loadedRewards = PersistentDataManager.LoadDictionaryData<string, int>(_saveFileName);

            foreach (RewardData reward in _availableRewards)
            {
                if (reward == null)
                    continue;

                _rewardAmounts[reward.Id] = loadedRewards.TryGetValue(reward.Id, out int amount) ? Mathf.Max(0, amount) : 0;
            }
            IsLoaded = true;
            RewardsLoaded?.Invoke();
        }

        public void SaveRewards()
        {
            PersistentDataManager.SaveDictionaryData(_saveFileName, _rewardAmounts);
        }
    }
}
