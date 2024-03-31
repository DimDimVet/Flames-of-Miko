using Registrator;
using TemleLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIInformationE : MonoBehaviour
    {
        [Header("”казать выводимый текст")]
        [SerializeField] private string inText;

        [SerializeField] private Text dinamicText;
        [SerializeField] protected GameObject trackingObject;
        private Camera currentCamera;
        private Construction cameraObject;
        protected int thisHash, currentRecipientHash;
        private Canvas canvas;
        protected bool isStopClass = false, isRun = false;

        private IListDataExecutor dataList;
        private ITempleScanerExecutor templeScaner;
        [Inject]
        public void Init(ITempleScanerExecutor _templeScaner, IListDataExecutor _dataList)
        {
            dataList = _dataList;
            templeScaner = _templeScaner;
        }
        private void OnEnable()
        {
            thisHash = trackingObject.GetHashCode();
            templeScaner.OnFindPlayer += TextEnableOn;
            templeScaner.OnLossPlayer += TextEnableOff;
        }
        protected virtual void TextEnableOn(Construction player, int recipientHash)
        {
            if (dinamicText.enabled == false)
            {
                if (thisHash == player.Hash) { currentRecipientHash = recipientHash; dinamicText.enabled = true; }
            }
        }
        protected virtual void TextEnableOff(int recipientHash)
        {
            if (dinamicText.enabled == true && currentRecipientHash== recipientHash) { dinamicText.enabled = false; }
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

                    dinamicText.text = $"{inText}";
                    dinamicText.enabled = false;

                    if (currentCamera == null) { isRun = false; return; }
                    canvas = GetComponent<Canvas>();
                    canvas.planeDistance = 1;
                    canvas.worldCamera = currentCamera;

                    isRun = true;
                }
                else { isRun = false; }
            }
        }

        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
        protected virtual void LateUpdate()
        {
            gameObject.transform.LookAt(currentCamera.transform);
        }
    }
}

