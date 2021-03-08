using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Elysium.UI
{
    public class UI_Popup : MonoBehaviour, IPopup
    {
        [SerializeField] private TMP_Text titleTextComponent = default;
        [SerializeField] private TMP_Text descriptionTextComponent = default;
        [SerializeField] private Image iconImageComponent = default;
        [SerializeField] private Button buttonComponent = default;

        public void Setup(Popup _popup)
        {
            if (titleTextComponent != null) { titleTextComponent.text = _popup.Title; }
            if (descriptionTextComponent != null) { descriptionTextComponent.text = _popup.Description; }
            if (iconImageComponent != null) { iconImageComponent.sprite = _popup.Icon; }
            if (buttonComponent != null)
            {
                buttonComponent.onClick.RemoveAllListeners();
                if (_popup.Action != null) { buttonComponent.onClick.AddListener(_popup.Action); }
            }
        }

        public void SetActionAdditive(UnityAction _action)
        {
            buttonComponent.onClick.AddListener(_action);
        }

        public IPopup Create(Transform _parent)
        {
            return Instantiate(this, _parent);
        }

        public void Dispose()
        {
            Destroy(gameObject, 0.1f);
        }
    }
}