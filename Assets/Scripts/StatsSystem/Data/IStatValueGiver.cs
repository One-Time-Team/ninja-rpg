using StatsSystem.Enums;

namespace StatsSystem.Data
{
    public interface IStatValueGiver
    {
        float GetValue(StatType statType);
    }
}