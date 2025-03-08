using UnityEngine;

namespace Game.Scripts.Utils
{
    public class CashRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        
        private void Update() => 
            transform.Rotate(Vector3.up, Time.deltaTime * _rotationSpeed);
    }
}