using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "ScriptableObjects/WindowStaticData")]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }
}