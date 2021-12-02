using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimatorHandler : MonoBehaviour
{

    [SerializeField] Animator animator;
    GameWorldSpeedController gameWorldSpeedController;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameWorldSpeedController = Object.FindObjectOfType<GameWorldSpeedController>();
        gameWorldSpeedController.AddAnimatorToWorldAnimatorList(animator);
    }

    private void OnDestroy()
    {
        gameWorldSpeedController.RemoveAnimatorToWorldAnimatorList(animator);

    }
}
