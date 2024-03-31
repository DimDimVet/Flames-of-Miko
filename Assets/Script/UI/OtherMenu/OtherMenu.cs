using StatisticPlayer;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class OtherMenu : MonoBehaviour
    {
        [Header("Кнопка в Меню")]
        [SerializeField] private Button menuButton;
        [SerializeField] private Text menuText;
        [SerializeField] private Text specialMarksText;

        [SerializeField] private string startRezult;
        [SerializeField] private string currentRezult;
        [SerializeField] private string topRezult;

        private Statistic tempStat;

        private bool isSpecialMark=false;
        [Header("Скорость вывода текста"), Range(0, 5)]
        [SerializeField] private float timer;
        [Header("Цикличность")]
        [SerializeField] private bool loop = false;
        private string mainName;
        private int index;
        private float countTime = 0;
        private bool isStop = false, isRunWrite = false;

        private bool isStopClass = false, isRun = false;

        private IStatisticExecutor statistic;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels, IStatisticExecutor _statistic)
        {
            panels = _panels;
            statistic = _statistic;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (panels != null)
                {
                    isRun = true;
                    SetEventButton();

                    statistic.InitStatistic();
                    tempStat = statistic.GetStatistic();
                    if (tempStat.SpecialMarks == 0) { mainName = startRezult; }
                    if (tempStat.SpecialMarks == -1) { mainName = currentRezult; }
                    if (tempStat.SpecialMarks == 1) { mainName = topRezult; }
                    menuText.text = String.Format("{0:00}:{1:00}.{2:00}", tempStat.CurrentHour, tempStat.CurrentMinute, tempStat.CurrentSecund);
                    isSpecialMark = true;
                }
                else { isRun = false; }
            }
        }
        private void SetEventButton()
        {
            menuButton.onClick.AddListener(MainMenuPanel);
        }
        private void MainMenuPanel()
        {
            panels.MainMenu();
            panels.AudioClick();
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
            if (isSpecialMark) { UpTimer(); }
            
        }
        //
        private void AddWrite(bool _isRun)
        {
            if (!isStop & _isRun & mainName != "" & specialMarksText != null)
            {
                index++;
                if (index <= mainName.Length)
                {
                    specialMarksText.text = mainName.Substring(0, index);
                    panels.AudioClick();
                }
                else
                {
                    if (loop) { index = 0; }
                    else { isStop = true; return; }
                }
            }
        }
        private void UpTimer()
        {
            if (isStop) { return; }
            if (countTime <= timer)
            {
                countTime += Time.deltaTime;
            }
            else
            {
                if (isRunWrite) { isRunWrite = !isRunWrite; }
                else { isRunWrite = !isRunWrite; AddWrite(isRunWrite); }
                countTime = 0;
            }
        }
    }
}

