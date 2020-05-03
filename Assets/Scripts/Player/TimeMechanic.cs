using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimeMechanic : MonoBehaviour
{

    public float timeToChange = 0.2f;

    private NavMeshAgent agent;
    private PlayerController playerCtrl;
    private float moveTimer = 0.0f;
    private float normalFixedDeltaTime = 0.02f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalFixedDeltaTime = Time.fixedDeltaTime;
        playerCtrl = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        //Lerp time based on movement
        if (!playerCtrl.amDead)
        {
            Time.timeScale = Mathf.Lerp(0.1f, 1.0f, Mathf.Clamp(agent.velocity.magnitude, 0.0f, 1.0f) / timeToChange);
        }
        else
        {
            moveTimer += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(0.1f, 1.0f, moveTimer / timeToChange);
        }
        Time.fixedDeltaTime = Time.timeScale * normalFixedDeltaTime;


    }
}
