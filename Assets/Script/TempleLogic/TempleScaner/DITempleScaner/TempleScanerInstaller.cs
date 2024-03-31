using Zenject;

namespace TemleLogic
{
    public class TempleScanerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITempleScanerExecutor>().To<TempleScanerExecutor>().AsSingle().NonLazy();
        }
    }
}

