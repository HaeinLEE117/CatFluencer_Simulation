using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }

    public override void RefreshUI()
    {
        base.RefreshUI();
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
}
