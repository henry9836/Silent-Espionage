using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public static class AdTime
{

    public static bool lastAdSuccessful = false;
    public static bool adServiceRunning = false;

    //Placement AD IDs (Get from unity dashboard)
    public const string ADID_LOSS = "LossDeathAd";

    //Game ID
    const string APPLE_STORE_GAME_ID = "3581266";
    const string ANDROID_STORE_GAME_ID = "3581267";

    public static void Initialize()
    {
#if UNITY_EDITOR
        Advertisement.Initialize(ANDROID_STORE_GAME_ID, true);
#endif
#if UNITY_ANDROID
        Advertisement.Initialize(ANDROID_STORE_GAME_ID, false);
#endif
//#if UNITY_IOS 
//        Advertisement.Initialize(APPLE_STORE_GAME_ID, false);
//#endif
    }

    public static bool AdThyme(string adID)
    {
        lastAdSuccessful = false;

        //Show Ad
        if (Advertisement.IsReady())
        {
            //Advertisement.Show(adID, new ShowOptions { resultCallback = HandleShowResult });
            Advertisement.Show(adID);
            lastAdSuccessful = true;
            return lastAdSuccessful;
        }

        //Ad Failed To Show
        return lastAdSuccessful;
    }

    //private static void HandleShowResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            {
    //                Debug.Log("The ad was successfully shown.");
    //                lastAdSuccessful = true;
    //                break;
    //            }
    //        case ShowResult.Skipped:
    //            {
    //                Debug.Log("The ad was skipped before reaching the end.");
    //                lastAdSuccessful = true;
    //                break;
    //            }
    //        case ShowResult.Failed:
    //            {
    //                Debug.LogError("The ad failed to be shown.");
    //                break;
    //            }
    //    }

    //    Debug.Log($"A issue occured when trying to handle the result of the ad: {result}");

    //}

}
