using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectBehind : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !animator.GetBool("CanSeePlayer"))
        {
            agent.ResetPath();
            agent.SetDestination(other.gameObject.transform.position);
        }
    }
}
