using UnityEngine;

public class UI_FirUI_VideoDetailPopupePopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        CastPhoto,
    }

    enum Buttons
    {
        PreButton,
        NextButton,

    }

    enum Texts
    {
        VideoDetailText,

        VideoNameText,
        VideoSalesText,

        Stat1Text,
        Stat2Text, 
        Stat3Text,

        Stat1PointText,
        Stat2PointText,
        Stat3PointText,

        LocationText,
        ContentText,
        CastNameText,
        DescriptionText,
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
}
