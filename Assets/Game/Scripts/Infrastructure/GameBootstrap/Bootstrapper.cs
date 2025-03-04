using Game.Scripts.Infrastructure.GameStates;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameBootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new GameStateMachine(new SceneLoader());
            _stateMachine.Enter<BootstrapState>();
        }
    }
}