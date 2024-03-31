using TemleLogic;
using UnityEngine;
using Zenject;

namespace EntityLogic
{
    enum ModeMove
    {
        Line, Circle
    }
    public class MoveEntity : MonoBehaviour
    {
        [Header("Режим движения")]
        [SerializeField] private ModeMove modeMove;

        [Header("Скорость движения"), Range(0, 10f)]
        [SerializeField] private float speedMove = 1f;
        private float currentSpeedMove;
        [Header("Скорость поворота"), Range(0, 10f)]
        [SerializeField] private float speedTurn = 1f;
        [Header("Точки цели")]
        [SerializeField] private GameObject[] targets;
        [Header("Радиус"), Range(0, 50f)]
        [SerializeField] private float radius = 5f;

        private Vector3 targetPosition;
        private int currentCount, countTargets;
        private float angle = 0f;
        private Rigidbody rigidbodyGameObject;
        private Vector3 moveDirection;
        private Quaternion deltaRotation, directionRotation;
        private bool isTrigger = false;

        private bool isStopClass = false, isRun = false;
        //private int thisHash;

        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(ITempleExecutor _templeExecutor)
        {
            templeExecutor = _templeExecutor;
        }
        private void OnEnable()
        {
            templeExecutor.OnPlusSpeedEntity += PlusSpeedEntity;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                //thisHash = gameObject.GetHashCode();
                currentSpeedMove = speedMove;
                rigidbodyGameObject = gameObject.GetComponent<Rigidbody>();

                if (targets != null) { countTargets = targets.Length; }
                else { return; }

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
            if (modeMove == ModeMove.Line) { MoveInTargetLine(); }
            if (modeMove == ModeMove.Circle) { MoveInTargetCircle(); }
        }
        private void PlusSpeedEntity(float percentSpeed)
        {
            currentSpeedMove = ((percentSpeed) / 100) * speedMove;
        }
        private void TargetSelect()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targetPosition == targets[i].transform.position)
                {
                    currentCount++;
                    if (currentCount >= countTargets) { currentCount = 0; }
                    targetPosition = targets[currentCount].transform.position;
                    return;
                }
            }
            targetPosition = targets[0].transform.position;
            currentCount = 0;
        }
        public Quaternion GetRotation(Vector3 moveDirection)
        {
            moveDirection.y = 0;
            deltaRotation = Quaternion.LookRotation(moveDirection);
            directionRotation = Quaternion.Lerp(this.gameObject.transform.rotation,
                                                deltaRotation,
                                                speedTurn);
            return directionRotation;
        }
        private void MoveInTargetLine()
        {
            if (targetPosition == Vector3.zero)
            {
                TargetSelect();
                return;
            }

            moveDirection = targetPosition - this.gameObject.transform.position;

            if (moveDirection.magnitude <= 1.1f && isTrigger)
            {
                rigidbodyGameObject.velocity = Vector3.zero;
                rigidbodyGameObject.MoveRotation(GetRotation(moveDirection));
                TargetSelect();
                isTrigger = !isTrigger;
            }
            else
            {
                rigidbodyGameObject.velocity = this.gameObject.transform.forward * currentSpeedMove;
                rigidbodyGameObject.MoveRotation(GetRotation(moveDirection));
                isTrigger = true;
            }

        }

        private void MoveInTargetCircle()
        {
            if (targetPosition == Vector3.zero)
            {
                TargetSelect();
                return;
            }

            if (moveDirection.magnitude <= 1.1f && isTrigger)
            {
                rigidbodyGameObject.velocity = Vector3.zero;
                moveDirection = targetPosition - this.gameObject.transform.position;
                rigidbodyGameObject.MoveRotation(GetRotation(moveDirection));
                TargetSelect();
                isTrigger = !isTrigger;
            }
            else
            {
                moveDirection = new Vector3(targetPosition.x + Mathf.Cos(angle) * radius,
                                            transform.position.y,
                                            targetPosition.z + Mathf.Sin(angle) * radius);

                rigidbodyGameObject.MovePosition(this.gameObject.transform.forward + moveDirection);
                angle = angle + Time.deltaTime * currentSpeedMove;
                if (angle >= 360f) { angle = 0; }

                rigidbodyGameObject.MoveRotation(GetRotation(moveDirection - targetPosition));//
                isTrigger = true;
            }
        }
    }
}
