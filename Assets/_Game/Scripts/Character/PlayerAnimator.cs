using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Animator animator;

    private void Awake()
    {
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, playerMovement.IsRunning());
    }

}
