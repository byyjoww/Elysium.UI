using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.UI
{
    public interface IPopup
    {
        IPopup Create(Transform _parent);
        void Setup(PopupConfig _popup);
        void SetActionAdditive(UnityAction _action);
        void Dispose();
    }
}