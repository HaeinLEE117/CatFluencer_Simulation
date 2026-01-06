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
        UI_EducationClicked,
        UI_HireClicked,
        UI_FireClicked,
        UI_GoalsClicked,
        UI_PreVideosClicked,
        // Selection events
        UI_LocationSelected,
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
