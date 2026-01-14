using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        LoadingScene,
        DevScene,
    }

    public enum EEventType
    {
        None,
        GoldChanged,
        LanguageChanged,
        UI_MenuButtonClicked,
        // Left Panel secondary actions
        UI_NewVideoClicked,
        UI_LiveStreamClicked,
        UI_HireClicked,
        UI_FireClicked,
        UI_GoalsClicked,
        UI_PreVideosClicked,
        // Selection events
        UI_LocationSelected,
        UI_CastSelected,
        // Popup stack changed
        UI_PopupStackChanged,
        UI_ContentSelected,
        //GameData Changed
        SubscriberChanged,
        ChannelNameChanged,
        VideoBalancePointsChanged,
        SecondsPerWeekChanged,
        UpgradeCountChanged,
        InitHiredEmployeesChanged,
        EmployEducationDone,
        //날짜 업데이트
        WeekAdvanced,
        MonthAdvanced,
        YearAdvanced,
        UI_PopupOpened,
        UI_PopupClosed,
        UI_LeftPanelClosed,
        UI_LeftPanelOpened,
        HireableEmployeesChanged,
    }

    public enum ESound
    {
        Bgm,
        Effect,

        MaxCount
    }

    public enum ELanguage
    {
        KOR,
        ENG,
    }

	public enum EAnimation
	{
		b_wait,
		b_walk,
		f_wait,
		f_walk
	}

	public enum ECatState
	{
		Idle,
		Move,
		Work
	}
}
