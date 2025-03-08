using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CustomerData", menuName = "ScriptableObjects/CustomerData")]
    public class CustomerData : ScriptableObject
    {
        [Header("Order Waiting Time")] 
        public float WaitingTime;
        
        [Header("Eating Time")]
        public float EatingTime;
        
        [Header("Customer Spawn Delay")]
        public float SpawnDelay;
        
        [Header("Prefabs")]
        public List<GameObject> CustomerPrefabs;
    }
}