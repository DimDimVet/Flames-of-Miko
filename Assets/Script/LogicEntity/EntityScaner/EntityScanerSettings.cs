using UnityEngine;

namespace EntityLogic
{
    [CreateAssetMenu(fileName = "EntityScanerSettings", menuName = "ScriptableObjects/EntityScanerSettings")]
    public class EntityScanerSettings : ScriptableObject
    {
        [Header("Диаметр коллайдера")]
        public float DiametrCollider = 40f;
    }
}

