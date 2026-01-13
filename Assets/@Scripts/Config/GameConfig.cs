using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    // TODO: 게임 전반에 걸친 설정값들을 여기에 추가
    // 예: 초기 직원 정보

    [Header("Game Settings")]
    private int initialGold = 500;
    private int initalSubscriber;
    private int initialYear = 1;
    private int initialMonth = 1;
    private int initialWeek = 1;
    private int initialVideoBalancePoints = 4;
    private string initialChannelName = "Myao Studio";

    [SerializeField]
    private float SecondsPerWeek = 10f;

    [Header("Staff Settings")]
    private int initHiredEmployee = 1001;    // 초기 고용 직원 ID 노리스


    public int InitialGold => initialGold;
    public int InitialSubscriber => initalSubscriber;
    public int InitailYear => initialYear;
    public int InitailMonth => initialMonth;
    public int InitailWeek => initialWeek;
    public int InitialVideoBalancePoints => initialVideoBalancePoints;
    public int InitialHiredEmployee => initHiredEmployee;
    public string InitialChannelName => initialChannelName;

    public float GetSecondsPerWeek()
    {
        return SecondsPerWeek;
    }

    public int InitHiredEmployee => initHiredEmployee;
}
