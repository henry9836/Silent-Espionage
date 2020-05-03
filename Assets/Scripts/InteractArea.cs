using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractArea : MonoBehaviour
{
    public UnityEvent onComplete;
    public float timeToComplete = 1.0f;
    [HideInInspector]
    public float progress = 0.0f;
    [HideInInspector]
    public bool completed = false;

    private bool playerInArea;
    private float playerTimer = 0.0f;


    void Update()
    {
        if (!completed)
        {
            if (playerInArea)
            {
                playerTimer += Time.unscaledDeltaTime;
            }
            else
            {
                playerTimer -= Time.unscaledDeltaTime;
            }

            playerTimer = Mathf.Clamp(playerTimer, 0.0f, 1.0f);

            progress = playerTimer / timeToComplete;

            if (progress >= 1.0f)
            {
                onComplete.Invoke();
                completed = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInArea = false;
        }
    }

}
