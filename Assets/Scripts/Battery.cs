using UnityEngine;
using UnityEngine.InputSystem;

public class Battery : MonoBehaviour
{
    public InputActionReference interactInputAction;
    public GameManager gameManager;
    bool inRange = false;
    public bool done = false;
    public GameObject interactUI;
    public GameObject battery;
    void Update()
    {
        if(inRange && interactInputAction.action.WasPerformedThisFrame())
        {
            battery.SetActive(true);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.BatteryCell]--;
            StrikeThrough.Instance.CompleteMission("Dead Battery Cell");
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
