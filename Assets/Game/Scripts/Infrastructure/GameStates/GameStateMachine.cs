using System;
using System.Collections.Generic;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine()
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(),
                [typeof(LoadLevelState)] = new LoadLevelState(),
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