using System.Collections;
using UnityEngine;

public class UI_JobPostingPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        JobPostingImage,
    }

    enum Buttons
    {
        PostingButton,
    }

    enum Texts
    {
        JobPostingText,
        JobPostingDescribtionText,
        PostingButtonText,
        CostText,

    }

    protected override void Awake()
    {
            base.Awake();

            BindObjects(typeof(GameObjects));
            BindButtons(typeof(Buttons));
            BindTexts(typeof(Texts));

        GetButton((int)Buttons.PostingButton).onClick.AddListener(EmployeeManager.Instance.StartUIJobPosting);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }

}
