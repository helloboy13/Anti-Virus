using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationComeplete : MonoBehaviour
{
    public static AnimationComeplete Instance;

    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject UI;
    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        Instance = this;
    }

    public void AnimationComplete()
    {
        gameObject.SetActive(false);
        MainCamera.SetActive(true);
        UI.SetActive(true);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void AnimationComplete2()
    {
        gameObject.SetActive(false);

        MainCamera.SetActive(true);

        UI.SetActive(true);

        playerInput.SwitchCurrentActionMap("Player");
    }
}