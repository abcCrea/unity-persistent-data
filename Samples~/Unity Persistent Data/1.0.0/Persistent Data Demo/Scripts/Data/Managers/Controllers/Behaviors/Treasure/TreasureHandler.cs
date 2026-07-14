using System;
using System.Collections;
using abcCrea.PersistentData.Samples.Controllers;
using abcCrea.PersistentData.Samples.Data;
using UnityEngine;
using UnityEngine.Events;

namespace abcCrea.PersistentData.Samples.Treasure
{
    public class TreasureHandler : MonoBehaviour, ITreasureActivationTarget
    {
        [Serializable]
        private class RewardUnityEvent : UnityEvent<RewardData, int>
        {
        }

        public event Action<ITreasureActivationTarget, RewardData, int> RewardGranted;

        [SerializeField] private TreasureData _treasureData;
        [SerializeField] private string _playerTag = "Player";
        [SerializeField] private SpriteRenderer _activeVisual;
        [SerializeField] private UnityEvent _onActivated = new();
        [SerializeField] private UnityEvent _onDeactivated = new();
        [SerializeField] private RewardUnityEvent _onRewardGranted = new();

        private TreasureData.TreasureRewardResult _currentRewad;
        private Coroutine _expirationRoutine;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _activeVisual.sprite = null;
        }

        private void OnDestroy()
        {
            StopExpirationRoutine();
        }

        public void Activate(float activeSeconds)
        {
            _currentRewad = _treasureData.GetWeightedReward();
            if (!_currentRewad.IsValid)
            {
                Deactivate();
                return;
            }
            _activeVisual.sprite = _currentRewad.Reward.Icon;
            IsActive = true;
            _onActivated.Invoke();

            StopExpirationRoutine();
            _expirationRoutine = StartCoroutine(DeactivateAfter(activeSeconds));
        }

        public void Deactivate()
        {
            if (!IsActive)return;

            IsActive = false;
            _activeVisual.sprite = null;
            StopExpirationRoutine();
            _onDeactivated.Invoke();
        }

        private void OnTriggerEntered(Collider2D other)
        {
            if (!IsActive || other == null || !_treasureData || !IsPlayerCollider(other)) return;

            RewardGranted?.Invoke(this, _currentRewad.Reward, _currentRewad.Amount);
            _onRewardGranted.Invoke(_currentRewad.Reward, _currentRewad.Amount);
            Deactivate();
        }

        private bool IsPlayerCollider(Collider2D other)
        {
            if (string.IsNullOrEmpty(_playerTag) || other.CompareTag(_playerTag))
                return true;
     
            Transform parent = other.transform.parent;
            if (parent != null && parent.CompareTag(_playerTag))
                return true;

            Transform root = other.transform.root;
            return root != null && root.CompareTag(_playerTag);
        }
        

        private IEnumerator DeactivateAfter(float seconds)
        {
            yield return new WaitForSeconds(Mathf.Max(0.1f, seconds));
            Deactivate();
        }

        private void StopExpirationRoutine()
        {
            if (_expirationRoutine == null) return;

            StopCoroutine(_expirationRoutine);
            _expirationRoutine = null;
        }
    }
}
