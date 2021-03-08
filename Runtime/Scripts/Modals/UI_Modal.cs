using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.UI
{
    public class UI_Modal : MonoBehaviour
    {
        public GameObject Panel => gameObject;
        public Button DisableButton = default;
        public bool EnableOnStart = default;
    }
}