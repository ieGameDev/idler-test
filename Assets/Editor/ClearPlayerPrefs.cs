using Game.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ClearPlayerPrefs : MonoBehaviour
    {
        [MenuItem(Constants.MenuItemNames.HeaderName + "/Clear PlayerPrefs", priority = 2)]
        public static void ClearPlayPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}