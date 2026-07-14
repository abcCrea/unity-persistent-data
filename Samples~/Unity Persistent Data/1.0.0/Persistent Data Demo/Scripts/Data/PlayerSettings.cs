using UnityEngine;

namespace abcCrea.PersistentData.Samples.Data
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "abcCrea/Persistent Data Demo/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        public float MovementSpeed => _movementSpeed;

        [SerializeField, Min(0f)] private float _movementSpeed = 5f;
    }
}
