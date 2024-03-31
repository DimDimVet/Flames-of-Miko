using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "MoveSettings", menuName = "ScriptableObjects/MoveSettings")]
    public class MoveSettings : ScriptableObject
    {
        [Header("�������� ������"), Range(0, 150)]
        public float SpeedMove = 5f;
        [Header("�������� ��������"), Range(0, 150)]
        public float SpeedTurn = 5f;
    }
}

