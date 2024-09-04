using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private DynamicJoystick dynamicJoystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Player player;
    private bool isRunning;
    private void Start()
    {
        dynamicJoystick = GameManager.Ins.DynamicJoystick;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!GameManager.IsState(GameState.GamePlay))
        {
            isRunning = false;
            return;
        }
        Vector3 moveDirection = new Vector3(dynamicJoystick.Direction.x, 0f, dynamicJoystick.Direction.y);
        if(!player.isCanMoveForward&& moveDirection.z > 0)
        {
            moveDirection.z = 0;
        }
        float rotateSpeed = 5f;
        transform.position += moveDirection.normalized * player.GetPlayerSpeed() * Time.deltaTime;
        isRunning = moveDirection != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
