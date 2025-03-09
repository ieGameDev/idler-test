using UnityEngine;

namespace Game.Scripts.UI
{
    public class JoystickCanvasSetting : MonoBehaviour
    {
        private void Start()
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 10;
        }
    }
}