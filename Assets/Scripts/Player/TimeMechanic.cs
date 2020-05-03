using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimeMechanic : MonoBehaviour
{

    public float timeToChange = 0.2f;

    private NavMeshAgent agent;
    private float moveTimer = 0.0f;
    private float normalFixedDeltaTime = 0.02f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        //Lerp time based on movement


        Time.timeScale = Mathf.Lerp(0.1f, 1.0f, Mathf.Clamp(agent.velocity.magnitude, 0.0f, 1.0f) / timeToChange);
        Time.fixedDeltaTime = Time.timeScale * normalFixedDeltaTime;


    }
}
