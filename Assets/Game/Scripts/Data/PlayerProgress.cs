using System;

namespace Game.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData = new();
        public UpgradesData UpgradesData = new();
    }
}