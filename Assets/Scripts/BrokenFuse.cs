using UnityEngine;
using UnityEngine.InputSystem;

public class BrokenFuse : MonoBehaviour
{
    public InputActionReference interactionActionReference;
    public GameManager gameManager;
    public GameObject fuseObject;
    public bool done = false;
    public GameObject interactUI;
    bool inRange = false;
    void Update()
    {
        if(inRange && interactionActionReference.action.WasPerformedThisFrame())
        {
            fuseObject.SetActive(true);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.ReplacementFuse]--;
            StrikeThrough.Instance.CompleteMission("Broken Fuse");
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
