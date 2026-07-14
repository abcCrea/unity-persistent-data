using System;
using abcCrea.PersistentData.Samples.Data;

namespace abcCrea.PersistentData.Samples.Controllers
{
    public interface ITreasureActivationTarget
    {
        event Action<ITreasureActivationTarget, RewardData, int> RewardGranted;

        bool IsActive { get; }

        void Activate(float activeSeconds);
        void Deactivate();
    }
}
