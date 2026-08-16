using UnityEngine;
using UnityEngine.InputSystem;

public class DustNest : MonoBehaviour
{
    public InputActionReference interactInputAction;
    public GameManager gameManager;
    bool inRange = false;
    public bool done = false;
    public GameObject interactUI;
    public GameObject dust;
    void Update()
    {
        if(inRange && interactInputAction.action.WasPerformedThisFrame())
        {
            dust.SetActive(false);
            done = true;
            interactUI.SetActive(false);
            gameManager.inventory[GameManager.ItemType.VentCleaner]--;
            StrikeThrough.Instance.CompleteMission("Dust Nest");
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
