using TMPro;
using Unity.VisualScripting;
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

        EventManager.Instance.AddEvent(Define.EEventType.GoldChanged, RefreshUI);
        EventManager.Instance.AddEvent(Define.EEventType.WeekAdvanced, RefreshUI);
        EventManager.Instance.AddEvent(Define.EEventType.MonthAdvanced, RefreshUI);
        EventManager.Instance.AddEvent(Define.EEventType.YearAdvanced, RefreshUI);
    }


    public override void RefreshUI()
    {
        base.RefreshUI();
        GetText((int)Texts.StudioNameText).SetTextwithFont(GameManager.Instance.ChannelName);
        string date = GameManager.Instance.NowYear.ToString() + LocalizationManager.Instance.GetLocalizedText("YEAR")
            + GameManager.Instance.NowMonth.ToString() + LocalizationManager.Instance.GetLocalizedText("MONTH")
            + GameManager.Instance.NowWeek.ToString() + LocalizationManager.Instance.GetLocalizedText("WEEK");

        GetText((int)Texts.CalandarText).SetTextwithFont(date);
        GetText((int)Texts.SubscribersText).SetTextwithFont(GameManager.Instance.Subscribers.ToString());
        GetText((int)Texts.GoldText).SetTextwithFont( GameManager.Instance.Gold.ToString() + "G");

    }

}
