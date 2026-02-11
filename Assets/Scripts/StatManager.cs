using UnityEngine;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{
    private Dictionary<IStat, List<StatModifier>> statTypeModifier;

    private void GetStat(string statType)
    {

    }

    public void ApplyStat(StatModifier statModifier)
    {
        statModifier.destroyed += delegate { print("this would be where i'd remove it from the list, if i had any"); };
    }
}