using UnityEngine;
using UnityEngine.Events;

namespace Elysium.UI
{
    public class UI_Demo : MonoBehaviour
    {
        private void Start()
        {
            CreatePopup();
        }

        private void CreatePopup()
        {
            Sprite example = Resources.Load<Sprite>("white_pixel");
            UnityAction action = () => Debug.Log("This action happens when you click the confirmation button");
            PopupConfig popup = new PopupConfig("Tutorial", "This is a tutorial message", example, action);
            PopupManager.CreateGenericPopup(popup);
        }
    }
}