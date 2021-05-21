using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Elysium.UI
{
    public class UI_ScreenManager : MonoBehaviour
    {
        [SerializeField] private UI_Screen[] Screens = new UI_Screen[0];

        public GameObject[] Panels => Screens.Where(x => x.Panel != null).Select(x => x.Panel.gameObject).ToArray();
        public Button[] Buttons => Screens.Where(x => x.EnableButton != null).Select(x => x.EnableButton).ToArray();

        private void Awake()
        {
            for (int i = 0; i < Screens.Length; i++)
            {
                SetupScreen(Screens[i]);
            }
        }

        private void SetupScreen(UI_Screen _config)
        {            
            EnableAllScreens();
            SetupCanvas(_config);
            SetupEnableButton(_config);
            SetupDisableButton(_config);
            HandleStartStatus(_config);
        }

        private void EnableAllScreens()
        {
            foreach(var s in Screens)
            {
                s.gameObject.SetActive(true);
            }
        }
        
        private void SetupCanvas(UI_Screen _config)
        {
            _config.Panel.sortingOrder = _config.SortOrder;
        }

        private void SetupEnableButton(UI_Screen _config)
        {
            if (_config.EnableButton == null) { return; }
            List<GameObject> exceptions = Screens.Where(x => x.Panel != null && x.AlwaysActive).Select(x => x.Panel.gameObject).ToList();
            exceptions.Add(_config.Panel.gameObject);
            void DisableOtherScreensAndEnableThis() => DisableAllPanelsAndEnableExceptions(exceptions);
            _config.EnableButton.onClick.AddListener(DisableOtherScreensAndEnableThis);
        }

        private void SetupDisableButton(UI_Screen _config)
        {
            if (_config.DisableButton == null) { return; }
            void DisableThis() => _config.Panel.gameObject.SetActive(false);
            _config.DisableButton.onClick.AddListener(DisableThis);
        }

        private void HandleStartStatus(UI_Screen _config)
        {
            _config.Panel.gameObject.SetActive(_config.EnableOnStart);
        }

        private void DisableAllPanelsAndEnableExceptions(List<GameObject> _exceptions)
        {
            for (int i = 0; i < Panels.Length; i++)
            {
                if (_exceptions.Contains(Panels[i]))
                {
                    Panels[i].SetActive(true);
                    continue;
                }

                Panels[i].SetActive(false);
            }
        }

        private void OnValidate()
        {
            Screens = GetComponentsInChildren<UI_Screen>();
        }
    }
}
