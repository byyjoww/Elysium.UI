using Elysium.Utils;
using Elysium.Utils.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.UI
{
    public class UI_PopupManager : Singleton<UI_PopupManager>
    {
        [Separator("Debug", true)]
        [SerializeField, ReadOnly] private Popup DEBUGActivePopup = default;
        [SerializeField, ReadOnly] private Popup[] DEBUGQueuedPopups = default;

        [Separator("Styles", true)]
        [SerializeField] private UI_Popup genericPopupPrefab = default;

        public event UnityAction OnPopupAdded;
        public event UnityAction OnActivePopupClosed;
        
        private Popup activePopup = default;
        private Queue<Popup> popupQueue = default;        


        // ---------------- GENERIC POPUP STYLE ----------------

        public static void CreateGenericPopup(Popup _popup) => Instance.EnqueuePopup(_popup, Instance.genericPopupPrefab);

        // -----------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            popupQueue = new Queue<Popup>();
            DEBUGQueuedPopups = popupQueue.ToArray();
            activePopup = null;
            DEBUGActivePopup = null;
            OnPopupAdded += TryInstantiatePopup;
            OnActivePopupClosed += ResetActivePopup;
        }

        private void EnqueuePopup(Popup _popup, IPopup _style)
        {
            _popup.Prefab = _style;
            popupQueue.Enqueue(_popup);
            OnPopupAdded?.Invoke();
        }

        private void InstantiatePopup()
        {
            Popup popup = popupQueue.Dequeue();
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
            var popup = new Popup("Dummy Popup", "This is a dummy popup.", null, null);
            UI_PopupManager.CreateGenericPopup(popup);
        }
    }
}