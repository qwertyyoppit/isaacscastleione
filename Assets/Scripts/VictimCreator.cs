using UnityEngine;
using UnityEngine.InputSystem;

public class VictimCreator : MonoBehaviour
{
    private PlayerInput input;

    private int victimCount;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();

        input.actions["interact"].started += _ =>
        {
            victimCount += 1;
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = $"Victim {victimCount}";
            obj.AddComponent<Rigidbody>();
        };
    }
}
