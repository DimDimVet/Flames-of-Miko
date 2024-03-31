using Zenject;

namespace TemleLogic
{
    public class TempleExecutorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITempleExecutor>().To<TempleExecutor>().AsSingle().NonLazy();
        }
    }
}

