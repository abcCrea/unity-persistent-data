using System;
using UnityEngine;
using UnityEngine.Events;

namespace abcCrea.PersistentData.Samples.Behaviors.Generic
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class TriggerEventRelay2D : MonoBehaviour
    {
        [Serializable]
        private sealed class Collider2DUnityEvent : UnityEvent<Collider2D>
        {
        }

        public event Action<Collider2D> TriggerEntered;
        public event Action<Collider2D> TriggerStayed;
        public event Action<Collider2D> TriggerExited;

        [SerializeField] private Collider2DUnityEvent _onTriggerEnter = new();
        [SerializeField] private Collider2DUnityEvent _onTriggerStay = new();
        [SerializeField] private Collider2DUnityEvent _onTriggerExit = new();

        private Collider2D _collider;

        private void Awake()
        {
            if (!TryGetComponent(out _collider))
            {
                Debug.LogWarning($"{nameof(TriggerEventRelay2D)} requires a Collider2D component.", this);
                return;
            }

            if (!_collider.isTrigger)
            {
                Debug.LogWarning($"{nameof(TriggerEventRelay2D)} expects the Collider2D to be configured as a trigger.", this);
            }
        }

        private void Reset()
        {
            if (!TryGetComponent(out _collider))
            {
                return;
            }

            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered?.Invoke(other);
            _onTriggerEnter.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TriggerStayed?.Invoke(other);
            _onTriggerStay.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerExited?.Invoke(other);
            _onTriggerExit.Invoke(other);
        }
    }
}
