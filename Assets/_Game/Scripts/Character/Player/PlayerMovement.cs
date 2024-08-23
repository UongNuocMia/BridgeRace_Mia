using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private DynamicJoystick dynamicJoystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 10;
    [SerializeField] private Player player;
    private bool isRunning;
    private NavMeshAgent nav;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        dynamicJoystick = GameManager.Ins.DynamicJoystick;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(dynamicJoystick.Direction.x, 0f, dynamicJoystick.Direction.y);
        Debug.Log("moveDirection____" + moveDirection);
        Vector3 destination = transform.position + moveDirection * speed;
        nav.SetDestination(destination);
        isRunning = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
