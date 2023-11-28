using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button PlayButton; // button for play
    [SerializeField] Button OptionsButton; // option menu
    [SerializeField] Button QuitButton; // button for quitting
    [SerializeField] Button BackButton; // button for going back
    [SerializeField] TMP_Dropdown ResolutionDropdownmenu;

    [SerializeField] GameObject OptionMenu;

    // Start is called before the first frame update
    void Start()
    {
        PlayButton.onClick.AddListener(LoadGameScene); // on click checks if its pressed and listens if it has the scene will change
        OptionsButton.onClick.AddListener(EnterOptionsMenu);
        OptionMenu.SetActive(false);
        PopulateResolutionDropDown();
        ResolutionDropdownmenu.onValueChanged.AddListener(ChangeResolution);
        int SavedResIndex = PlayerPrefs.GetInt("ResolutionIndex", 0); // retrieves the previous saved resolution or it will default to 0 teh default resoultion
        ResolutionDropdownmenu.value = SavedResIndex; // sets the value from the dropdown to the savedresindex 
        ChangeResolution(SavedResIndex); // changeresolution method is called with the savedresindex arguement changing the resolution of the screen based on its index
        BackButton.onClick.AddListener(BackToMenu);
        QuitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGameScene() // method to load the game
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex; // local variable currentindexscene will equal to the scenemanages active scene with its current build index (0)
        SceneManager.LoadScene(CurrentSceneIndex + 1); // index is incremented to the next scene 
    }

    void EnterOptionsMenu() 
    {
        OptionMenu.SetActive(true); // optionmenu opens
        gameObject.SetActive(false); // our gameobject that the script is on is deactivated
    }

    void PopulateResolutionDropDown() // populates the dropdown method
    {
        ResolutionDropdownmenu.ClearOptions(); // clear options will remove existing options in the dropdownmenu
        List<string> Resoptions = new List<string>(); // a list which will hold the resolutions
        Resolution[] prefResolutions = Screen.resolutions; // gets an array of all the supported screen resolutions

        foreach (var res in prefResolutions) // loops through for each available resolution
        {
            Resoptions.Add(res.width + "x" + res.height); // formats the width and height into string which is added after
        }
        ResolutionDropdownmenu.AddOptions(Resoptions); // add the list of the newly formatted strings as options in the dropdown
    }

    void ChangeResolution(int Index) 
    {
        Resolution[] PrefResolutions = Screen.resolutions; // get an array of supported screen resolutions
        if (Index < 0 || Index >= PrefResolutions.Length) // checks if the list of resolutions is within the correct range of resolutions 
        {
            return; // if the index is invalid exit the method
        }
        Resolution SelectedResolution = PrefResolutions[Index]; // what ever index is chosen is what resolution that will be selected
        Screen.SetResolution(SelectedResolution.width, SelectedResolution.height, Screen.fullScreen); // set the screen resolution to the selected width and height  
        PlayerPrefs.SetInt("ResolutionIndex", Index); // store the resolution index using playerprefs so that it continues to the next scene
        PlayerPrefs.Save(); // save the resolution data 
    }

    void BackToMenu() 
    {
        OptionMenu.SetActive(false); // optionmenu is deactivated
        gameObject.SetActive(true); // the main menu is back open
    }

    void QuitGame() // quit method
    {
        Debug.Log("You quit!");
        Application.Quit();
    }
}
