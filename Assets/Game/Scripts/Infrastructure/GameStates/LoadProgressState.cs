using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameStates
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(GameStateMachine gameStateMachine, IProgressService progressService,
            ISaveLoadService saveLoadService, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            PlayerUpgradesData playerData = _staticDataService.ForPlayerUpgrades();

            PlayerProgress progress = new PlayerProgress()
            {
                UpgradesData =
                {
                    Capacity = playerData.PlayerCapacityLevel1,
                    PlayerLevel = 1
                }
            };
            
            return progress;
        }
    }
}