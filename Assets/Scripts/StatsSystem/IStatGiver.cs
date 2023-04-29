using Core.Enums;

namespace StatsSystem
{
    public interface IStatGiver
    {
        Stat GetStat(StatType statType);
    }
}