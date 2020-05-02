using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolController : StateMachineBehaviour
{

    private GuardController guardCtrl;
    private GameObject guard;
    private NavMeshAgent agent;
    private List<Transform> patrolPoints = new List<Transform>();

    private Transform targetGuardPoint;
    private int targetTransformelement;
    private float thresholdArriveDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Set up vars

        if (!guard)
        {
            guard = animator.gameObject;
        }

        if (!guardCtrl)
        {
            guardCtrl = guard.GetComponent<GuardController>();
        }

        if (patrolPoints.Count < 1)
        {
            patrolPoints = guardCtrl.guardPath;
        }

        if (!agent)
        {
            agent = guard.GetComponent<NavMeshAgent>();
        }

        thresholdArriveDistance = guardCtrl.arriveThreshold;

        //go to the nearest guard point

        float shortest = Mathf.Infinity;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (Vector3.Distance(patrolPoints[i].position, guard.transform.position) < shortest)
            {
                targetGuardPoint = patrolPoints[i];
                shortest = Vector3.Distance(patrolPoints[i].position, guard.transform.position);
                targetTransformelement = i;
            }
        }

        //Set destination
        agent.SetDestination(targetGuardPoint.position);

    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //When we arrive at a position
        if (agent.remainingDistance <= thresholdArriveDistance)
        {
            //set next target
            targetTransformelement++;
            if (targetTransformelement > patrolPoints.Count)
            {
                targetTransformelement = 0;
            }

            targetGuardPoint = patrolPoints[targetTransformelement];

            //Go to new target
            agent.SetDestination(targetGuardPoint.position);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
