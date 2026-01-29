using System;
using UnityEngine;
using TMPro;

public class UI_ConfirmPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        CancleButton,
        ConfirmButton,
    }

    enum Texts
    {
        ConfirmPopupText,
        DescribtionText,

        CancleButtonText,
        ConfirmButtonText,
    }

    private Action _onConfirm;
    private Action _onCancel;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.CancleButton).onClick.AddListener(OnClickCancel);
        GetButton((int)Buttons.ConfirmButton).onClick.AddListener(OnClickConfirm);

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        // Set all text fonts to current localization font asset
        var font = LocalizationManager.Instance != null ? LocalizationManager.Instance.CurrentFontAsset : null;
        if (font != null)
        {
            TrySetFont((int)Texts.ConfirmPopupText, font);
            TrySetFont((int)Texts.DescribtionText, font);
            TrySetFont((int)Texts.CancleButtonText, font);
            TrySetFont((int)Texts.ConfirmButtonText, font);
        }
    }

    private void TrySetFont(int textIndex, TMP_FontAsset font)
    {
        var t = GetText(textIndex);
        if (t != null)
        {
            t.font = font;
        }
    }

    private void OnClickCancel()
    {
        UIManager.Instance.ClosePopupUI();
        _onCancel?.Invoke();
    }

    private void OnClickConfirm()
    {
        UIManager.Instance.ClosePopupUI();
        _onConfirm?.Invoke();
    }

    public void SetTitle(string title)
    {
        var t = GetText((int)Texts.ConfirmPopupText);
        if (t != null) t.text = title;
    }

    public void SetMessage(string message)
    {
        var t = GetText((int)Texts.DescribtionText);
        if (t != null) t.text = message;
    }

    public void SetActions(Action onConfirm, Action onCancel = null, string confirmLabel = "CONFIRM", string cancelLabel = "CLOSE")
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel != null ? onCancel : UIManager.Instance.ClosePopupUI;

        GetText((int)Texts.ConfirmButtonText).SetLocalizedText(confirmLabel);
        GetText((int)Texts.CancleButtonText).SetLocalizedText(cancelLabel);
        RefreshUI();
    }
}
