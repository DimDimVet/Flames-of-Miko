using UnityEngine;

namespace Effect
{
    public class AnimGirlPlayer : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string commandMove;
        private float currentVelocity;
        private Rigidbody rigidbodyGameObject;
        private bool isStopClass = false, isRun = false;

        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                rigidbodyGameObject = gameObject.GetComponent<Rigidbody>();
                if (!(rigidbodyGameObject is Rigidbody))
                {
                    rigidbodyGameObject = gameObject.AddComponent<Rigidbody>();
                    isRun = false;
                }
                else { isRun = true; }
            }
        }
        void FixedUpdate()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
            RunUpdate();
        }
        private void RunUpdate()
        {
            Move();
        }
        private void Move()
        {
            currentVelocity = Mathf.Abs(rigidbodyGameObject.velocity.x)+ Mathf.Abs(rigidbodyGameObject.velocity.z);
            if (currentVelocity > 0.1f && commandMove != "") { animator.SetFloat(commandMove, 1); }
            else { animator.SetFloat(commandMove, 0); }
        }
    }
}
