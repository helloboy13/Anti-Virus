using UnityEngine;
using UnityEngine.InputSystem;

public class CookieThieves : MonoBehaviour
{
    public InputActionReference interactInputAction;
    public GameManager gameManager;
    bool inRange = false;
    public bool done = false;
    public GameObject interactUI;
    void Update()
    {
        if (inRange && interactInputAction.action.WasPerformedThisFrame())
        {
            done = true;
            interactUI.SetActive(false);
            StrikeThrough.Instance.CompleteMission("Cookie Thieves");
            gameManager.CheckMissionCompletedAfterMission();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!done)
        {
            inRange = true;
            interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
        interactUI.SetActive(false);
    }
}
