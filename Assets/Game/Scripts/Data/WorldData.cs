using System;
using UnityEngine;

namespace Game.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public Vector3 PlayerPosition;
        public CashData CashData = new();
        public CookingAreasData CookingAreasData = new();
    }
}