using NUnit.Framework.Internal.Builders;

public class constants
{
    public const int WEEKSPERMONTH = 4;
    public const int MONTHSPERYEAR = 12;

    // 게임 내 1주일이 실제 시간으로 몇 초인지 정의,
    // TODO: config에서 테스트 후  가져오기
    public const float SecondsPerWeek = 10f;

    public const int WEEKSFORJOBPOSTINGDONE = 2;

    // 레벨 비례 데이터시트에서 가져올 직원수
    public const int BASEEMPLOYEESFORLEVEL = 3;

    // 레벨 비례 데이터시트에서 가져올 콘텐츠수
    public const int BASECONTENTFORLEVEL = 5;

    // 레벨 비례 데이터시트에서 가져올 촬영장소 수
    public const int BASELOCATIONSFORLEVEL = 2;

    // 레벨 비례 비디오 밸런스 증가 포인트
    public const int BASEVIDEOBALANCEPOINTFORLEVEL = 3;

    // 레벨 비례 캐스팅 가능한 출연진 수
    public const int BASECASTFORLEVEL = 2;

    // 새 비디오 촬영시 점수 계산을 위한 최소/최대 값
    public const int MIN_CAST_STAT1_SCORE = 1;
    public const int MIN_CAST_STAT2_SCORE = 1;
    public const int MIN_CAST_STAT3_SCORE = 0;

    public const int MAX_CAST_STAT1_SCORE = 10;
    public const int MAX_CAST_STAT2_SCORE = 10;
    public const int MAX_CAST_STAT3_SCORE = 10;

    public const float COMBO_BONUS_MULTIPLIER = 2f;

}