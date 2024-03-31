using UnityEngine;

namespace TemleLogic
{
    [CreateAssetMenu(fileName = "TempleScanerSettings", menuName = "ScriptableObjects/TempleScanerSettings")]
    public class TempleScanerSettings : ScriptableObject
    {
        [Header("Диаметр коллайдера")]
        public float DiametrCollider = 40f;
    }
}

