using UnityEngine;

namespace TemleLogic
{
    [CreateAssetMenu(fileName = "TempleScanerSettings", menuName = "ScriptableObjects/TempleScanerSettings")]
    public class TempleScanerSettings : ScriptableObject
    {
        [Header("������� ����������")]
        public float DiametrCollider = 40f;
    }
}

