using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PausePanel; // opens our pause panel i created
    public bool IsPaused = false; // bool to check if it the game is paused

    void Start()
    {
        PausePanel.SetActive(false); // pause is always set to false at the start
        PlayerInput playerInput = GetComponent<PlayerInput>(); // Get a reference to the PlayerInput component on this GameObject
        playerInput.onActionTriggered += PauseTriggered; // Subscribe to the PlayerInput events for pause activation
    }

    public void PauseTriggered(InputAction.CallbackContext context) // our pause method
    {
        if (context.action.name == "Pause") // Check if the triggered action is for pausing
        {
            IsPaused = !IsPaused; // Toggle pause state
            Time.timeScale = IsPaused ? 0 : 1; // Freeze time when paused
            PausePanel.SetActive(IsPaused);
            Cursor.visible = IsPaused; // cursor is set the visible only if the game is paused
            Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked; // cursor is checked if its paused, if so then the cursor is shown, otherwise the cursor is locked and not visible
        }
    }
}


