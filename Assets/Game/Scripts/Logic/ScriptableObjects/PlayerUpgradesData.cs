using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerUpgradesData", menuName = "ScriptableObjects/PlayerUpgrades")]
    public class PlayerUpgradesData : ScriptableObject
    {
        public int PlayerCapacityLevel1;
        public int PlayerCapacityLevel2;
        public int PlayerCapacityLevel3;
    }
}