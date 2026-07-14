using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace abcCrea.PersistentData.Samples.Data
{
    [CreateAssetMenu(fileName = "Treasure Data", menuName = "abcCrea/Persistent Data Demo/Treasure Data")]
    public class TreasureData : ScriptableObject
    {
        [Serializable]
        public class TreasureRewardEntry
        {
            public RewardData Reward => _reward;
            public int Weight => _weight;
            public int MinAmount => _minAmount;
            public int MaxAmount => _maxAmount;
            public bool IsValid => _reward != null && _weight > 0 && _minAmount > 0 && _maxAmount >= _minAmount;

            [SerializeField] private RewardData _reward;
            [SerializeField, Min(1)] private int _weight = 1;
            [SerializeField, Min(1)] private int _minAmount = 1;
            [SerializeField, Min(1)] private int _maxAmount = 5;

            public int GetRandomAmount() => Random.Range(_minAmount, _maxAmount + 1);
        }

        public readonly struct TreasureRewardResult
        {
            public static readonly TreasureRewardResult Empty = new(null, 0);

            public TreasureRewardResult(RewardData reward, int amount)
            {
                Reward = reward;
                Amount = amount;
            }

            public RewardData Reward { get; }
            public int Amount { get; }
            public bool IsValid => Reward != null && Amount > 0;
        }

        public IReadOnlyList<TreasureRewardEntry> PossibleRewards => _possibleRewards;

        [SerializeField] private List<TreasureRewardEntry> _possibleRewards = new();

        public TreasureRewardResult GetWeightedReward()
        {
            int totalWeight = 0;

            foreach (TreasureRewardEntry reward in _possibleRewards)
            {
                if (!reward.IsValid) continue;
                totalWeight += reward.Weight;
            }

            if (totalWeight <= 0)
                return TreasureRewardResult.Empty;

            int roll = Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (TreasureRewardEntry reward in _possibleRewards)
            {
                if (!reward.IsValid) continue;

                currentWeight += reward.Weight;
                if (roll >= currentWeight) continue;

                return new TreasureRewardResult(reward.Reward, reward.GetRandomAmount());
            }
            return TreasureRewardResult.Empty;
        }
    }
}
