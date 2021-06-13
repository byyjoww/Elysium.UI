using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_TextArrayUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private ValueSO[] uiText;

    [SerializeField] string formatString;

    private void OnEnable()
    {
        if (uiText == null || uiText.Length < 1 || textComponent == null) { return; };
        foreach (var t in uiText)
        {
            t.OnValueChanged += Refresh;
        }
        Refresh();
    }

    private void OnDisable()
    {
        if (uiText == null || uiText.Length < 1 || textComponent == null) { return; };
        foreach (var t in uiText)
        {
            t.OnValueChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        textComponent.text = string.Format(formatString, uiText.Select(x => x.ValueAsString).ToArray());
    }

    private void OnValidate()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
    }
}
