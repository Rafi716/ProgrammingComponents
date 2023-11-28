using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenResSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0); // int variable savedredolutionindex will always be what ever playerprefs has stored otherwise its default
        Resolution[] resolutions = Screen.resolutions; // get the array of all available resolutions
        if (savedResolutionIndex >= 0 && savedResolutionIndex < resolutions.Length) // checks the range of availble resolutions
        {
            Resolution selectedResolution = resolutions[savedResolutionIndex]; // retrieve the resolution from the list that is the saved resolution 
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen); // sets the selected resolution to the game
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
