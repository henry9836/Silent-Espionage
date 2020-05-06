using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject ResetUI;
    public GameObject LevelSelectUI;

    private MillionairBaby iap;
    private bool userLogginedIntoGoogle = false;
    private Text debugger;


    void Debugger(string debugStr)
    {
        debugger.text += '\n' + debugStr;
    }

    private void Start()
    {
        debugger = GameObject.Find("DEBUG_OUTPUT").GetComponent<Text>();

        //Start Google Services
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, OnAuthenticated);

        //Connect to Google Services
        //PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();
        //PlayGamesPlatform.InitializeInstance(builder.Build());
        //Debugger
        //#if UNITY_EDITOR
        //PlayGamesPlatform.DebugLogEnabled = true;
        //#endif
        //PlayGamesPlatform.Activate();

        //Start Ad Service
        AdTime.Initialize();
        //Start Money Machine
        iap = GetComponent<MillionairBaby>();



    }

    private void OnAuthenticated(SignInStatus result)
    {
        switch (result)
        {
            case SignInStatus.Success:
                Debug.Log("GPG sign in successful");
                Debugger("GPG sign in successful");
                userLogginedIntoGoogle = true;
                break;
            default:
                Debug.LogWarning($"GPG sign in failed: {result}");
                Debugger($"GPG sign in failed: {result}");
                userLogginedIntoGoogle = false;
                break;
        }
    }

    private void FixedUpdate()
    {
        //if (!userLogginedIntoGoogle)
        //{
        //    Social.localUser.Authenticate((bool success, string errorMsg) =>
        //    {
        //        if (success)
        //        {
        //            Debug.Log("Auth success to google services");
        //            userLogginedIntoGoogle = true;
        //        }
        //        else
        //        {
        //            Debug.Log($"Failed to Auth to google services [{errorMsg}]");
        //        }
        //    });
        //}
    }

    public void resetSave(bool resetFlag)
    {
        //UI is shown
        if (ResetUI.activeInHierarchy)
        {
            //Reset Save
            if (resetFlag)
            {
                //Get ad playerpref
                int adFlag = PlayerPrefs.GetInt("noAdsPurchased");

                //Reset save data
                Debugger("Resetting Player Prefs...");
                PlayerPrefs.DeleteAll();

                //Restore purchases
                if (adFlag == 1)
                {
                    PlayerPrefs.SetInt("noAdsPurchased", adFlag);
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

    public void Play()
    {
        PlayerPrefs.Save();
        LevelSelectUI.SetActive(true);
        //SceneManager.LoadScene("Level1");
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
