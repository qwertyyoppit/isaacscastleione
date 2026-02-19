using System;
using UnityEditor;
using UnityEngine;

public class StatModifier
{
    public StatModifierType ModifierType;
    public Action Destroyed;

    public StatModifier(StatModifierType modifierType, IStat stat)
    {
        ModifierType = modifierType;
    }
    ~StatModifier()
    {
        Destroyed?.Invoke();
    }
}