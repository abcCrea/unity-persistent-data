using System;
using System.Collections;
using System.Collections.Generic;
using abcCrea.PersistentData.Samples.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace abcCrea.PersistentData.Samples.Controllers
{
    public class TreasureController : MonoBehaviour
    {
        public event Action<RewardData, int> RewardGranted;

        [SerializeField] private List<MonoBehaviour> _treasureTargets = new();
        [SerializeField, Min(0f)] private float _minActivationDelay = 1f;
        [SerializeField, Min(0f)] private float _maxActivationDelay = 3f;
        [SerializeField, Min(0.1f)] private float _minActiveDuration = 4f;
        [SerializeField, Min(0.1f)] private float _maxActiveDuration = 7f;

        private readonly List<ITreasureActivationTarget> _targets = new();
        private Coroutine _activationLoop;

        private void Awake()
        {
            CollectTargets();

            foreach (ITreasureActivationTarget target in _targets)
            {
                target.RewardGranted += OnRewardGranted;
                target.Deactivate();
            }

            _activationLoop = StartCoroutine(ActivationLoop());
        }

        private void OnDestroy()
        {
            if (_activationLoop != null)
            {
                StopCoroutine(_activationLoop);
                _activationLoop = null;
            }

            foreach (ITreasureActivationTarget target in _targets)
            {
                target.RewardGranted -= OnRewardGranted;
                target.Deactivate();
            }
        }

        private void CollectTargets()
        {
            _targets.Clear();

            foreach (MonoBehaviour treasureTarget in _treasureTargets)
                AddTarget(treasureTarget);
        }

        private void AddTarget(MonoBehaviour behaviour)
        {
            if (behaviour is not ITreasureActivationTarget target || _targets.Contains(target))
                return;

            _targets.Add(target);
        }

        private IEnumerator ActivationLoop()
        {
            while (enabled)
            {
                float delay = Random.Range(_minActivationDelay, Mathf.Max(_minActivationDelay, _maxActivationDelay));
                yield return new WaitForSeconds(delay);

                ITreasureActivationTarget target = PickInactiveTarget();
                if (target == null)
                {
                    yield return null;
                    continue;
                }

                float activeDuration = Random.Range(_minActiveDuration, Mathf.Max(_minActiveDuration, _maxActiveDuration));
                target.Activate(activeDuration);

                while (target.IsActive)
                    yield return null;
            }
        }

        private ITreasureActivationTarget PickInactiveTarget()
        {
            List<ITreasureActivationTarget> inactiveTargets = new();

            foreach (ITreasureActivationTarget target in _targets)
            {
                if (!target.IsActive)
                    inactiveTargets.Add(target);
            }

            return inactiveTargets.Count == 0 ? null : inactiveTargets[Random.Range(0, inactiveTargets.Count)];
        }

        private void OnRewardGranted(ITreasureActivationTarget source, RewardData reward, int amount)
        {
            RewardGranted?.Invoke(reward, amount);
        }
    }
}
