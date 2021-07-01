using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Elysium.Utils.Attributes;

namespace Elysium.UI
{
    public class UI_Screen : MonoBehaviour
    {
        [SerializeField] private Canvas panel = default;
        [SerializeField] private Button enableButton = default;
        [SerializeField] private Button disableButton = default;
        [SerializeField] private int sortOrder = 0;
        [SerializeField] private bool alwaysActive = default;
        [SerializeField] [ConditionalField("alwaysActive", true)] private bool enableOnStart = default;

        public Canvas Panel => panel;
        public bool AlwaysActive => alwaysActive;
        public int SortOrder { get => sortOrder; set => sortOrder = value; }

        public void SetupCanvas()
        {
            Panel.overrideSorting = true;
            Panel.sortingOrder = sortOrder;
        }

        public void HandleStartStatus()
        {
            Panel.gameObject.SetActive(enableOnStart || alwaysActive);
        }

        public void SetupEnableButton(UnityAction _onClick)
        {
            if (enableButton == null) { return; }            
            enableButton.onClick.AddListener(_onClick);
        }

        public void SetupDisableButton()
        {
            if (disableButton == null) { return; }
            disableButton.onClick.AddListener(delegate { Panel.gameObject.SetActive(false); });
        }

        private void OnValidate()
        {
            if (panel == null) { panel = GetComponent<Canvas>(); }
            if (alwaysActive) { enableOnStart = false; }
        }
    }
}