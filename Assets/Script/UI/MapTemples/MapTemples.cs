using Registrator;
using TemleLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    struct ObjectMap
    {
        public int Hash;
        public Image Image;
        public bool isOne;
    }
    public class MapTemples : MonoBehaviour
    {
        [Header("Таймер индикации храма")]
        [Range(0, 5)]
        [SerializeField] private float timerTikImg;

        [Header("Активное состояние")]
        [SerializeField] private Sprite onImg;
        [Header("Отключенное состояние")]
        [SerializeField] private Sprite offImg;
        [Header("Объекты карты")]
        [SerializeField] private Image[] mapImages;
        [Header("Связанные объекты")]
        [SerializeField] private GameObject[] gameObjectsImg;
        private Construction[] temples;
        private ObjectMap[] objectMaps;
        private Color colorOne;
        private float countTimeBaf;

        private bool isStarter = false;

        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(ITempleExecutor _templeExecutor)
        {
            templeExecutor = _templeExecutor;
        }
        private void OnEnable()
        {
            templeExecutor.OnFireTemple += FireTemple;
            templeExecutor.OnOffTemples += SetFireTemple;
        }
        private void Starter()
        {
            temples = templeExecutor.GetTemples();
            colorOne=new Color(1,1,1,1);
            if (temples != null && temples.Length == mapImages.Length)
            {
                isStarter = true;
                objectMaps = new ObjectMap[gameObjectsImg.Length];
                for (int i = 0; i < gameObjectsImg.Length; i++)
                {
                    objectMaps[i].Hash = gameObjectsImg[i].GetHashCode();
                    objectMaps[i].Image = mapImages[i];
                }
            }
            else { isStarter = false; return; }
        }
        private void FixedUpdate()
        {
            if (!isStarter) { Starter(); }
            UpDataTimer();
        }
        private void UpDataTimer()
        {
            if (countTimeBaf > 0)
            {
                countTimeBaf -= Time.deltaTime;
            }
            else
            {
                countTimeBaf = timerTikImg;
                for (int i = 0; i < objectMaps.Length; i++)
                {
                    if (objectMaps[i].isOne && objectMaps[i].Image.color.a == 1) { colorOne.a = 0; objectMaps[i].Image.color= colorOne; }
                    else if (objectMaps[i].isOne && objectMaps[i].Image.color.a == 0) { colorOne.a = 1; objectMaps[i].Image.color = colorOne; }

                    if (!objectMaps[i].isOne) { colorOne.a = 1; objectMaps[i].Image.color = colorOne; }
                }
            }
        }

        private void MapUpdate()
        {
            for (int i = 0; i < temples.Length; i++)
            {
                SetMapObject(temples[i]);
            }
        }
        private void SetMapObject(Construction temple)
        {
            for (int i = 0; i < objectMaps.Length; i++)
            {
                if (objectMaps[i].Hash == temple.Hash)
                {
                    if (temple.StatusTemle == StatusTemple.Destoy) { objectMaps[i].Image.sprite = offImg; objectMaps[i].isOne = false; }
                    if (temple.StatusTemle == StatusTemple.Null) { objectMaps[i].Image.sprite = offImg; objectMaps[i].isOne = false; }
                    if (temple.StatusTemle == StatusTemple.Two) { objectMaps[i].Image.sprite = onImg; objectMaps[i].isOne = false; }
                    if (temple.StatusTemle == StatusTemple.One) { objectMaps[i].isOne = true; }
                }
            }
        }
        private void SetFireTemple(Construction[] _temples)
        {
            temples = _temples;
            MapUpdate();
        }
        private void FireTemple(Construction temple, Construction[] temples)
        {
            for (int i = 0; i < temples.Length; i++)
            {
                if (temples[i].Hash == temple.Hash) { temples[i].StatusTemle= StatusTemple.Two; }
            }
            MapUpdate();
        }
    }
}
