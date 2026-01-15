using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    Transform _root;
    Transform Root
    {
        get
        {
            return Utils.GetRootTransform(ref _root, "@UI_Root");
        }
    }

    #region Scene UI
    private UI_Base _sceneUI;
    public UI_Base SceneUI
    {
        get
        {
			foreach (UI_Base ui in FindObjectsByType<UI_Base>(FindObjectsSortMode.None))
			{
				if (ui is IUI_Scene)
				{
					_sceneUI = ui;
					break;
				}
			}
			
			return _sceneUI;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Base, IUI_Scene
	{
        if (_sceneUI != null)
            return _sceneUI as T;

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI = FindFirstObjectByType<T>();
        if (sceneUI == null)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            sceneUI = Utils.GetOrAddComponent<T>(go);
        }

        sceneUI.transform.SetParent(Root);
        _sceneUI = sceneUI;

        return sceneUI;
    }
    #endregion

    #region Popup UI
    Transform _popupRoot;
    Transform PopupRoot
    {
        get
        {
            return Utils.GetRootTransform(ref _popupRoot, "@PopupRoot", Root);
        }
    }

    // Single active popup model
    private UI_Base _activePopup;
    private bool _wasPopupOpen;
    private readonly Dictionary<string, UI_Base> _popups = new Dictionary<string, UI_Base>();
    private const int FixedPopupSortingOrder = 300;
    private readonly Stack<UI_Base> _popupStack = new Stack<UI_Base>();

    public T ShowPopupUI<T>(string name = null) where T : UI_Base, IUI_Popup
	{
        return ShowPopupUI<T>(name, closeCurrent: false);
    }

    // Overload with option to close existing or stack current
    public T ShowPopupUI<T>(string name, bool closeCurrent) where T : UI_Base, IUI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (closeCurrent)
            CloseAllPopupUI();
        else if (_activePopup != null)
        {
            _popupStack.Push(_activePopup);
            SetPopupActive(_activePopup, false);
        }

        if (_popups.TryGetValue(name, out UI_Base popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = Utils.GetOrAddComponent<T>(go);
            _popups[name] = popup;
        }

        _activePopup = popup;
        _wasPopupOpen = true;

        popup.transform.SetParent(PopupRoot);
        SetPopupActive(popup, true);

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StopTime();
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupOpened);
        return popup as T;
    }

    public UI_Base ShowPopupUI(string name)
    {
        return ShowPopupUI(name, closeCurrent: true);
    }

    // Overload with option to close existing or stack current
    public UI_Base ShowPopupUI(string name, bool closeCurrent)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        if (closeCurrent)
            CloseAllPopupUI();
        else if (_activePopup != null)
        {
            _popupStack.Push(_activePopup);
            SetPopupActive(_activePopup, false);
        }

        if (_popups.TryGetValue(name, out UI_Base popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = go.GetComponent<UI_Base>();
            if (popup == null)
                popup = Utils.GetOrAddComponent<UI_GenericPopup>(go);
            _popups[name] = popup;
        }

        _activePopup = popup;
        _wasPopupOpen = true;

        popup.transform.SetParent(PopupRoot);
        SetPopupActive(popup, true);

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StopTime();
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupOpened);
        return popup;
    }

    public T GetLastPopupUI<T>() where T : UI_Base
	{
        return _activePopup as T;
    }

    public void ClosePopupUI()
    {
        // If no active popup, do nothing
        if (_activePopup == null)
            return;

        // Deactivate current active popup
        UI_Base popup = _activePopup;
        _activePopup = null;

        SetPopupActive(popup, false);

        // If there is a popup in stack, restore it; else resume time
        if (_popupStack.Count > 0)
        {
            _activePopup = _popupStack.Pop();
            SetPopupActive(_activePopup, true);
        }
        else
        {
            if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
            {
                GameManager.Instance.StartTime();
            }
            _wasPopupOpen = false;
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupClosed);
    }

    public void CloseAllPopupUI()
    {
        // Close active and clear stack entirely
        if (_activePopup != null)
        {
            SetPopupActive(_activePopup, false);
            _activePopup = null;
        }
        while (_popupStack.Count > 0)
        {
            var p = _popupStack.Pop();
            SetPopupActive(p, false);
        }

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StartTime();
        }

        _wasPopupOpen = false;
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupClosed);
    }
    #endregion

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate(name);

        return go.GetOrAddComponent<T>();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _popups.Clear();

        Root.DestroyChildren();

        _sceneUI = null;
    }

    public void NotifyLocationSelected(string location)
    {
        // 공통 검증/전처리
        if (string.IsNullOrEmpty(location))
            return;

        GameManager.Instance.UpdateRecordingLocation(location);
    }

    #region 대화창 팝업

    internal void ShowChatPopup(string titleText, string commentText, string npcNameText, Action onClosed = null)
    {
        // Use stacking behavior by default (do not force close existing)
        var chat = ShowPopupUI<UI_ChatPopup>(nameof(UI_ChatPopup), closeCurrent: false);
        if (chat != null)
        {
            chat.Configure(titleText, commentText, npcNameText);
            chat.SetOnClosed(onClosed);
        }
    }
    #endregion

    public void ShowConfirmPopup(string titleText,string message, Action onConfirm, Action onCancel = null)
    {
        var popup = ShowPopupUI<UI_ConfirmPopup>(null, closeCurrent: false);
        if (popup == null)
            return;
        popup.SetTitle(titleText);
        popup.SetMessage(message);
        popup.SetActions(onConfirm, onCancel, confirmLabel: null, cancelLabel: null);
    }

    private void SetPopupActive(UI_Base popup, bool active)
    {
        if (popup == null) return;
        if (popup is UI_Toolkit toolkitUI)
        {
            var doc = toolkitUI.GetComponent<UIDocument>();
            doc.sortingOrder = FixedPopupSortingOrder;
            doc.rootVisualElement.visible = active;
        }
        else
        {
            var canvas = popup.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = FixedPopupSortingOrder;
            popup.gameObject.SetActive(active);
        }
        if (active)
        {
            popup.transform.SetParent(PopupRoot);
        }
    }
}
