using Zenject;

namespace EntityLogic
{
    public class LogicEntityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILogicEntityExecutor>().To<LogicEntityExecutor>().AsSingle().NonLazy();
        }
    }
}