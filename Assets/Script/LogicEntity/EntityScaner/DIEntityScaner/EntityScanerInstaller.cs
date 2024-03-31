using Zenject;

namespace EntityLogic
{
    public class EntityScanerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEntityScanerExecutor>().To<EntityScanerExecutor>().AsSingle().NonLazy();
        }
    }
}

