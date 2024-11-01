using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputReader : MonoBehaviour
    {
        PlayerInput playerInput;
        InputAction selectAction;
        InputAction fireAction;

        public event Action Fire;

        // Sensitivity for joystick movement
        public float joystickSensitivity = 1000f;

        Vector2 mousePosition;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            // Bind actions for keyboard, mouse, and controller
            selectAction = playerInput.actions["Select"];
            fireAction = playerInput.actions["Fire"];
            fireAction.performed += context => Fire?.Invoke();

            // Initialize with the current mouse position
            mousePosition = Mouse.current.position.ReadValue();
        }

        void Update()
        {
            HandleMouseInput();
            HandleControllerInput();
        }

        void HandleMouseInput()
        {
            // Directly update mouse position when using the mouse
            if (Mouse.current.delta.ReadValue() != Vector2.zero)
            {
                mousePosition = Mouse.current.position.ReadValue();
            }

            // Detect an actual mouse click
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Fire?.Invoke();
            }
        }

        void HandleControllerInput()
        {
            // Move mouse with joystick input
            Vector2 joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (joystickInput.magnitude > 0.1f)
            {
                mousePosition += joystickInput * joystickSensitivity * Time.deltaTime;
                Mouse.current.WarpCursorPosition(mousePosition);
            }

            // Trigger left-click with the right trigger
            if (Input.GetButtonDown("Fire1") || playerInput.actions["Fire"].WasPressedThisFrame())
            {
                Fire?.Invoke();
            }
        }

        void OnDestroy()
        {
            fireAction.performed -= context => Fire?.Invoke();
        }

        public Vector2 Selected => mousePosition;
    }
}
