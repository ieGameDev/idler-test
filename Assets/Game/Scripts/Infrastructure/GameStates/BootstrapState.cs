using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly DiContainer _container;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, DiContainer container)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _container = container;

            RegisterServices();
            InitialInputService();
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(Constants.Levels.Initial, EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            RegisterStaticData();
            _container.RegisterSingle(InitialInputService());
            _container.RegisterSingle<IProgressService>(new ProgressService());
            _container.RegisterSingle<IGameFactory>(new GameFactory(_container.Single<IInputService>(),
                _container.Single<IStaticDataService>(), _container.Single<IProgressService>()));
            _container.RegisterSingle<ISaveLoadService>(new SaveLoadService(_container.Single<IProgressService>(),
                _container.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();

            staticData.LoadCustomer();
            staticData.LoadDishes();
            staticData.LoadCookingAreas();
            staticData.LoadPurchasableAreas();

            _container.RegisterSingle(staticData);
        }

        private IInputService InitialInputService()
        {
            if (Application.isEditor)
                return new DesktopInput();

            return new MobileInput();
        }
    }
}