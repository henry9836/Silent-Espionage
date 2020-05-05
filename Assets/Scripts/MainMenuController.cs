using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private MillionairBaby iap;

    private void Start()
    {
        //Start Ad Service
        AdTime.Initialize();
        //Start Money Machine
        iap = GetComponent<MillionairBaby>();
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
