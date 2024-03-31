using Unity.Mathematics;

namespace Input
{
    public struct InputData
    {
        public float2 Move;//движение оси WASD
        public float2 Mouse;//мыш оси
        public float2 MousePosition;//мыш позиция
        public float MouseLeftButton;//мыш левая
        public float MouseMiddleButton;//мыш колесо
        public float MouseRightButton;//мыш правая
        public float Executor;
        public float Mode;
        public float Menu;
        public Mode ModeAction;
        public Mode[] Modes;
    }
}

