using Zenject;

namespace UI
{
    public class PanelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPanelsExecutor>().To<PanelsExecutor>().AsSingle().NonLazy();
        }
    }
}

