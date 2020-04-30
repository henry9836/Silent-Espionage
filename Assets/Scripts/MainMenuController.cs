using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    MillionairBaby iap;

    private void Start()
    {
        //Start Ad Service
        AdTime.Initialize();
        //Start Money Machine
        iap.Initialize();
    }

    public void buyNoAds()
    {

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
