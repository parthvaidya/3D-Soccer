using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerinput;
    private PlayerInput.PlayerActionsActions actions;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {

        playerinput = new PlayerInput();
        actions = playerinput.PlayerActions;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerController.ProcessLook(actions.Look.ReadValue<Vector2>()); // Handle look input
    }

    private void FixedUpdate()
    {
        playerController.processMove(actions.Move.ReadValue<Vector2>()); // Handle movement input
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Shoot.performed += _ => playerController.Shoot(); // Register shoot action
    }

    private void OnDisable()
    {
        actions.Disable();
        actions.Shoot.performed -= _ => playerController.Shoot(); // Unregister shoot action
    }
}
