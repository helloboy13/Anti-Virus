using UnityEngine;
using UnityEngine.InputSystem;

public class CircuitPath : MonoBehaviour
{
    public InputActionReference interactInputAction;
    public GameManager gameManager;
    bool inRange = false;
    public bool done = false;
    public GameObject interactUI;
    public GameObject bandage;
    void Update()
    {
        if (inRange && interactInputAction.action.WasPerformedThisFrame())
        {
            bandage.SetActive(true);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.CircuitPatch]--;
            StrikeThrough.Instance.CompleteMission("Cracked Circuit Path");
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
