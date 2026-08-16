using UnityEngine;
using UnityEngine.InputSystem;

public class LooseWire : MonoBehaviour
{
    public InputActionReference interactInputAction;
    public GameManager gameManager;
    bool inRange = false;
    public bool done = false;
    public GameObject interactUI;
    public GameObject cable;
    void Update()
    {
        if (inRange && interactInputAction.action.WasPerformedThisFrame())
        {
            cable.SetActive(true);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.SpareCable]--;
            StrikeThrough.Instance.CompleteMission("Loose Wire");
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
