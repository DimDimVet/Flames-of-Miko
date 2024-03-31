using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "MoveSettings", menuName = "ScriptableObjects/MoveSettings")]
    public class MoveSettings : ScriptableObject
    {
        [Header("Скорость вперед"), Range(0, 150)]
        public float SpeedMove = 5f;
        [Header("Скорость поворота"), Range(0, 150)]
        public float SpeedTurn = 5f;
    }
}

