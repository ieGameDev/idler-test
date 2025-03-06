using Game.Scripts.Infrastructure.DI;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public class SaveTrigger : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;
        private void Awake() =>
            _saveLoadService = DiContainer.Instance.Single<ISaveLoadService>();

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            gameObject.SetActive(false);
        }
    }
}