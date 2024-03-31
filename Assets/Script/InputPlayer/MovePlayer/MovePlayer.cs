using EntityLogic;
using FMOD.Studio;
using FMODUnity;
using UI;
using UnityEngine;
using Zenject;

namespace Input
{
    public class MovePlayer : MonoBehaviour
    {
        [SerializeField] private MoveSettings settings;
        [SerializeField] private GameObject probnucGroundRay;
        [SerializeField][Range(0,10)] private float forceY=1f;

        private Rigidbody rigidbodyGameObject;
        private float speedMove, speedTurn;
        private Vector3 moveDirection, directionY;
        private Quaternion deltaRotation, directionRotation;
        private bool isStopClass = false, isRun = false;
        private int thisHash;
        private float countTimeBaf;
        private float percentSpeed;
        private Ray ray;
        private RaycastHit hit;
        private bool isTriggerTimer = false, isTriggerAction = true,isTrigerStep=true;

        private ILogicEntityExecutor logicEntityExecutor;
        private IInputPlayerExecutor inputs;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IInputPlayerExecutor _inputs, ILogicEntityExecutor _logicEntityExecutor, IPanelsExecutor _panels)
        {
            logicEntityExecutor = _logicEntityExecutor;
            inputs = _inputs;
            panels = _panels;
        }
        private void OnEnable()
        {
            logicEntityExecutor.OnMinusSpeedBaf += MinusSpeedBaf;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                inputs.Enable();
                speedTurn = settings.SpeedTurn;
                speedMove = settings.SpeedMove;

                thisHash = gameObject.GetHashCode();
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
        void Update()
        {
            UpDataBuffer();
        }
        private void RunUpdate()
        {
            Move();
        }
        private void MinusSpeedBaf(float _percentSpeed, float _timeBaf)
        {
            if (isTriggerAction)
            {
                isTriggerAction = !isTriggerAction;
                percentSpeed = _percentSpeed;
                countTimeBaf = _timeBaf;
                isTriggerTimer = true;
                ExecutorInTime(isTriggerAction);
            }

        }
        public void UpDataBuffer()
        {
            if (isTriggerTimer)
            {
                if (countTimeBaf > 0)
                {
                    countTimeBaf -= Time.deltaTime;
                }
                else
                {
                    isTriggerTimer = !isTriggerTimer;
                    isTriggerAction = true;
                    ExecutorInTime(isTriggerAction);
                }
            }
        }
        private void ExecutorInTime(bool isAction)
        {
            if (!isAction)
            {
                speedMove = (speedMove / 100) * percentSpeed;
            }
            else { speedMove = settings.SpeedMove; inputs.OnNormSpeed(); }
        }
        private void Move()
        {
            ray = new Ray(probnucGroundRay.transform.position, -probnucGroundRay.transform.up);
            if (Physics.Raycast(ray, out hit))
            {
                directionY = hit.point - probnucGroundRay.transform.position;
            }

            if (inputs.Updata().Move.y > 0)
            {
                rigidbodyGameObject.velocity = new Vector3(0.71f, 0, 0.71f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }

            }
            if (inputs.Updata().Move.y < 0)
            {
                rigidbodyGameObject.velocity = new Vector3(-0.71f, 0, -0.71f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }
            if (inputs.Updata().Move.x > 0)
            {
                rigidbodyGameObject.velocity = new Vector3(0.71f, 0, -0.71f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }
            if (inputs.Updata().Move.x < 0)
            {
                rigidbodyGameObject.velocity = new Vector3(-0.71f, 0, 0.71f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }

            if (inputs.Updata().Move.y > 0 && inputs.Updata().Move.x > 0)
            {
                rigidbodyGameObject.velocity = new Vector3(1f, 0, 0) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }
            if (inputs.Updata().Move.y > 0 && inputs.Updata().Move.x < 0)
            {
                rigidbodyGameObject.velocity = new Vector3(0, 0, 1f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }

            if (inputs.Updata().Move.y < 0 && inputs.Updata().Move.x < 0)
            {
                rigidbodyGameObject.velocity = new Vector3(-1f, 0, 0) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }
            if (inputs.Updata().Move.y < 0 && inputs.Updata().Move.x > 0)
            {
                rigidbodyGameObject.velocity = new Vector3(0, 0, -1f) * speedMove;
                if (directionY.y <= -1) { rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange); }
            }

            moveDirection = rigidbodyGameObject.velocity;

            if (inputs.Updata().Move.y != 0 || inputs.Updata().Move.x != 0)
            {
                if (isTrigerStep) { panels.AudioStep(thisHash, true); isTrigerStep = false; }

                deltaRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                directionRotation = Quaternion.RotateTowards(transform.rotation, deltaRotation, speedTurn);
                rigidbodyGameObject.MoveRotation(directionRotation);
            }
            else
            {
                if (!isTrigerStep) { panels.AudioStep(thisHash, false); isTrigerStep = true; }

                rigidbodyGameObject.velocity = directionY;
                rigidbodyGameObject.AddForce(directionY * forceY, ForceMode.VelocityChange);
            }
 
        }
    }
}

