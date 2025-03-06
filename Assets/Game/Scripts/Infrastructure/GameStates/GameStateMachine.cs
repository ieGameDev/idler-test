using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Utils;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, DiContainer container)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, container),
                [typeof(LoadProgressState)] = new LoadProgressState(this, container.Single<IProgressService>(),
                    container.Single<ISaveLoadService>()),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, container.Single<IGameFactory>(),
                    container.Single<IProgressService>()),
                [typeof(GameLoopState)] = new GameLoopState(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _currentState?.Exit();

            TState state = GetStated<TState>();
            _currentState = state;

            return state;
        }

        private TState GetStated<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}