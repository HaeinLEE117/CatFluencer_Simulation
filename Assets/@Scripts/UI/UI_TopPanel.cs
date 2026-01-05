using UnityEngine;

public class UI_TopPanel : UI_UGUI
{
    enum Buttons
    {
        StudioName,
        Calandar,
        Subscribers,
        Gold,
    }

    enum Texts
    {
        StudioNameText,
        CalandarText,
        SubscribersText,
        GoldText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }
}
