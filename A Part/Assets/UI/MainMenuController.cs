using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HTPMenu;

    // Start is called before the first frame update
    void Start()
    {

        //set up menus
        MainMenu.SetActive(true);
        HTPMenu.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {
        playButtonSound();
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        playButtonSound();
        Application.Quit();
    }

    public void playButtonSound()
    {
        GetComponent<AudioSource>().Play();
    }

    //HTP -> How To Play
    public void OpenHTPMenu()
    {
        HTPMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    //HTP -> How To Play
    public void CloseHTPMenu()
    {
        MainMenu.SetActive(true);
        HTPMenu.SetActive(false);
        
    }
}
