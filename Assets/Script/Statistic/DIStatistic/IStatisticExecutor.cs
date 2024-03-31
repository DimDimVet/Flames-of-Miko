using System;

namespace StatisticPlayer
{
    public interface IStatisticExecutor
    {
        Action<Statistic> OnUpdateStatistic { get; set; }

        bool InitStatistic();
        void SetStatistic(Statistic statistic);
        Statistic GetStatistic();
        void OnDisable();
    }
}