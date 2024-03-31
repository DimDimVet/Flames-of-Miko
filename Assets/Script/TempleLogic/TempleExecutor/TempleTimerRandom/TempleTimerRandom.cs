using ModestTree;
using Registrator;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace TemleLogic
{
    public class TempleTimerRandom : MonoBehaviour
    {
        [Header("Время, через которое сработает выбор храма")]
        [SerializeField] private float timeOffsetForDestroy;
        [Header("Конец таймера")]
        [SerializeField] private float timerEndValue;
        [Header("Время, через которое погаснет вторая свеча храма")]
        [SerializeField] private float secondFireTime;
        private float timerValue;
        [Header("Процент увеличения скорости сущностей за итерацию")]
        [SerializeField] private float propercentSpeed;
        [Header("Время итерации увелечения скорости сущности (сек)")]
        [SerializeField] private float timeIteration = 60f;
        [Header("Максимальный процент")]
        [SerializeField] private float propercentMaxSpeed = 200f;
        private float entityCurSpeed;
        //Random
        private float timeCheckpointRandom;
        private float timeCheckpointSpeed;
        private Queue<int> queue;
        private List<int> ints;

        private Construction[] temples;
        private float[] toDestroyCheckpoint;

        private float clock, countClockSec = 0f, countClockMin = 0f, countClockHour = 0f;

        private float faithValue;
        [Header("Настройки веры")]
        [SerializeField] private float maxFaith;

        private int inactiveTemples;
        [SerializeField] private float faithDamage;
        [SerializeField] private float riseSpeed;

        private float checkpointForUI;
        private bool isStarter = false;

        private IPanelsExecutor panels;
        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(ITempleExecutor _templeExecutor, IPanelsExecutor _panels)
        {
            templeExecutor = _templeExecutor;
            panels = _panels;
        }
        private void OnEnable()
        {
            templeExecutor.OnFaithDamage += FaithDamage;
        }

        void Start()
        {

        }
        private void Starter()
        {
            temples = GetTemples();
            checkpointForUI = Time.time;
            timeCheckpointSpeed = Time.time;
            timeCheckpointRandom = timerValue;
            faithValue = maxFaith / 2;
            ints = new List<int> { 1, 1, 2 };
            queue = new Queue<int>(ints);

            toDestroyCheckpoint = new float[temples.Length];
            entityCurSpeed = 100;

            //
            clock = Time.time;
        }
        private void FixedUpdate()
        {
            if (!isStarter) { isStarter = !isStarter; Starter(); }
        }
        void Update()
        {
            Clock();
            TimerTick();
            FaithTick();
            EventExecutor();
            RandomLogic();
        }
        private void Clock()
        {
            if (clock + 1 <= Time.time)
            {
                clock = Time.time;
                countClockSec++;
                if (countClockSec >= 60) { countClockMin++; countClockSec = 0f; }
                if (countClockMin >= 60) { countClockHour++; countClockMin = 0f; }
                panels.SetClock(countClockSec, countClockMin, countClockHour);
            }
        }

        private void TimerTick()
        {
            if (timerValue >= 0 && timerValue <= timerEndValue)
                timerValue += Time.deltaTime;
        }

        private void FaithTick()
        {
            if (faithValue > maxFaith) faithValue = maxFaith;
            if (faithValue >= 0 && faithValue <= maxFaith)
            {
                if (inactiveTemples <= 0) faithValue += RiseFaith();
                if (inactiveTemples > 0) faithValue -= FaithDamage();
            }
        }

        #region Faith
        private float FaithDamage()
        {
            return faithDamage * inactiveTemples * Time.deltaTime;
        }

        private float RiseFaith()
        {
            return riseSpeed * Time.deltaTime;
        }

        //################################################
        /// <summary>
        /// Метод для выполнения урона вере от сущностей
        /// </summary>
        /// <param name="damageValue"></param>
        private void FaithDamage(float damageValue)
        {
            if (faithValue >= damageValue)
            {
                faithValue -= damageValue;
            }
            else
            {
                faithValue = 0;
            }
        }
        //################################################
        #endregion

        private void EventExecutor()
        {
            if (checkpointForUI + 1 <= Time.time)
            {
                CheckInactiveTemples();
                panels.SetSliders(faithValue, timerValue);
                checkpointForUI = Time.time;
                TemplesDestroyer();
            }
            if (timeCheckpointSpeed + timeIteration <= Time.time)
            {
                timeCheckpointSpeed = Time.time;
                //Сюда ивент, который говорит сущностям, что минута прошла и пора ускоряться на 10%
                if (entityCurSpeed <= propercentMaxSpeed) { entityCurSpeed += propercentSpeed; }
                else { entityCurSpeed = propercentMaxSpeed; }
                templeExecutor.PlusSpeedEntity(entityCurSpeed);
            }
        }

        #region RandomTemples
        private Construction[] GetTemples()
        {
            return templeExecutor.GetTemples();
        }

        private Construction[] GetActiveTemples()
        {
            return temples.Where(e => e.StatusTemle == StatusTemple.Two).ToArray();
        }

        private bool GetRandomTemple(out Construction construction)
        {
            Construction[] activeTemples = GetActiveTemples();
            if (activeTemples.Length > 0)
            {
                construction = activeTemples[Randomizer(0, activeTemples.Length)];
                return true;
            }
            else
            {
                construction = GetTemples()[0];
                return false;
            }
        }

        private void TempleStartDestroy(Construction target)
        {
            if (target.StatusTemle == StatusTemple.Two)
            {
                target.StatusTemle = StatusTemple.One;
            }
            //сохраним обновленное состояние
            for (int i = 0; i < temples.Length; i++)
            {
                if (temples[i].Hash == target.Hash) { temples[i].StatusTemle = target.StatusTemle; }
            }
            templeExecutor.OffTemples(temples);
        }

        private void RandomLogic()
        {

            if (timeCheckpointRandom + timeOffsetForDestroy <= timerValue)
            {
                timeCheckpointRandom = timerValue;
                RefreshQueue();
                int q = queue.Dequeue();

                for (int i = 0; i < q; i++)
                {
                    if (GetRandomTemple(out Construction construction))
                    {
                        TempleStartDestroy(construction);
                        foreach (var e in temples)
                        {
                            if (e.Hash == construction.Hash)
                                toDestroyCheckpoint[temples.IndexOf(e)] = Time.time;
                        }
                    }
                }
            }
        }

        private void TemplesDestroyer()
        {
            if (toDestroyCheckpoint == null) { return; }
            for (int i = 0; i < toDestroyCheckpoint.Length; i++)
            {
                if (toDestroyCheckpoint[i] > -1 &&
                    toDestroyCheckpoint[i] + secondFireTime <= Time.time)
                {
                    if (temples[i].StatusTemle == StatusTemple.One)
                    {
                        temples[i].StatusTemle = StatusTemple.Null;
                        templeExecutor.OffTemples(temples);
                    }
                    toDestroyCheckpoint[i] = -1;
                }
            }

        }

        private void RefreshQueue()
        {
            if (queue.Count <= 0)
            {
                foreach (var e in ints)
                {
                    queue.Enqueue(e);
                }
            }
        }

        private void CheckInactiveTemples()
        {
            inactiveTemples = GetTemples().Where(e => e.StatusTemle == StatusTemple.Null).Count();
        }

        private int Randomizer(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
        #endregion


    }
}
