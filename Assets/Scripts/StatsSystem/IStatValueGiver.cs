using Core.Enums;

namespace StatsSystem
{
    public interface IStatValueGiver
    {
        float GetValue(StatType statType);
    }
}