using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager; // reference our gamemanager script
    
    [Header("ReferenceEnemy")]
    public GameObject TargetEnemy;

    [Header("Flashlight")]
    public GameObject FlashLightOverlay; 
    public float DestroyDelay = 3.0f; // the delay timer for the invoke repeating 

    private bool Destroyed = false; // bool for when the object had been destroyed
    private bool FlashLightActive = false;  // bool for on and off flashlight
    private Light FlashlightObj; // were going to get ahold of the light itself

    [SerializeField] GameObject BatteryDropped; 

    RaycastHit hit; // raycast variable 

    [Header("Movement")]
    [SerializeField] float PlayerSpeed = 7.0f; // players speed
    [SerializeField] float PlayerSprintSpeed = 9.0f; // players sprint
    [SerializeField] float JumpMultiplier; // player jump multiplier
    private Vector2 CurrentMovementInput;
    private Rigidbody PlayerRigidbody;
    private bool IsSprinting;

    public LayerMask GroundLayer;

    [Header("Camera turn")]
    public Transform CameraOBJ; // obj to transform the value of the camera within the CameraOBJ object
    public float MinXTurn; // min and max for looking to avoid the spinning bug
    public float MaxXTurn; 
    public float LookSens; // value for changing the sensitivity of looking around

    private Vector2 MouseDelta;
    private float CamCurrentXrotation; // get the cameras current location
    internal static object instance;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        FlashlightObj = GameObject.Find("FlashLightOBJ").GetComponent<Light>(); // automatically gets the spotlight on the character through string reference
        FlashlightObj.enabled = false; // turns off the component (the light)
        FlashLightOverlay.SetActive(false); // the flashlight shall be off at the start
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Flashlight();
        if (FlashLightActive == true) // during the update, if the flashlight is on
        {
            RaycastIsOn();
            BatteryDepleted(); // this function is ran in order to see if the battery had depleted
        }
    }

    private void Flashlight()
    {
        if (Input.GetKeyDown(KeyCode.F)) // when key F is pressed
        {
            if (FlashLightActive == false) // if the flashlight isnt active
            {
                FlashLightOverlay.GetComponent<Flashlight>().StartDrain();
                FlashLightOverlay.SetActive(true); // flashlightoverlay is on
                FlashlightObj.enabled = true; // spot light turns on
                FlashLightActive = true; // flashlight bool is turned on
                BatteryDepleted(); // this method will run if the battery had depleted so that the light doesnt turn on when power outage
            }
            else if (FlashLightActive == true) // else the flashlight is active
            {
                FlashLightOverlay.SetActive(false); // turn off the overlay
                FlashlightObj.enabled = false; // spot light turns off
                FlashLightOverlay.GetComponent<Flashlight>().StopDrain(); // call the function from the flashlight script in order to turn the drainage off 
                FlashLightActive = false; // switch of the flashlight bool

                if (Destroyed == true) // if the object has destroyed
                {
                    CancelInvoke("DestroyTarget"); // invoke repeating function is stopped
                    Destroyed = false; // destroyed is set back to false
                }
            }
        }
    }

    private void RaycastIsOn() // this method will allow the raycast to be on conitnuously under the update function only if the flashlightactive bool is true
    {
        if (FlashLightActive == true) // if the bool for the flashlight is active
        {
            float MaxRayDistance = FlashlightObj.range; // a float cache variable holding the max value for the raycast can reach which in this case is the range the spotlight can reach
            if (Physics.Raycast(FlashlightObj.transform.position, FlashlightObj.transform.forward, out hit, MaxRayDistance, LayerMask.GetMask("Enemy"))) // the ray is positioned in the direction of the light and if the variable "hit" contains the details of the colliders position as well as containing the max distance the ray will travel and lastly specifiying the layer the ray will only hit in the enemy layer 
            {
                if (!Destroyed) // if an enemy hit by the ray cast and hasnt been destroyed 
                {
                    TargetEnemy = hit.collider.gameObject; // the enemy thats being hit is set to the target that will be destroyed
                    InvokeRepeating("DestroyTarget", DestroyDelay, DestroyDelay); // the invoke repeating function is active in order to destroy said gameobject
                    Destroyed = true; // destroyed is true marking it as destroyed
                }
                else if (hit.collider.gameObject != TargetEnemy && Destroyed) // if a different enemy is hit or the one before had been destroyed
                {
                    CancelInvoke("DestroyTarget"); // cancel the invoke 
                    Destroyed = false; // destroy is set back to false
                }
            }
            else // or the ray hits nothing
            {
                CancelInvoke("DestroyTarget"); // stop the destroy method
                Destroyed = false; // reset to false
            }
        }
    }

    private void DestroyTarget() // method for destroying a gameobject
    {
        if (BatteryDropped != null)
        {
            Vector3 targetPosition = TargetEnemy.transform.position; // Store the position of the target enemy in our vector 3 local variable
            Instantiate(BatteryDropped, targetPosition, Quaternion.identity); // Instantiate the dropObject at the target's position
            Destroy(TargetEnemy); // Destroy the target object
            Destroyed = false; // reset the destroy to false to allow us to destroy another target 
        }
        else 
        {
            Debug.Log("non existent");
        }
    }

    private void BatteryDepleted() // method for when battery runs out
    {
        if (FlashLightOverlay.GetComponent<Flashlight>().BatteryPower <= 0) // if the batterypower from the flashlight script is 0
        {
            FlashLightOverlay.SetActive(false); // flashlightoverlay should be falsed (turned off)
            FlashlightObj.enabled = false; // spotlight is turned off
            FlashLightOverlay.GetComponent<Flashlight>().StopDrain(); // stop drain function is activated since there is nothing to drain
            FlashLightActive = false; // flashlight is definitely turned off 
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void LateUpdate()
    {
        CameraFollow(); // camera will move after the player had moved hence the late update
    }

    private void PlayerMove()
    {
        float PlayerNormSpeed = PlayerSpeed; // Local cache variable turning holding players normal speed

        if (IsSprinting && MovingForwards()) // checks if the player is sprinting and moving forwards
        {
            PlayerNormSpeed = PlayerSprintSpeed; // if sprinting  and going in the forwards direction, set the sprintspeed as the players speed
        }

        Vector3 DirectionMoved = transform.forward * CurrentMovementInput.y + transform.right * CurrentMovementInput.x; // Calculate the direction based on the players input and speed
        DirectionMoved *= PlayerNormSpeed; // apply the speed to the player
        DirectionMoved.y = PlayerRigidbody.velocity.y; // maintain the y velocity
        PlayerRigidbody.velocity = DirectionMoved; // update the players velocity
    }

    public void OnSprint(InputAction.CallbackContext context) 
    {
        if (context.started) // if the sprint speed had commenced 
        {
            IsSprinting = true; // sprining bool is true 
        }
        else if (context.canceled) // if the action is canceled 
        {
            IsSprinting = false; // set the sprinting to false
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // condition for when a button is pressed
        {
            CurrentMovementInput = context.ReadValue<Vector2>(); // player will move
        }
        else if (context.phase == InputActionPhase.Canceled) // condition for button isnt being pressed
        {
            CurrentMovementInput = Vector2.zero; // stop the player from moving 
        }
        if (context.action.name == "Sprint") // checks if the input action is sprinting
        {
            OnSprint(context); // call the onsprint method
        }
    }

    private bool MovingForwards() 
    {
        return CurrentMovementInput.y > 0.1f; // cehck if the player is moving forwards
    }

    public void OnJump(InputAction.CallbackContext context) 
    { 
        if (context.phase == InputActionPhase.Performed) // when the buttons pressed
        {
            if (IsGrounded()) // is player on the ground layer
            {
                PlayerRigidbody.AddForce(Vector3.up * JumpMultiplier, ForceMode.Impulse); // add force upwards to the player
            }
        }
    }

    bool IsGrounded() // the method is a bool whihc will cast rays downwards from our player 
    {
        Ray[] jumpRays = new Ray[4] // an array which will point in different directions around the player
        {
            new Ray (transform.position + (transform.forward * 0.2f), Vector3.down), // the many directions the ray will be coming from
            new Ray (transform.position + (-transform.forward * 0.2f), Vector3.down),
            new Ray (transform.position + (transform.right * 0.2f), Vector3.down),
            new Ray (transform.position + (-transform.right * 0.2f), Vector3.down)
        };

        for (int i = 0; i < jumpRays.Length; i++) // i will increment as long as our local variable jumprays is bigger than it
        {
            if (Physics.Raycast(jumpRays[i], 0.1f, GroundLayer)) // raycast will face downwards due to the vector3.down command from our local variable the ray will be 0.1f long to check for ground collisions
            {
                return true; // exits the method 
            }
        }

        return false; // false returned if the rays dont hit the ground layer
    }

    private void OnDrawGizmos() // this allowed me to draw the gizmos of the raycast 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down); // visual representation of the rays 
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);

        if (FlashLightActive) // if the flashlight is on, gizmos will be drawn
        {
            float MaxRayDistance = FlashlightObj.range; // local variable for holding the max ray length of the flashlight

            Gizmos.color = Color.red; 
            Vector3 directionofray = FlashlightObj.transform.forward; // calculates the rays direction based on the flashlights forwards direction
            Gizmos.DrawRay(FlashlightObj.transform.position, directionofray * MaxRayDistance); // draw the gizmo from the flashlight in the forwards position and multiply the distance by the flashlughts range by the
        }
    }

    public void LookInput(InputAction.CallbackContext context) // function to see if player had pressed a certain button or not 
    {
        if (!gameManager.IsPaused) 
        {
            MouseDelta = context.ReadValue<Vector2>(); // MouseDelta variable will hold the vector2 value within it
        }
    }

    void CameraFollow()
    {
        if (!gameManager.IsPaused) // checks in our gamanager script if the game is currently not paused
        {
            CamCurrentXrotation += MouseDelta.y * LookSens; // add the mousevalue and times the rotation to the camera and finally multiply it all by the sensetivity value
            CamCurrentXrotation = Mathf.Clamp(CamCurrentXrotation, MinXTurn, MaxXTurn); // limits the up and down loooking of the character
            CameraOBJ.localEulerAngles = new Vector3(-CamCurrentXrotation, 0, 0); // adds the camcurrent x rotation to the cameraOBJ gameobject
            transform.eulerAngles += new Vector3(0, MouseDelta.x * LookSens, 0);
        }
        else // if it is paused then to avoid the player from spinning around
        {
            MouseDelta = Vector2.zero; // mousedelta is reset to 0 so there is no accumulated mouse movement when paused
        }
    }
}