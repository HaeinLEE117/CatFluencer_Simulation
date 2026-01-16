using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

[System.Serializable]
public class ContentsGoodComboData
{
    public int ContentsId;
    public int[] goodLocationsIds;
}

[CreateAssetMenu(fileName = "VideoContentsConfig", menuName = "Config/VideoContentsConfig")]
public class VideoContentsConfig : ScriptableObject
{
    [Header("컨텐츠 기준으로 조합 좋은 장소 나열")]
    [SerializeField]
    private List<LocationGoodComboData> _goodComboLocations = new List<LocationGoodComboData>
    {

    };


}
