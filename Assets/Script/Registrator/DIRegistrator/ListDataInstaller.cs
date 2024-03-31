using Zenject;

namespace Registrator
{
    public class ListDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IListDataExecutor>().To<ListDataExecutor>().AsSingle().NonLazy();
        }
    }
}

