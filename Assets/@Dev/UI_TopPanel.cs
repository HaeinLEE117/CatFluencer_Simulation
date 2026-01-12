using UnityEngine;

public class UI_TopPanel : UI_UGUI
{
    enum Buttons
    {
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

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        //TODO: Update top panel texts based on game data
    }
}
