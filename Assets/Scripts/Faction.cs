using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Faction", menuName = "Scriptable Objects/Faction")]
public class Faction : ScriptableObject
{
    public List<Faction> Exceptionlist = new();

    public bool HostileByDefault;

    public bool Attacks(Faction faction)
    {
        return HostileByDefault ^ Exceptionlist.Contains(faction);
    }

    // This has limitations. Example
    // A zombie doesn't attack other zombies, doesn't target critters (but attacks them unintentionally)
    // attacks & targets NPCs, and attacks player but might prioritize player over others
}
