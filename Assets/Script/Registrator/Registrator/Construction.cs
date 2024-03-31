using UnityEngine;

namespace Registrator
{
    public enum TypeObject
    {
        Player,
        Enemy,
        Other
    }
    public enum StatusTemple
    {
        Two, One, Null, Destoy
    }
    public struct Construction : IConstruction
    {
        public int Hash { get; set; }
        public bool IsDead { get; set; }
        public Transform Transform;
        public int[] ChildrenHash { get; set; }
        public TypeObject TypeObject { get; set; }
        public Camera CameraComponent;
        public StatusTemple StatusTemle { get; set; }
    }
}

