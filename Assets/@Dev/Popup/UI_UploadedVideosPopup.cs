using UnityEngine;

public class UI_UploadedVideosPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        UploadedVideosText,
    }

    enum Buttons
    {
    }

    enum Texts
    {
        Video1_TitleText,
        Video1_SalesText,
        Video2_TitleText,
        Video2_SalesText,
        Video3_TitleText,
        Video3_SalesText,
        Video4_TitleText,
        Video4_SalesText,
        Video5_TitleText,
        Video5_SalesText,

        VideoTitleText,
        VideoSalesText
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
