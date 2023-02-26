using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] private GameObject player;

    [Header("Panels")]
    [SerializeField] private GameObject GameplayUI;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject DeathMenu;
    [SerializeField] private GameObject TutorialMenu;
    [SerializeField] private Image healthSlider;

    [Header("Tutorial Objects")]
    [SerializeField] private GameObject[] TutorialActions;
    [SerializeField] private GameObject storyUI;
    //[SerializeField] private GameObject attackUI;
    //[SerializeField] private GameObject swapUI;
    [SerializeField] private string[] TutorialTexts;
    [SerializeField] private TextMeshProUGUI storyText;
    private int StoryIndex;
    private int ActionIndex;
    private bool isTutorial;
    private bool inCouroutine = false;

    // Start is called before the first frame update
    void Start()
    {
        GameplayUI.SetActive(false);
        PauseMenu.SetActive(false);
        DeathMenu.SetActive(false);
        TutorialMenu.SetActive(true);
        healthSlider.fillAmount = 1.0f;
        Time.timeScale = 0.0f;
        ActionIndex = 0;
        isTutorial = false;

        StartTutorial();
    }

    

    void StartTutorial()
    {
        
        storyUI.SetActive(true);
        //Turns off all tutorial action UI
        for(int i = 0;i  < TutorialActions.Length; i++)
        {
            TutorialActions[i].SetActive(false);
        }
        StoryIndex = -1;
        showNextStoryIndex();
    }


    public void showNextStoryIndex()
    {
        if(StoryIndex >= TutorialTexts.Length-1)
        {
            storyUI.SetActive(false);
            endStory();
        }
        else
        {
            storyText.text = TutorialTexts[++StoryIndex];
        }

        
        
    }

    void endStory()
    {
        
        //storyUI.SetActive(false);
        //attackUI.SetActive(true);
        GameplayUI.SetActive(true);
        TutorialActions[0].SetActive(true);
        Time.timeScale = 1.0f;
        isTutorial = true;
    }

    //public void setSwapUI()
    //{
    //    swapUI.SetActive(true);
    //    attackUI.SetActive(false);
    //}

     IEnumerator  displayNextAction()
    {
        yield return new WaitForSeconds(0.5f);
        if (ActionIndex >= TutorialActions.Length-1)
        {
            endTutorial();
            TutorialActions[ActionIndex].SetActive(false);
            
        } else
        {
            TutorialActions[ActionIndex++].SetActive(false);
            TutorialActions[ActionIndex].SetActive(true);
        }

        inCouroutine = false;
    }

    public void endTutorial()
    {
        //swapUI.SetActive(false);
        isTutorial = false;
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //updateHealth();
            OpenPausePanel();
        }

        if (isTutorial && !inCouroutine)
        {
            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                && TutorialActions[0].activeInHierarchy)
            {
                inCouroutine = true;
                StartCoroutine(displayNextAction());
            }

            if (Input.GetAxis("Attack") != 0 && TutorialActions[1].activeInHierarchy)
            {
                inCouroutine = true;
                StartCoroutine(displayNextAction());
            }

            if (Input.GetAxis("Swap") != 0 && TutorialActions[2].activeInHierarchy)
            {
                inCouroutine = true;
                StartCoroutine(displayNextAction());
                //endTutorial();  
            }
        }



        //update player stats onto gameplay UI
        healthSlider.fillAmount = Mathf.Clamp(player.GetComponent<Health>().GetHealth()/100, 0, 1);


    }

    //IEnumerator delay(float delayTime)
    //{
    //    yield return delayTime;
    //}

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
