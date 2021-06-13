using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpriteUpdater : MonoBehaviour
{
    [SerializeField] private Image spriteImageComponent;
    [SerializeField] private SpriteValueSO uiSprite;

    private void OnEnable()
    {
        if (uiSprite == null || spriteImageComponent == null) { return; };

        uiSprite.OnValueChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (uiSprite == null || spriteImageComponent == null) { return; };

        uiSprite.OnValueChanged -= Refresh;
    }

    private void Refresh()
    {
        spriteImageComponent.sprite = uiSprite.Value;
    }

    private void OnValidate()
    {
        if (spriteImageComponent == null) spriteImageComponent = GetComponent<Image>();
    }
}
