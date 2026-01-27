using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IFramesManager : MonoBehaviour
{
    private Dictionary<Entity, float> entityIFrames = new();

    private void Update()
    {
        List<Entity> toRemove = new();
        foreach (var ent in entityIFrames.Keys.ToArray())
        {
            entityIFrames[ent] -= Time.deltaTime;
            if (entityIFrames[ent] <= 0) toRemove.Add(ent);
        }

        foreach (var ent in toRemove) entityIFrames.Remove(ent);
    }

    public void InflictEntityIFrames(Entity entity, float duration = 1.0f)
    {
        if (entityIFrames.ContainsKey(entity)) entityIFrames[entity] = duration;
        else entityIFrames.Add(entity, duration);
    }

    public bool CheckImmunity(Entity entity)
    {
        return entityIFrames.ContainsKey(entity);
    }
}
