using UnityEngine;

public class Entity : MonoBehaviour
{
    public Faction Faction;

    public IFramesManager IFrames;

    private void Awake()
    {
        if (!TryGetComponent<IFramesManager>(out var comp))
        {
            comp = gameObject.AddComponent<IFramesManager>();
        }
        IFrames = comp;
    }
}
