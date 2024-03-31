using Registrator;
using UI;
using UnityEngine;
using Zenject;

namespace TemleLogic
{
    public class TempleFire : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;
        private int thisHash;
        private Construction[] temples;
        
        private bool isStopClass = false, isRun = false;

        private IPanelsExecutor panels;
        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(ITempleExecutor _templeExecutor, IPanelsExecutor _panels)
        {
            templeExecutor = _templeExecutor;
            panels = _panels;
        }
        private void Awake()
        {
            if (particleSystems != null)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Stop();
                }
            }
        }
        private void OnEnable()
        {
            templeExecutor.OnFireTemple += FireTemple;
            templeExecutor.OnOffTemples += SetFireTemple;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (particleSystems != null)
                {
                    thisHash = gameObject.GetHashCode();
                    templeExecutor.SetData(thisHash);

                    for (int i = 0; i < particleSystems.Length; i++)
                    {
                        particleSystems[i].Play();
                        panels.AudioFire(thisHash, true);
                    }

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
        }
        private void FireTemple(Construction temple, Construction[] temples)
        {
            if (thisHash == temple.Hash ) { StartFireTemple(temple); }
        }
        private void StartFireTemple(Construction temple)
        {
            temples = templeExecutor.GetTemples();
            for (int i = 0; i < temples.Length; i++)
            {
                if (temples[i].Hash == thisHash && temples[i].StatusTemle != StatusTemple.Destoy)
                {
                    if (temples[i].StatusTemle == StatusTemple.One) { temples[i].StatusTemle = StatusTemple.Two; }
                    if (temples[i].StatusTemle == StatusTemple.Null) { temples[i].StatusTemle = StatusTemple.Two; }
                }
            }
            templeExecutor.SetTemples(temples);

            if (particleSystems != null)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i].isStopped)
                    {
                        particleSystems[i].Play();
                        panels.AudioFire(thisHash,true);//
                        //return;
                    }
                }
            }
        }
        private void SetFireTemple(Construction[] temples)
        {
            for (int i = 0; i < temples.Length; i++)
            {
                if (thisHash == temples[i].Hash ) {  OffFireTemple(temples[i]); }
            }
        }
        private void OffFireTemple(Construction temple)
        {
            Construction[] temples = templeExecutor.GetTemples();
            for (int i = 0; i < temples.Length; i++)
            {
                if (temples[i].Hash == temple.Hash && temples[i].StatusTemle == StatusTemple.Destoy)
                {
                    this.gameObject.SetActive(false);
                }
            }

            if (particleSystems != null && temple.StatusTemle != StatusTemple.Two && temple.StatusTemle != StatusTemple.Destoy)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i].isPlaying)
                    { 
                        particleSystems[i].Stop();
                        ContolOffAudio();
                        return; 
                    }
                }
            }
        }
        private void ContolOffAudio()
        {
            panels.AudioFireOff(thisHash, true);
            for (int j = 0; j < particleSystems.Length; j++)
            {
                if (particleSystems[j].isEmitting) { return; }
            }
            panels.AudioFire(thisHash,false); 
        }
        private void OnDisable()
        {
            templeExecutor.OnFireTemple -= FireTemple;
            templeExecutor.OnOffTemples -= SetFireTemple;
        }
    }
}
