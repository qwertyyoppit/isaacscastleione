using UnityEngine;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{
    private Dictionary<IStat, List<StatModifier>> statTypeModifier = new();

    private List<IStat> cachedStats = new();

    public List<IStat> BaseStats = new();

    public void GetStat<T>() where T : IStat
    {
        
    }

    public void ApplyStat(StatModifier statModifier)
    {
        statModifier.Destroyed += delegate { print("this would be where i'd remove it from the list, if i had any"); };
    }
}