using System;
using Game.Scripts.Infrastructure.Services.UI;
using Game.Scripts.UI.Windows;

namespace Game.Scripts.Logic.ScriptableObjects
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowID;
        public BaseWindow Window;
    }
}