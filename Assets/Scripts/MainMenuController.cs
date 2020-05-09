using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject ResetUI;
    public GameObject LevelSelectUI;
    public Button lvl1;
    public Button lvl2;
    public Button lvl3;

    private MillionairBaby iap;
    private bool userLogginedIntoGoogle = false;
    private Text debugger;


    void Debugger(string debugStr)
    {
        if (debugger)
        {
            debugger.text += '\n' + debugStr;
        }
    }

    private void Start()
    {
        //Find Debugger
        if (GameObject.Find("DEBUG_OUTPUT")) {
            debugger = GameObject.Find("DEBUG_OUTPUT").GetComponent<Text>();
        }

        //Start Google Services
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, OnAuthenticated);

        //Start Ad Service
        AdTime.Initialize();
        //Start Money Machine
        iap = GetComponent<MillionairBaby>();

        //Check for ads purchased
        if (PlayerPrefs.GetInt("noAdsPurchased") == 0)
        {
            if (iap.checkPurchase(iap.removeAds))
            {
                PlayerPrefs.SetInt("noAdsPurchased", 1);
            }
        }

    }

    private void FixedUpdate()
    {
        //Enable Buttons
        lvl2.interactable = (PlayerPrefs.HasKey("LEVELBESTTIME1"));
        lvl3.interactable = (PlayerPrefs.HasKey("LEVELBESTTIME2"));
    }

    private void OnAuthenticated(SignInStatus result)
    {
        switch (result)
        {
            case SignInStatus.Success:
                Debugger("GPG sign in successful");
                userLogginedIntoGoogle = true;
                break;
            default:
                Debugger($"GPG sign in failed: {result}");
                userLogginedIntoGoogle = false;
                break;
        }
    }

    public void resetSave(bool resetFlag)
    {
        //UI is shown
        if (ResetUI.activeInHierarchy)
        {
            //Reset Save
            if (resetFlag)
            {
                //Reset save data
                Debugger("Resetting Player Prefs...");
                PlayerPrefs.DeleteAll();

                //Restore purchases
                //Check for ads purchased
                if (iap.checkPurchase(iap.removeAds))
                {
                    PlayerPrefs.SetInt("noAdsPurchased", 1);
                }

                ResetUI.SetActive(false);
            }
            //Close UI
            else
            {
                ResetUI.SetActive(false);
            }
        }
        //UI is not shown
        else
        {
            ResetUI.SetActive(true);
        }
    }

    public void buyNoAds()
    {
        iap.Purchase(iap.removeAds);
    }

    public void showAchieve()
    {
        Social.ShowAchievementsUI();
    }

    public void showLeader()
    {
        Social.ShowLeaderboardUI();
    }

    public void Play()
    {
        PlayerPrefs.Save();
        LevelSelectUI.SetActive(true);
    }

    public void closeLevelSelectUI()
    {
        LevelSelectUI.SetActive(false);
    }

    public void loadLevel(int levelID)
    {
        SceneManager.LoadScene("Level" + levelID.ToString());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
