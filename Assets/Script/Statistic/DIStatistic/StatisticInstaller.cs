using Zenject;

namespace StatisticPlayer
{
    public class StatisticInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IStatisticExecutor>().To<StatisticExecutor>().AsSingle().NonLazy();
        }
    }
}

