using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset; // x = -5; y = 10
    [SerializeField] private float speed = 20;

    public void FindPlayer(Transform playerTransform)
    {
        target = playerTransform;
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
