using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float distance;
    [SerializeField] private Vector3 offset;

    [ExecuteAlways]
    private void Update()
    {
        if (!target) return;
        transform.position += target.position - transform.position + transform.forward * -distance + offset;
    }
}
