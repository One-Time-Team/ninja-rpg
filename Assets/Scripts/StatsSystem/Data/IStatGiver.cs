using StatsSystem.Enums;

namespace StatsSystem.Data
{
    public interface IStatGiver
    {
        Stat GetStat(StatType statType);
    }
}