using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public List<string> objectiveLines = new List<string>();
    public Text currentTime;
    public Text bestTime;
    public Text currentObjective;
    public int LevelID = -1;
    public GameObject restartUI;
    public GameObject continueUI;
    [HideInInspector]
    public bool levelOver = false;

    
    private int objectivesCompleted = 0;
    private string timerString;
    private float timer = 0.0f;
    private float bestTimeFloat = 0.0f;
    private GameObject player;
    private PlayerController playerCtrl;
    private bool shownAd = false;
    private bool started = false;

    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void objectiveCompleted()
    {
        objectivesCompleted++;
    }

    void endScreen()
    {
        levelOver = true;

        restartUI.SetActive(true);

        if (playerCtrl.amDead)
        {
            if (bestTimeFloat != 0.0f)
            {
                continueUI.SetActive(true);
            }
        }
        else
        {
            continueUI.SetActive(true);
        }
    }

    private void Start()
    {
        currentObjective.text = objectiveLines[objectivesCompleted];
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<PlayerController>();

        //Load best time if there is one
        bestTimeFloat = PlayerPrefs.GetFloat("LEVELBESTTIME" + LevelID.ToString());

        if (bestTimeFloat > 0.0f)
        {
            bestTime.text = (Mathf.Round(bestTimeFloat * 100f) / 100f).ToString();
        }
        //Start Ad Service
        AdTime.Initialize();
    }

    private void Update()
    {

        if (started)
        {

            //timerString = (Mathf.Round(timer * 100f) / 100f).ToString();
            currentTime.text = timer.ToString("F2");
            //During level
            if (objectivesCompleted < objectiveLines.Count && !playerCtrl.amDead)
            {
                currentObjective.text = objectiveLines[objectivesCompleted];
                timer += Time.unscaledDeltaTime;
            }
            //Completed Level
            else if (!playerCtrl.amDead)
            {
                endScreen();
                currentObjective.text = "You Win!";
                //If we have a new best time
                if (bestTimeFloat > (Mathf.Round(timer * 100f) / 100f))
                {
                    PlayerPrefs.SetFloat("LEVELBESTTIME" + LevelID.ToString(), (Mathf.Round(timer * 100f) / 100f));
                }
            }

            //Lost Level
            if (playerCtrl.amDead)
            {
                currentObjective.text = "Game Over";
                //Show Ad
                if (!shownAd)
                {
                    StartCoroutine(showAd());
                    shownAd = true;
                }
            }
        }
        else
        {
            currentTime.text = timer.ToString("F2");
            if (player.GetComponent<NavMeshAgent>().velocity.magnitude > 0.0f)
            {
                started = true;
            }
        }
    }

    IEnumerator showAd()
    {
        yield return new WaitForSeconds(1.5f);
        AdTime.AdThyme(AdTime.ADID_LOSS);
        endScreen();
    }

}
