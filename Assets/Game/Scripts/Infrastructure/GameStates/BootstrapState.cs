using Game.Scripts.Utils;

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
    }
}