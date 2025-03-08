using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Logic.CustomerSpawnLogic
{
    public class CustomerReturnTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.Characters.CustomerTag)) 
                return;
            
            Destroy(other.gameObject);
        }
    }
}