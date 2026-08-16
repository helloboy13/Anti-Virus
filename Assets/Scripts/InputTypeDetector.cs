using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputTypeDetector : MonoBehaviour
{
    private bool usingController = false;

    [SerializeField] private Image Interaction;
    [SerializeField] private Sprite interactKeyboardSprite;
    [SerializeField] private Sprite interactControllerSprite;

    [SerializeField] private Image Heal;
    [SerializeField] private Sprite healKeyboardSprite;
    [SerializeField] private Sprite healControllerSprite;

    [SerializeField] private Image EatPill;
    [SerializeField] private Sprite eatPillKeyboardSprite;
    [SerializeField] private Sprite eatPillControllerSprite;

    private const float stickDeadZone = 0.3f;

    private void Start()
    {
        usingController = false;
        KeyboardInput();
    }

    private void Update()
    {
        bool controllerUsed =
            Gamepad.current != null &&
            (
                Gamepad.current.buttonSouth.wasPressedThisFrame ||
                Gamepad.current.buttonNorth.wasPressedThisFrame ||
                Gamepad.current.buttonEast.wasPressedThisFrame ||
                Gamepad.current.buttonWest.wasPressedThisFrame ||

                Gamepad.current.leftShoulder.wasPressedThisFrame ||
                Gamepad.current.rightShoulder.wasPressedThisFrame ||

                Gamepad.current.startButton.wasPressedThisFrame ||
                Gamepad.current.selectButton.wasPressedThisFrame ||

                Gamepad.current.dpad.up.wasPressedThisFrame ||
                Gamepad.current.dpad.down.wasPressedThisFrame ||
                Gamepad.current.dpad.left.wasPressedThisFrame ||
                Gamepad.current.dpad.right.wasPressedThisFrame ||

                Gamepad.current.leftStick.ReadValue().magnitude > stickDeadZone ||
                Gamepad.current.rightStick.ReadValue().magnitude > stickDeadZone
            );

        bool keyboardUsed =
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null &&
            (
                Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame ||
                Mouse.current.delta.ReadValue() != Vector2.zero ||
                Mouse.current.scroll.ReadValue() != Vector2.zero
            ));

        if (controllerUsed && !usingController)
        {
            usingController = true;
            ControllerInput();
        }
        else if (keyboardUsed && usingController)
        {
            usingController = false;
            KeyboardInput();
        }
    }

    private void ControllerInput()
    {
        Debug.Log("Controller");

        Interaction.sprite = interactControllerSprite;
        Heal.sprite = healControllerSprite;
        EatPill.sprite = eatPillControllerSprite;
    }

    private void KeyboardInput()
    {
        Debug.Log("Keyboard");

        Interaction.sprite = interactKeyboardSprite;
        Heal.sprite = healKeyboardSprite;
        EatPill.sprite = eatPillKeyboardSprite;
    }
}