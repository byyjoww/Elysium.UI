using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Elysium.UI
{
    public class UI_Canvas : MonoBehaviour
    {
        [Space]
        [SerializeField] private bool sortOrderBasedOnHierarchy = true;
        [SerializeField] private UI_Screen[] screens = new UI_Screen[0];

        public GameObject[] Panels => screens.Where(x => x.Panel != null).Select(x => x.Panel.gameObject).ToArray();

        private void Awake()
        {
            for (int i = 0; i < screens.Length; i++)
            {
                SetupScreen(screens[i]);
            }
        }

        private void SetupScreen(UI_Screen _config)
        {
            List<GameObject> exceptions = screens
                .Where(x => x.Panel != null)
                .Where(x => x.AlwaysActive || x == _config.Panel)
                .Select(x => x.Panel.gameObject)
                .ToList();

            _config.SetupCanvas();
            _config.SetupEnableButton(() => DisableAllPanelsAndEnableExceptions(exceptions));
            _config.SetupDisableButton();
            _config.HandleStartStatus();
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

        public void OnValidate()
        {
            screens = GetComponentsInChildren<UI_Screen>(true);
            if (FindObjectOfType<EventSystem>() == null)
            {
                Debug.Log("Missing event system in scene!");
            }

            if (sortOrderBasedOnHierarchy)
            {
                for (int i = 0; i < screens.Length; i++)
                {
                    screens[i].SortOrder = i;
                }
            }
        }
    }
}

namespace Elysium.UI
{
    using Elysium.Utils;
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CustomEditor(typeof(UI_Canvas))]
    public class UI_CanvasEditor : Editor
    {
        private UI_Canvas canvas = default;

        private void OnEnable()
        {
            canvas = (UI_Canvas)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("New Screen")) { CreateNewScreen(); }
        }

        private void CreateNewScreen()
        {
            string path = "Screen";
            GameObject screen = Resources.Load<GameObject>(path);
            GameObject obj = Instantiate(screen, canvas.transform);
            obj.name = "New Screen";
            if (obj == null) { Debug.LogError($"Unable to load screen at path: '{path}' from resources"); }
            RectTransform objT = obj.transform as RectTransform;
            objT.localScale = Vector3.one;
            objT.Stretch();
            canvas.OnValidate();
        }
    }
}
