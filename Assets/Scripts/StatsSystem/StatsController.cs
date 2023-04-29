using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Services.Updater;
using UnityEngine;

namespace StatsSystem
{
    public class StatsController: IDisposable, IStatValueGiver, IStatGiver
    {
        private readonly List<Stat> _currentStats;
        private readonly List<StatModificator> _activeModificators;

        
        public StatsController(List<Stat> currentStats)
        {
            _currentStats = currentStats;
            _activeModificators = new List<StatModificator>();
            
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public Stat GetStat(StatType statType) => _currentStats.Find(stat => stat.Type == statType);
        public float GetValue(StatType statType) => _currentStats.Find(stat => stat.Type == statType);

        public void ProcessModificator(StatModificator modificator)
        {
            if (modificator.Stat == 0)
                modificator.Stat.SetValue(Mathf.Epsilon);
            
            var statToChange = _currentStats.Find(stat => stat.Type == modificator.Stat.Type);
            
            if (statToChange == null)
                return;

            var newValue = modificator.Type == StatModificatorType.Additive
                ? statToChange + modificator.Stat
                : statToChange * modificator.Stat;
            
            statToChange.SetValue(newValue);

            if (modificator.Duration < 0)
                return;

            if (_activeModificators.Contains(modificator))
                _activeModificators.Remove(modificator);
            else
            {
                newValue = modificator.Type == StatModificatorType.Additive 
                    ? -modificator.Stat 
                    : 1 / modificator.Stat;
                
                var addedStat = new Stat(modificator.Stat.Type, newValue);
                var tempModificator = new StatModificator(addedStat, modificator.Type, modificator.Duration, Time.time);
                _activeModificators.Add(tempModificator);
            }
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= OnUpdate;
        }

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0)
                return;

            var expiredModificators =
                _activeModificators.Where(modificator => modificator.StartTime + modificator.Duration >= Time.time);

            foreach (var modificator in expiredModificators)
            {
                ProcessModificator(modificator);
            }
        }
    }
}