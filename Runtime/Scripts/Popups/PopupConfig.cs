using UnityEngine;
using UnityEngine.Events;

namespace Elysium.UI
{
    [System.Serializable]
    public class PopupConfig
    {
        public string Title;
        public string Description;
        public Sprite Icon;
        public UnityAction Action;

        // POPUP STYLE
        public IPopup Prefab;

        public PopupConfig(string title, string description, Sprite icon = null, UnityAction action = null)
        {
            Title = title;
            Description = description;
            Icon = icon;
            Action = action;
        }
    }
}