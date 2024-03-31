using Registrator;
using TemleLogic;
using UnityEngine;
using Zenject;

namespace Input
{
    public class ExecutorPlayer : MonoBehaviour
    {
        private int thisHash;
        private bool isStopClass = false, isRun = false;

        private ITempleExecutor templeExecutor;
        private IInputPlayerExecutor inputs;
        private ITempleScanerExecutor templeScaner;
        [Inject]
        public void Init(IInputPlayerExecutor _inputs, ITempleScanerExecutor _templeScaner,
                         ITempleExecutor _templeExecutor)
        {
            templeExecutor = _templeExecutor;
            inputs = _inputs;
            templeScaner = _templeScaner;
        }
        private void OnEnable()
        {
            templeScaner.OnFindPlayer += FireTempleOn;
        }
        private void FireTempleOn(Construction player, int recipientHash)
        {
            if (thisHash == player.Hash)
            {
                if (inputs.Updata().Executor == 1 )
                {
                    templeExecutor.FireTemple(recipientHash);
                }
            }
        }

        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                thisHash = gameObject.GetHashCode();

                if (thisHash == 0)
                {
                    isRun = false;
                }
                else { isRun = true; }
            }
        }
        void FixedUpdate()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }

    }
}

