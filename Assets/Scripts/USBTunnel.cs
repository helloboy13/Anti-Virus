using UnityEngine;
using UnityEngine.InputSystem;

public class USBTunnel : MonoBehaviour
{
    bool inRange = false;
    public bool done = false;
    public InputActionReference interactionActionReference;
    public GameManager gameManager;
    public GameObject interactUI;
    public GameObject dust;
    void Update()
    {
        if (inRange && interactionActionReference.action.WasPerformedThisFrame()) 
        {
            dust.SetActive(false);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.USBCleaner]--;
            StrikeThrough.Instance.CompleteMission("Blocked USB Tunnel");
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
