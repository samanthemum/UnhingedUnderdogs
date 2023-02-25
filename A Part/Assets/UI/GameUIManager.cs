using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private GameObject GameplayUI;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject DeathMenu;
    [SerializeField] private Image healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameplayUI.SetActive(true);
        PauseMenu.SetActive(false);
        DeathMenu.SetActive(false);
        healthSlider.fillAmount = 1.0f;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            updateHealth();
            OpenPausePanel();
        }

        //Bring up death menu when player dies
        //Node: a delay will be a nice feature to add to this
        
    }

    public void updateHealth()
    {
        //Note: we will want to change this to player health/max health later
        healthSlider.fillAmount = Mathf.Clamp(0.6f, 0.0f, 1.0f);
    }

    public void OpenPausePanel()
    {
        Time.timeScale = 0.0f;
        PauseMenu.SetActive(true);
    }

    public void closePausePanel()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void openDeathMenu()
    {
        DeathMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    
}
