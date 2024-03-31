using EntityLogic;
using Input;
using Registrator;
using System;
using TemleLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIInformationDeBaf : MonoBehaviour
    {
        [SerializeField] private Image speedImg;
        [SerializeField] private Image faithImg;
        [Header("Таймер индикации вера")]
        [Range(0, 5)]
        [SerializeField] private float timerTikImg;
        private float countTimeBaf;

        [SerializeField] protected GameObject trackingObject;
        private Camera currentCamera;
        private Construction cameraObject;
        protected int thisHash, currentRecipientHash;
        private Canvas canvas;

        private bool isTriggerActionSpeed = true, isTriggerActionFaith = false;
        protected bool isStopClass = false, isRun = false;

        private IPanelsExecutor panels;
        private ILogicEntityExecutor logicEntityExecutor;
        private IListDataExecutor dataList;
        private IInputPlayerExecutor inputs;
        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(IListDataExecutor _dataList, IPanelsExecutor _panels,
                         ILogicEntityExecutor _logicEntityExecutor, IInputPlayerExecutor _inputs,
                         ITempleExecutor _templeExecutor)
        {
            logicEntityExecutor = _logicEntityExecutor;
            dataList = _dataList;
            inputs = _inputs;
            panels = _panels;
            templeExecutor = _templeExecutor;
        }
        private void OnEnable()
        {
            thisHash = trackingObject.GetHashCode();

            logicEntityExecutor.OnMinusSpeedBaf += MinusSpeedBaf;
            inputs.OnNormSpeed += NormSpeedBaf;

            templeExecutor.OnFaithDamage += FaithDamage;
        }

        private void FaithDamage(float obj)
        {
            isTriggerActionFaith = true;
            faithImg.enabled = true;
        }
        private void MinusSpeedBaf(float _percentSpeed, float _timeBaf)
        {
            if (isTriggerActionSpeed)
            {
                speedImg.enabled = true;
                isTriggerActionSpeed = false;
            }
        }
        private void NormSpeedBaf()
        {
            if (!isTriggerActionSpeed)
            {
                speedImg.enabled = false;
                isTriggerActionSpeed = true;
            }
        }
        void Start()
        {
            SetClass();
        }
        protected virtual void SetClass()
        {
            if (!isRun)
            {
                if (trackingObject != null)
                {
                    cameraObject = dataList.GetCamera();
                    currentCamera = cameraObject.CameraComponent;

                    speedImg.enabled = false;
                    faithImg.enabled = false;

                    if (currentCamera == null) { isRun = false; return; }
                    canvas = GetComponent<Canvas>();
                    canvas.planeDistance = 1;
                    canvas.worldCamera = currentCamera;

                    isRun = true;
                }
                else { isRun = false; }
            }
        }

        private void VisualFaith()
        {
            if (countTimeBaf > 0)
            {
                countTimeBaf -= Time.deltaTime;
            }
            else
            {
                countTimeBaf = timerTikImg;
                faithImg.enabled = false;
                isTriggerActionFaith = false;
            }
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
            if (isTriggerActionFaith) { VisualFaith(); }
        }
        protected virtual void LateUpdate()
        {
            gameObject.transform.LookAt(currentCamera.transform);
        }
    }
}

