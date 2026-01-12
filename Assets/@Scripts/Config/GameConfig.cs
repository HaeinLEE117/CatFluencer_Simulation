using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    // TODO: 게임 전반에 걸친 설정값들을 여기에 추가
    // 예: 초기 직원 정보

    [Header("Game Settings")]
    
    [Min(500)]
    [SerializeField]
    private int initialGold = 1000;

    [Range(1, 20)]
    [SerializeField]
    private int initialLevel = 1;

    public int InitialGold => initialGold;
    public int InitialLevel => initialLevel;
}
