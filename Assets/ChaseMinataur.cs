using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseMinataur : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    public bool isAttack = false;
    public float coolDown = 1.5f;
    private bool isCooldownActive = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 9f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 5)
        {
            animator.SetBool("isChasing", false);
        }
        if (distance < 3)
        {
            if (!isAttack & !isCooldownActive)
            {
                animator.SetBool("isAttack", true);
                isAttack = true;
                animator.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(StartCooldown(animator));
            }
        } else
        {
            isAttack = false;
        }

    }

    private IEnumerator StartCooldown(Animator animator)
    {
        isCooldownActive = true;

        yield return new WaitForSeconds(coolDown);

        isCooldownActive = false;
        isAttack = false;
        animator.SetBool("isAttack", false);
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
