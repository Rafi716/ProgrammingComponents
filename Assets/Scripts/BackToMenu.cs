using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    [SerializeField] Button ReturnMenuButton; // back button reference

    // Start is called before the first frame update
    void Start()
    {
        ReturnMenuButton.onClick.AddListener(ReturnToMenu); // reference the backbutton in the scene and activate the method when clicked
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReturnToMenu() 
    {
        SceneManager.LoadScene("Main menu ui"); // go back to scene 1
        Time.timeScale = 1; // unpause
    }    
}

