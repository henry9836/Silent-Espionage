using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class chasePlayer : StateMachineBehaviour
{

    private GuardController guardCtrl;
    private GameObject guard;
    private NavMeshAgent agent;
    private List<Transform> patrolPoints = new List<Transform>();
    private GameObject player;
    private GameObject gun;
    private Vector3 lastSeenPlayerPos;

    private float timeOutTime;
    private float timeOutTimer;
    private float gunTimer;
    private float gunTime;
    private float gunRange;
    private Vector3 gunScaleTarget;

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

        timeOutTimer = 0.0f;

        if (timeOutTime == 0.0f)
        {
            timeOutTime = guardCtrl.timeToLossPlayer;
        }

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (gunRange == 0.0f)
        {
            gunRange = guardCtrl.shootDistanceThreshold;
        }

        if (gunTime == 0.0f)
        {
            gunTime = guardCtrl.shootPullOutTime;
        }

        if (gunScaleTarget == Vector3.zero)
        {
            gunScaleTarget = guardCtrl.gunScaleTarget;
        }

        if (!gun)
        {
            gun = guardCtrl.gun;
        }

        //Stop agent
        agent.ResetPath();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If we can see the player move towards him and get within shoot range
        if (animator.GetBool("CanSeePlayer"))
        {

            if (timeOutTimer > timeOutTime)
            {
                timeOutTimer = timeOutTime;
            }

            timeOutTimer -= Time.deltaTime;

            //Only chase if we are on a nav mesh
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(player.transform.position);
            }

            lastSeenPlayerPos = player.transform.position;

            if (player.GetComponent<NavMeshAgent>().velocity.magnitude > 0)
            {
                lastSeenPlayerPos += player.transform.forward * player.GetComponent<NavMeshAgent>().velocity.magnitude;
            }

            timeOutTimer = 0.0f;
        }
        //Lost sight of player
        else
        {

            if (timeOutTimer < 0.0f)
            {
                timeOutTimer = 0.0f;
            }

            timeOutTimer += Time.deltaTime;

            //Only chase if we are on a nav mesh
            if (agent.isOnNavMesh)
            {
                //Go to last seen Pos
                agent.SetDestination(lastSeenPlayerPos);
            }

            //When we go to last position look at other positions around
            if (agent.velocity.magnitude < 0.1f)
            {
                lastSeenPlayerPos += new Vector3(Random.Range(guardCtrl.wanderRange.x, guardCtrl.wanderRange.y), 0.0f, Random.Range(guardCtrl.wanderRange.x, guardCtrl.wanderRange.y));
            }

        }

        //Timeout
        if (timeOutTimer >= timeOutTime)
        {
            animator.SetTrigger("LostPlayer");
        }

        //Are we close enough to the player to shoot and have ammo if so increase gun timer 
        if (Vector3.Distance(guard.transform.position, player.transform.position) <= (gunRange - 1.5f) && guardCtrl.magCurrentSize > 0 && animator.GetBool("CanSeePlayer"))
        {
            if (gunTimer < 0.0f)
            {
                gunTimer = 0.0f;
            }

            //Only chase if we are on a nav mesh
            if (agent.isOnNavMesh)
            {
                //stop chasing
                agent.ResetPath();
            }

            //Get the gun out
            gunTimer += Time.deltaTime;
        }
        //Are we close enough to the player to shoot and have no ammo if so decrease gun timer 
        else if (Vector3.Distance(guard.transform.position, player.transform.position) <= (gunRange - 1.5f) && guardCtrl.magCurrentSize <= 0 && animator.GetBool("CanSeePlayer"))
        {
            //stop chasing
            agent.ResetPath();
            //Put the gun away to reload
            gunTimer -= Time.deltaTime;

            //If the gun is away reload
            if (gun.transform.localScale == Vector3.zero)
            {
                if (!guardCtrl.reloading)
                {
                    guardCtrl.Reload();
                }
            }

            //clamp
            if (gunTimer > gunTime)
            {
                gunTimer = gunTime;
            }
        }

        //Cannot shoot player as he/she is too far away
        else
        {
            if (gunTimer > gunTime)
            {
                gunTimer = gunTime;
            }

            gunTimer -= Time.deltaTime;
        }

        //Lerp Gun Scale
        gun.transform.localScale = Vector3.Lerp(Vector3.zero, gunScaleTarget, gunTimer / gunTime);

        //Shoot the player
        if (gunTimer >= gunTime && guardCtrl.magCurrentSize > 0 && !guardCtrl.reloading)
        {
            animator.SetTrigger("Shoot");
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
