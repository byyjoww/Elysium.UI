using UnityEngine;
using UnityEngine.UI;

namespace Elysium.UI
{
    public class UI_Screen : MonoBehaviour
    {
        public Canvas Panel = default;
        public Button EnableButton = default;
        public Button DisableButton = default;
        public int SortOrder = 0;
        public bool EnableOnStart = default;
        public bool AlwaysActive = default;

        private void OnValidate()
        {
            if (Panel == null) { Panel = GetComponent<Canvas>(); }
        }
    }
}