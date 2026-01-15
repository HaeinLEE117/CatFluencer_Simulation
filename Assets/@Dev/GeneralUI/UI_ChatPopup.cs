using System;
using UnityEngine;
using TMPro;

public class UI_ChatPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        NPCImage,
    }

    enum Buttons
    {
        NextButton,
    }

    enum Texts
    {
        ChatPopupTitleText,
        ComentText,
        NPCNameText
    }

    private Action _onClosed;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.NextButton).onClick.AddListener(OnClickNext);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        // Set all text fonts to current localization font asset
        var font = LocalizationManager.Instance != null ? LocalizationManager.Instance.CurrentFontAsset : null;
        if (font != null)
        {
            TrySetFont((int)Texts.ChatPopupTitleText, font);
            TrySetFont((int)Texts.ComentText, font);
            TrySetFont((int)Texts.NPCNameText, font);
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

    // Configure texts when showing the chat popup via UIManager
    public void Configure(string titleText, string commentText, string npcNameText)
    {
        var title = GetText((int)Texts.ChatPopupTitleText);
        var comment = GetText((int)Texts.ComentText);
        var npc = GetText((int)Texts.NPCNameText);
        if (title != null) title.text = titleText;
        if (comment != null) comment.text = commentText;
        if (npc != null) npc.text = npcNameText;
    }

    public void SetOnClosed(Action onClosed)
    {
        _onClosed = onClosed;
    }

    private void OnClickNext()
    {
        UIManager.Instance.ClosePopupUI();
        _onClosed?.Invoke();
    }
}
