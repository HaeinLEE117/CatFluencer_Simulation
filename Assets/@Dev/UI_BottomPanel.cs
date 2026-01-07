using System;
using UnityEngine;
using static Define;

public class UI_BottomPanel : UI_UGUI
{
    enum GameObjects
    {
    }

    enum Buttons
    {
    }

    enum Texts
    {

    }

    protected override void Awake()
    {
        base.Awake();


        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }


}
