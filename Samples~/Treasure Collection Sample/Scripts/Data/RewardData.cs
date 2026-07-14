using System;
using UnityEngine;

namespace abcCrea.PersistentData.Samples.Data
{
    [CreateAssetMenu(fileName = "Reward Data", menuName = "abcCrea/Persistent Data Demo/Reward Data")]
    public class RewardData : ScriptableObject
    {
        public string Id => _id;
        public string DisplayName =>  _displayName;
        public Sprite Icon => _icon;

        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        private void OnValidate()
        {
            EnsureId();
        }

        private void EnsureId()
        {
            if (!string.IsNullOrEmpty(_id)) return;
            _id = Guid.NewGuid().ToString("N");
        }
    }
}
