using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class UIMenuManager : MonoBehaviour
{
    private Animator CameraObject;
    [Header("MENUS")]
    [Tooltip("The Menu for when the MAIN menu buttons")]
    public GameObject mainMenu;
    [Tooltip("THe first list of buttons")]
    public GameObject firstMenu;
    [Tooltip("The Menu for when the PLAY button is clicked")]
    public GameObject playMenu;
    [Tooltip("The Menu for when the EXIT button is clicked")]
    public GameObject exitMenu;


    [Header("PANELS")]
    [Tooltip("The UI Panel parenting all sub menus")]
    public GameObject mainCanvas;
    [Tooltip("The UI Panel that holds the CONTROLS window tab")]
    public GameObject PanelControls;
    [Tooltip("The UI Sub-Panel under KEY BINDINGS for GENERAL")]
    public GameObject PanelGeneral;
    
    //Setting Screen
    [Header("SETTINGS SCREEN")]
    [Tooltip("Highlight Image for when GENERAL Tab is selected in Settings")]
    public GameObject lineGeneral;
    [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
    public GameObject lineControls;
  


    // Start is called before the first frame update
    void Start()
    {
        CameraObject = transform.GetComponent<Animator>();

        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        //if (extrasMenu) extrasMenu.SetActive(false);
        firstMenu.SetActive(true);
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    /*void Update()
    {
        // Check for a mouse click or touch input
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Play();
        }
    }

    public void Play()
    {
        //SceneManager.LoadScene("GameScene");
    }*/
    public void PlayCampaign()
    {
        exitMenu.SetActive(false);
        //if (extrasMenu) extrasMenu.SetActive(false);
        playMenu.SetActive(true);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void PlayCampaignMobile()
    {
        exitMenu.SetActive(false);
        //if (extrasMenu) extrasMenu.SetActive(false);
        playMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ReturnMenu()
    {
        playMenu.SetActive(false);
        //if (extrasMenu) extrasMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void DisablePlayCampaign()
    {
        playMenu.SetActive(false);
    }
    public void Position2()
    {
        DisablePlayCampaign();
        CameraObject.SetFloat("Animate", 1);
    }
    public void Position1()
    {
        CameraObject.SetFloat("Animate", 0);
    }

    void DisablePanels()
    {
        PanelControls.SetActive(false);
        PanelGeneral.SetActive(false);

        lineControls.SetActive(false);
        lineGeneral.SetActive(false);   
    }

    public void GeneralPanel()
    {
        DisablePanels();
        PanelGeneral.SetActive(true);
        lineGeneral.SetActive(true);
    }

    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }


    // Are You Sure - Quit Panel Pop Up
    public void AreYouSure()
    {
        exitMenu.SetActive(true);
        //if (extrasMenu) extrasMenu.SetActive(false);
        DisablePlayCampaign();
    }
    public void AreYouSureMobile()
    {
        exitMenu.SetActive(true);
       // if (extrasMenu) extrasMenu.SetActive(false);
        mainMenu.SetActive(false);
        DisablePlayCampaign();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
		Application.Quit();
        #endif
    }
}
