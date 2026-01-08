using UnityEngine;

public class UI_ReleaseReactionPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        Photo1,
        Photo2,
        Photo3,
        Photo4,
        Photo5,
    }

    enum Buttons
    {
    }

    enum Texts
    {
        ReleaseReactionText,
        VideoNameText,

        ComentText1,
        ComentText2,
        ComentText3,
        ComentText4,
        ComentText5,

        PointText1,
        PointText2,
        PointText3,
        PointText4,
        PointText5,
        
        TotalPointText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
    }
}
