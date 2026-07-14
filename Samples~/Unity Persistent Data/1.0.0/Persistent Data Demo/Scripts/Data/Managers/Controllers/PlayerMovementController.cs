using abcCrea.PersistentData.Samples.Data;
using abcCrea.PersistentData.Samples.Input;
using UnityEngine;

namespace abcCrea.PersistentData.Samples.Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Rigidbody2D _targetRigidbody;

        private void Awake()
        {
            if (_targetRigidbody == null)
                _targetRigidbody = GetComponentInParent<Rigidbody2D>();
        }

        private void Reset()
        {
            _targetRigidbody = GetComponentInParent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_targetRigidbody == null || _playerSettings == null) return;

            Vector2 movement = PlayerInputReader.ReadMovement();

            _targetRigidbody.MovePosition(_targetRigidbody.position + movement * (_playerSettings.MovementSpeed * Time.fixedDeltaTime));
        }
    }
}