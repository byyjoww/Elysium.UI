using Elysium.Utils;
using Elysium.Utils.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Elysium.UI
{
    [CreateFromPrefab("PopupCanvas")]
    public class PopupManager : Singleton<PopupManager>
    {
        [Separator("Debug", true)]
        [SerializeField, ReadOnly] private PopupConfig DEBUGActivePopup = default;
        [SerializeField, ReadOnly] private PopupConfig[] DEBUGQueuedPopups = default;

        [Separator("Styles", true)]
        [SerializeField] private Popup genericPopupPrefab = default;

        public event UnityAction OnPopupAdded;
        public event UnityAction OnActivePopupClosed;
        
        private PopupConfig activePopup = default;
        private Queue<PopupConfig> popupQueue = default;        


        // ---------------- GENERIC POPUP STYLE ----------------

        public static void CreateGenericPopup(PopupConfig _popup) => Instance.EnqueuePopup(_popup, Instance.genericPopupPrefab);

        // -----------------------------------------------------

        protected override void Awake()
        {
            SetupCanvas();
            base.Awake();
            popupQueue = new Queue<PopupConfig>();
            DEBUGQueuedPopups = popupQueue.ToArray();
            activePopup = null;
            DEBUGActivePopup = null;
            OnPopupAdded += TryInstantiatePopup;
            OnActivePopupClosed += ResetActivePopup;
        }

        private void SetupCanvas()
        {
            Canvas c = gameObject.GetComponent<Canvas>(); 
            if (c == null) { c = gameObject.AddComponent<Canvas>(); }
            c.sortingOrder = 999;

            CanvasScaler cs = gameObject.GetComponent<CanvasScaler>();
            if (cs == null) { cs = gameObject.AddComponent<CanvasScaler>(); }

            GraphicRaycaster gr = gameObject.GetComponent<GraphicRaycaster>();
            if (gr == null) { gr = gameObject.AddComponent<GraphicRaycaster>(); }
        }

        private void EnqueuePopup(PopupConfig _popup, IPopup _style)
        {
            _popup.Prefab = _style;
            popupQueue.Enqueue(_popup);
            OnPopupAdded?.Invoke();
        }

        private void InstantiatePopup()
        {
            PopupConfig popup = popupQueue.Dequeue();
            // Debug.Log("dequeued popup " + popup.Description);
            IPopup runtimePopup = popup.Prefab.Create(transform);
            runtimePopup.Setup(popup);
            runtimePopup.SetActionAdditive(GetBaseAction(runtimePopup));
            activePopup = popup;
            DEBUGActivePopup = popup;
            TryInstantiatePopup();
        }

        private void ResetActivePopup()
        {
            activePopup = null;
            DEBUGActivePopup = null;
            TryInstantiatePopup();
        }

        private void TryInstantiatePopup()
        {
            DEBUGQueuedPopups = popupQueue.ToArray();
            if (popupQueue.Count < 1) { return; }
            if (activePopup != null) { return; }
            InstantiatePopup();
        }

        private UnityAction GetBaseAction(IPopup _runtimePopup)
        {
            void hasClosed() => OnActivePopupClosed?.Invoke();
            void destroySelf() => _runtimePopup.Dispose();

            UnityAction baseAction = null;
            baseAction += destroySelf;
            baseAction += hasClosed;
            return baseAction;
        }

        [ContextMenu("Enqueue Dummy Popup")]
        private void EnqueueDummyPopup()
        {
            var popup = new PopupConfig("Dummy Popup", "This is a dummy popup.", null, null);
            PopupManager.CreateGenericPopup(popup);
        }
    }
}