using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractArea : MonoBehaviour
{
    public UnityEvent onComplete;
    public float timeToComplete = 1.0f;
    [HideInInspector]
    public float progress = 0.0f;
    [HideInInspector]
    public bool completed = false;
    public bool multiUse = false;

    private bool multiUseOnceTrigger = true;
    private bool playerInArea;
    private float playerTimer = 0.0f;
    private Image progressUI;

    private void Start()
    {
        progressUI = GameObject.Find("Interact Progress").GetComponent<Image>();
    }

    void Update()
    {
        if ((!completed || multiUse) && multiUseOnceTrigger)
        {
            if (playerInArea)
            {
                playerTimer += Time.unscaledDeltaTime;
                playerTimer = Mathf.Clamp(playerTimer, 0.0f, 1.0f);

                progress = playerTimer / timeToComplete;

                progressUI.fillAmount = progress;

                if (progress >= 1.0f && multiUseOnceTrigger)
                {
                    onComplete.Invoke();
                    progressUI.fillAmount = 0.0f;
                    completed = true;
                    multiUseOnceTrigger = false;
                }
            }
            else
            {
                playerTimer -= Time.unscaledDeltaTime;
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
            multiUseOnceTrigger = true;
            progressUI.fillAmount = 0.0f;
        }
    }

}
