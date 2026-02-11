using System;
using UnityEditor;
using UnityEngine;

class StatModifier
{
    public StatModifierType ModifierType;
    public Action destroyed;

    public StatModifier(StatModifierType modifierType, IStat stat)
    {
        ModifierType = modifierType;
    }
    ~StatModifier()
    {
        destroyed?.Invoke();
    }
}