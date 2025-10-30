namespace backend.Domain.Enums;

public enum Season
{
    None = 0,
    Winter = 1,
    Spring = 2,
    Summer = 3,
    Fall = 4
}

public static class SeasonExtensions
{
    public static int StartMonth(this Season season) => season switch
    {
        Season.Winter => 1,
        Season.Spring => 4,
        Season.Summer => 7,
        Season.Fall => 10,
        _ => throw new ArgumentOutOfRangeException(nameof(season), $"Invalid season: {season}")
    };

    public static int EndMonth(this Season season) => season switch
    {
        Season.Winter => 3,
        Season.Spring => 6,
        Season.Summer => 9,
        Season.Fall => 12,
        _ => throw new ArgumentOutOfRangeException(nameof(season), $"Invalid season: {season}")
    };
}