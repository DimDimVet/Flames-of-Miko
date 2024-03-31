using System;

namespace EntityLogic
{
    public interface ILogicEntityExecutor
    {
        Action<float, float> OnMinusSpeedBaf { get; set; }
        void MinusSpeedPlayer(float _percentSpeed, float timeBaf);
    }
}

