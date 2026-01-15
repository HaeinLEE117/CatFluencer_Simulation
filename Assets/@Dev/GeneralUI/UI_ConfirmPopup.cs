using System;
using UnityEngine;

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
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void OnClickCancel()
    {
        _onCancel?.Invoke();
        UIManager.Instance.ClosePopupUI();
    }

    private void OnClickConfirm()
    {
        _onConfirm?.Invoke();
        UIManager.Instance.ClosePopupUI();
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

    public void SetActions(Action onConfirm, Action onCancel = null, string confirmLabel = null, string cancelLabel = null)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;

        if (!string.IsNullOrEmpty(confirmLabel))
        {
            var txt = GetText((int)Texts.ConfirmButtonText);
            if (txt != null) txt.text = confirmLabel;
        }
        if (!string.IsNullOrEmpty(cancelLabel))
        {
            var txt = GetText((int)Texts.CancleButtonText);
            if (txt != null) txt.text = cancelLabel;
        }
    }
}
