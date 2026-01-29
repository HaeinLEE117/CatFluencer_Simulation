using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

[System.Serializable]
public class LocationGoodComboData
{
    public int locationId;
    public int[] goodContentsIds;
}

[CreateAssetMenu(fileName = "VideoLocationConfig", menuName = "Config/VideoLocationConfig")]
public class VideoLocationsConfig : ScriptableObject
{
    [Header("장소 기준으로 조합 좋은 컨텐츠 나열")]
    [SerializeField]
    private List<LocationGoodComboData> _goodComboContents = new List<LocationGoodComboData>
    {

    };

    public List<LocationGoodComboData> GoodComboContents => _goodComboContents;
}
