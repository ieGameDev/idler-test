using Game.Scripts.Infrastructure.GameStates;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameBootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new GameStateMachine();
            _stateMachine.Enter<BootstrapState>();
        }
    }
}