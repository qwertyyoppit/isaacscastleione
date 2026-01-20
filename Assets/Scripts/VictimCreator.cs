using UnityEngine;
using UnityEngine.InputSystem;

public class VictimCreator : MonoBehaviour
{
    private PlayerInput input;

    private int victimCount;

    [SerializeField] private GameObject victimPrefab;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();

        input.actions["interact"].started += _ =>
        {
            victimCount += 1;
            var obj = victimPrefab ? Instantiate(victimPrefab) : GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = $"Victim {victimCount}";
            if (!obj.GetComponent<Rigidbody>())
                obj.AddComponent<Rigidbody>();
        };
    }
}
