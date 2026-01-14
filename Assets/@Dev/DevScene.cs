using UnityEngine;

public class DevScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        SceneType = Define.EScene.DevScene;

        ResourceManager.Instance.LoadAll();

        DataManager.Instance.LoadData();

        Player cat = ObjectManager.Instance.SpawnPlayer("Cat");
        cat.State = Cat.ECatState.Idle;
        cat.transform.position = Vector3.zero;

        //시간 관련
        GameManager.Instance.StartTime();
        //EventManager.Instance.AddEvent(Define.EEventType.UI_PopupStackChanged, GameManager.Instance.);
    }
}
