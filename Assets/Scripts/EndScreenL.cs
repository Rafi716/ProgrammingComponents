using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenL : MonoBehaviour
{
    [Header("LscreenButtons")]
    [SerializeField] Button ReturntoMenu;
    [SerializeField] Button RestartGame;


    // Start is called before the first frame update
    void Start()
    {
        ReturntoMenu.onClick.AddListener(ReturnToMain);
        RestartGame.onClick.AddListener(RestarttheGame);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReturnToMain() 
    {
        SceneManager.LoadScene("Main menu ui");
    }

    void RestarttheGame() 
    {
        SceneManager.LoadScene("OutdoorsScene");
    }
}
