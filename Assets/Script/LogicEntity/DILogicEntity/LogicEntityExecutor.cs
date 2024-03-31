using System;

namespace EntityLogic
{
    public class LogicEntityExecutor : ILogicEntityExecutor
    {
        public Action<float, float> OnMinusSpeedBaf { get { return onMinusSpeedBaf; } set { onMinusSpeedBaf = value; } }
        private Action<float, float> onMinusSpeedBaf;
        public void MinusSpeedPlayer(float _percentSpeed, float timeBaf)
        {
            onMinusSpeedBaf?.Invoke(_percentSpeed, timeBaf);
        }
    }
}