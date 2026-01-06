using UnityEngine;

public class UI_GoalsPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        GoalsText,
    }

    enum Buttons
    {
    }

    enum Texts
    {
        CurrentInfoText,
        GoalsInfoText,
        NextGradeDescriptionText,

        Goal1_TitleText,
        Goal1_CurrentText,
        Goal1_GoalText,

        Goal2_TitleText,
        Goal2_CurrentText,
        Goal2_GoalText,
        Goal3_TitleText,
        Goal3_CurrentText,
        Goal3_GoalText,
        Goal4_TitleText,
        Goal4_CurrentText,
        Goal4_GoalText,
        Goal5_TitleText,
        Goal5_CurrentText,
        Goal5_GoalText,
    }

protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }

}
