using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private MillionairBaby iap;
    private bool userLogginedIntoGoogle = false;

    private void Start()
    {
        //Start Ad Service
        AdTime.Initialize();
        //Start Money Machine
        iap = GetComponent<MillionairBaby>();


        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, OnAuthenticated);

        ////Connect to Google Services
        //PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();
        //PlayGamesPlatform.InitializeInstance(builder.Build());
        ////Debugger
        ////#if UNITY_EDITOR
        //PlayGamesPlatform.DebugLogEnabled = true;
        ////#endif
        //PlayGamesPlatform.Activate();

    }

    private void OnAuthenticated(SignInStatus result)
    {
        switch (result)
        {
            case SignInStatus.Success:
                Debug.Log("GPG sign in successful");
                break;
            default:
                Debug.Log($"GPG sign in failed: {result}");
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

    public void buyNoAds()
    {
        iap.Purchase(iap.removeAds);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
