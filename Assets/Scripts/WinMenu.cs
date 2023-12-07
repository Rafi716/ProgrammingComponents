using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] Button BackToMenu;

    // Start is called before the first frame update
    void Start()
    {
        BackToMenu.onClick.AddListener(ReturnMainMenu);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReturnMainMenu() 
    {
        SceneManager.LoadScene("Main menu ui");
    }
}
