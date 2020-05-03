using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<string> objectiveLines = new List<string>();
    public Text currentTime;
    public Text bestTime;
    public Text currentObjective;
    public int LevelID = -1;
    
    private int objectivesCompleted = 0;
    private string timerString;
    private float timer = 0.0f;
    private float bestTimeFloat = Mathf.Infinity;
    private GameObject player;
    private PlayerController playerCtrl;
    private bool shownAd = false;

    public void objectiveCompleted()
    {
        objectivesCompleted++;
    }

    private void Start()
    {
        currentObjective.text = objectiveLines[objectivesCompleted];
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<PlayerController>();

        //Load best time if there is one
        bestTimeFloat = PlayerPrefs.GetFloat("LEVELBESTTIME" + LevelID.ToString());
        bestTime.text = (Mathf.Round(bestTimeFloat * 100f) / 100f).ToString();

        //Start Ad Service
        AdTime.Initialize();
    }

    private void Update()
    {
        timerString = (Mathf.Round(timer * 100f) / 100f).ToString();
        currentTime.text = timerString;
        //During level
        if (objectivesCompleted < objectiveLines.Count)
        {
            currentObjective.text = objectiveLines[objectivesCompleted];
            timer += Time.unscaledDeltaTime;
        }
        //Completed Level
        else
        {
            Debug.Log("Win!");
            PlayerPrefs.SetFloat("LEVELBESTTIME" + LevelID.ToString(), (Mathf.Round(timer * 100f) / 100f));
        }

        //Lost Level
        if (playerCtrl.amDead)
        {
            //Show Ad
            if (!shownAd)
            {
                StartCoroutine(showAd());
                shownAd = true;
            }
            Debug.Log("Lost");
        }


        IEnumerator showAd(){
            yield return new WaitForSeconds(1.5f);
            AdTime.AdThyme(AdTime.ADID_LOSS);
        }

    }

}
