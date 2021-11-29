using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdventurerAnimationController : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationHandling();
    }

    void AnimationHandling()
    {
        if (navMeshAgent.velocity.Equals(Vector3.zero))
            animator.SetBool("Idle", true);
        else
        {
            if (Mathf.Abs(navMeshAgent.velocity.y) > Mathf.Abs(navMeshAgent.velocity.x))
            {
                //Vertical Movement
                animator.SetFloat("Horizontal Movement", 0);
                animator.SetFloat("Vertical Movement", navMeshAgent.velocity.y);
            }
            else
            {
                //Horzontal Movement
                animator.SetFloat("Vertical Movement", 0);
                animator.SetFloat("Horizontal Movement", navMeshAgent.velocity.x);
            }
            animator.SetBool("Idle", false);
        }
    }
}
