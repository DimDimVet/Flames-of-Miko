using UnityEngine;
using Zenject;

namespace TemleLogic
{
    public class TempleScaner : MonoBehaviour
    {
        [SerializeField] private TempleScanerSettings settings;
        private float diametrCollider;
        private int thisHash;
        private Collider[] hitColl;

        private bool isStopClass = false, isRun = false;

        private ITempleScanerExecutor scan;
        [Inject]
        public void Init(ITempleScanerExecutor _scan, ITempleExecutor _templeExecutor)
        {
            scan = _scan;
        }

        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (settings!=null)
                {
                    thisHash = gameObject.GetHashCode();
                    diametrCollider = settings.DiametrCollider;
                    isRun = true;
                }
                else { isRun = false; }
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
            DetectObject();
        }
        private void DetectObject()
        {
            hitColl = Physics.OverlapSphere(this.gameObject.transform.position, diametrCollider);
            scan.FindPlayer(hitColl, thisHash);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, diametrCollider);
        }
    }
}
