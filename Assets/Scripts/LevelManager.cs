using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;

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
    [HideInInspector]
    public float timer = 0.0f;


    private int objectivesCompleted = 0;
    private string timerString;
    private float bestTimeFloat = 0.0f;
    private GameObject player;
    private PlayerController playerCtrl;
    private bool shownAd = false;
    private bool started = false;
    private MillionairBaby iapController;
    private bool adsEnabled = true;
    private Text debugger;
    private bool achivementOnce = false;

    public void nextLevel()
    {

        if (SceneManager.sceneCount - 1 > SceneManager.GetActiveScene().buildIndex)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
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
            if (bestTimeFloat != Mathf.Infinity)
            {
                continueUI.SetActive(true);
            }
        }
        else
        {
            continueUI.SetActive(true);
        }
    }

    void Debugger(string debugStr)
    {
        debugger.text += '\n' + debugStr;
    }

    void SubmitTime()
    {
        Debugger("Submitting New Score Online...");
        switch (LevelID)
        {

            case 1:
                {
                    Social.ReportScore((long)(timer * 1000.0f), SlientEspionageAchievements.leaderboard_best_time_level_1, (bool success) =>
                    {
                        if (success)
                        {
                            Debugger("Leaderboard Submitted");
                        }
                        else
                        {
                            Debugger("Leaderboard Failed");
                        }
                    });
                    break;
                }

            default:
                break;
        }
    }

    private void Start()
    {
        currentObjective.text = objectiveLines[objectivesCompleted];
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<PlayerController>();

        //Load best time if there is one
        bestTimeFloat = PlayerPrefs.GetFloat("LEVELBESTTIME" + LevelID.ToString());

        achivementOnce = false;

        if (bestTimeFloat > 0.0f)
        {
            bestTime.text = FloatToTime.convertFloatToTime(bestTimeFloat);
            //bestTime.text = (Mathf.Round(bestTimeFloat * 100f) / 100f).ToString();
        }
        else
        {
            bestTimeFloat = Mathf.Infinity;
        }
        //Check for purchase of removal of ads
        adsEnabled = true;

        if (PlayerPrefs.GetInt("noAdsPurchased") == 1)
        {
            adsEnabled = false;
        }

        if (adsEnabled)
        {
            //Start Ad Service
            AdTime.Initialize();
        }

        //DEBUGGER
        debugger = GameObject.Find("DEBUG_OUTPUT").GetComponent<Text>();
        Debugger("Best Time: " + bestTimeFloat.ToString());
        Debugger("AD BOOL: " + PlayerPrefs.GetInt("noAdsPurchased").ToString());

    }

    private void Update()
    {

        if (started)
        {
            currentTime.text = FloatToTime.convertFloatToTime(timer);
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
                    if (!achivementOnce)
                    {
                        //Submit to leaderboard
                        SubmitTime();
                    }
                }

                if (!achivementOnce)
                {
                    PlayerPrefs.Save();
                    //Achievement Logic
                    GetAchievement();
                }
            }

            //Lost Level
            if (playerCtrl.amDead)
            {
                currentObjective.text = "Game Over";
                //Show Ad
                if (!shownAd)
                {
                    PlayerPrefs.Save();
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

    //Gives an achievement to user according to the level ID
    void GetAchievement()
    {
        achivementOnce = true;
        Debugger("Attempting to gain achievement...");
        switch (LevelID)
        {
            //Level 1
            case 1:
                {
                    Social.ReportProgress(SlientEspionageAchievements.achievement_completed_level_1, 100, (bool success) => {
                        if (success)
                        {
                            Debugger("Achievement Successful Unlocked");
                        }
                        else
                        {
                            Debugger("Achievement Failed to unlock");
                        }
                    });
                    break;
                }

            case 2:
                {
                    Social.ReportProgress(SlientEspionageAchievements.achievement_completed_level_2, 100, (bool success) => {
                        if (success)
                        {
                            Debugger("Achievement Successful Unlocked");
                        }
                        else
                        {
                            Debugger("Achievement Failed to unlock");
                        }
                    });
                    break;
                }

            case 3:
                {
                    Social.ReportProgress(SlientEspionageAchievements.achievement_completed_level_3, 100, (bool success) => {
                        if (success)
                        {
                            Debugger("Achievement Successful Unlocked");
                        }
                        else
                        {
                            Debugger("Achievement Failed to unlock");
                        }
                    });

                    

                    break;
                }

            default:
                {
                    Debug.LogWarning($"No Achievement Logic Setup for Level ID [{LevelID}]");
                    Debugger($"No Achievement Logic Setup for Level ID [{LevelID}]");
                    break;
                }
        }

        //Check for 150 seconds or less completion time on all levels
        if (PlayerPrefs.HasKey("LEVELBESTTIME1") && PlayerPrefs.HasKey("LEVELBESTTIME2") && PlayerPrefs.HasKey("LEVELBESTTIME3"))
        {
            if ((PlayerPrefs.GetFloat("LEVELBESTTIME1") <= 150.0f) && (PlayerPrefs.GetFloat("LEVELBESTTIME2") <= 150.0f) && (PlayerPrefs.GetFloat("LEVELBESTTIME3") <= 150.0f))
            {
                Social.ReportProgress(SlientEspionageAchievements.achievement_master_saboteur, 100, (bool success) =>
                {
                    if (success)
                    {
                        Debugger("Achievement Successful Unlocked");
                        Social.ShowAchievementsUI();
                    }
                    else
                    {
                        Debugger("Achievement Failed to unlock");
                    }
                });
            }
        }

    }

    IEnumerator showAd()
    {
        yield return new WaitForSeconds(1.5f);
        if (adsEnabled)
        {
            AdTime.AdThyme(AdTime.ADID_LOSS);
        }
        endScreen();
    }

}
