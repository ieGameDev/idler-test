using Game.Scripts.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public class MenuSceneLoader : MonoBehaviour
    {
        [MenuItem(Constants.MenuItemNames.HeaderName + "/Scenes/Initial", priority = 0)]
        public static void LoadInitial() =>
            TryLoadScene(0);

        [MenuItem(Constants.MenuItemNames.HeaderName + "/Scenes/Main", priority = 1)]
        public static void LoadMain() =>
            TryLoadScene(1);

        private static void TryLoadScene(int index)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[index].path);
        }
    }
}