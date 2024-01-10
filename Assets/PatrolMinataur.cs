using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolMinataur : StateMachineBehaviour
{
    float timer;
    //List<Transform> wayPoints = new List<Transform>();
    public float radius = 10f;
    NavMeshAgent agent;
    Transform player;
    float chaseRange = 30f;
    private Vector3 randomPoint;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        //GameObject wp = GameObject.FindGameObjectWithTag("WayPoints");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 4.5f;

        randomPoint = animator.transform.position + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
        randomPoint = hit.position;
        agent.SetDestination(randomPoint);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            randomPoint = animator.transform.position + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
            randomPoint = hit.position;

            // Set the agent's destination to the new random point
            agent.SetDestination(randomPoint);
        }
        timer += Time.deltaTime;
        if (timer > 8)
        {
            animator.SetBool("isPatrolling", false);

        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {

            animator.SetBool("isChasing", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
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
