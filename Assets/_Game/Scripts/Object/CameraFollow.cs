using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private static Transform target;
    [SerializeField] private Vector3 offset; // x = -5; y = 10
    [SerializeField] private float speed = 20;

    public static void FindPlayer(Transform playerTransform)
    {
        target = playerTransform;
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
