using UnityEngine;

namespace Game.Scripts.UI
{
    public class CanvasSetting : MonoBehaviour
    {
        private void Start()
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 10;
        }
    }
}