using Game.Scripts.Infrastructure.Services.Input;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            
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
            _stateMachine.Enter<LoadLevelState>();
        
        private IInputService InitialInputService()
        {
            if (Application.isEditor)
                return new DesktopInput();

            return new MobileInput();
        }
    }
}