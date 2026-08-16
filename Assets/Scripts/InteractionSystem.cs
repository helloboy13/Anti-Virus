using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private StarterAssetsInputs gamesInputs;
    [SerializeField] private InputActionReference interactionActionReference;
    [SerializeField] private InputActionReference exitActionReference;
    [SerializeField] private InputActionReference resetActionReference;
    public Animator animator;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private StarterAssetsInputs gameInputs;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject storeUI;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private Animator TableAnimator;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject taskUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject eatPill;

    private DepthOfField depthOfField;

    private bool isBed = false;
    private bool isMachine = false;
    private bool isSetup = false;
    public static bool inStore = false;
    private bool canTakeKillZoneDamage = true;

    private bool tableDone = false;
    private bool storeDone = false;
    private bool bedDone = false;
    
    private bool pillTake = false;
    public int time;

    private void Awake()
    {
        globalVolume.profile.TryGet(out depthOfField);
    }
    private void Start()
    {
    }
    public void Update()
    {

        if (gameManager.currentHealth == 0)
        {
            animator.SetTrigger("Transition");
            Narrator.Instance.PlayDeathMessage();
            //TeleportReset.Instance.TeleportToDead();
            Invoke(nameof(AddDayDelayed), 0.8f);
            Invoke(nameof(TeleportDead) , 0.5f);
            gameManager.currentHealth = 100;
            gamesInputs.staminaDrainRate = 30;
            gamesInputs.staminaRegenRate = 15;
        }
        if(interactionActionReference.action.WasPressedThisFrame() && isBed && !bedDone)
        {
            Camera.SetActive(true);
            MainCamera.SetActive(false);
            GameUI.SetActive(false);
            TableAnimator.SetTrigger("Bed");
            playerInput.SwitchCurrentActionMap("BlockInput");
            Narrator.Instance.PlayBedMessage();
            Invoke(nameof(BedIntroDone), 1f);
        }
        if (interactionActionReference.action.WasPressedThisFrame() && isBed && bedDone)
        {
            
            if (gameManager.inventory[GameManager.ItemType.SleepToken] > 0)
            {
                animator.SetTrigger("Transition");
                gameManager.inventory[GameManager.ItemType.SleepToken]--;
                Invoke(nameof(AddDayDelayed), 0.8f);
            }
            else
            { 
                gameManager.instructionTxt.text = "You need sleeping pill to Skip a Day!";
                Invoke(nameof(ClearInstruction), 2.25f);
            }
            
        }
        if (isMachine && interactionActionReference.action.WasPressedThisFrame() && !storeDone)
        {
            Camera.SetActive(true);
            MainCamera.SetActive(false);
            GameUI.SetActive(false);
            TableAnimator.SetTrigger("Store");
            playerInput.SwitchCurrentActionMap("BlockInput");
            Narrator.Instance.PlayStoreMessage();


            Invoke(nameof(StoreIntroDone), 1f);


        }
        if (interactionActionReference.action.WasPressedThisFrame() && isMachine && storeDone)
        {
            EnableBlur();
            GameUI.SetActive(false);
            storeUI.SetActive(true);
            inventoryUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstButton);
            playerInput.SwitchCurrentActionMap("UI");
            inStore = true;

            gameInputs.cursorLocked = false;
            gameInputs.cursorInputForLook = false;

            GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
            GetComponent<StarterAssetsInputs>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        if (inStore && exitActionReference.action.WasPressedThisFrame() && storeDone)
        {
            
            DisableBlur();
            GameUI.SetActive(true);
            playerInput.SwitchCurrentActionMap("Player");
            inStore = false;
            storeUI.SetActive(false);

            gameInputs.cursorLocked = true;
            gameInputs.cursorInputForLook = true;

            GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
            GetComponent<StarterAssetsInputs>().enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (!gameManager.haveEverything)
            {
                Narrator.Instance.PlayIncompletePurchaseMessage();
            }
        }
        if ( isSetup && interactionActionReference.action.WasPressedThisFrame() && !tableDone)
        {
            Camera.SetActive(true);
            MainCamera.SetActive(false);
            GameUI.SetActive(false);
            TableAnimator.SetTrigger("Table");
            playerInput.SwitchCurrentActionMap("BlockInput");
            Narrator.Instance.PlayRepairTableMessage();
            taskUI.SetActive(true);

            Invoke(nameof(TableDoneIntro), 1f);
        }
        //if (tableDone && interactionActionReference.action.WasPressedThisFrame() && isSetup)
        //{
        //    taskUI.SetActive(true);
        //    pillTake = true;
        //}

        if (tableDone && interactionActionReference.action.WasPressedThisFrame() && isSetup)
        {
            taskUI.SetActive(true);
            pillTake = true;

            interactUI.SetActive(false);
            eatPill.SetActive(gameManager.haveEverything);
        }
        if (pillTake && gameManager.haveEverything)
        {
            eatPill.SetActive(isSetup);

            if (resetActionReference.action.WasPressedThisFrame() && isSetup)
            {
                playerAnimator.SetTrigger("Drinking");
                gameManager.inventory[GameManager.ItemType.NPAVPill]--;

                if (gameManager.inventory[GameManager.ItemType.EnergyDrink] > 0)
                {
                    gameManager.inventory[GameManager.ItemType.EnergyDrink]--;
                    gameInputs.staminaRegenRate = 20f;
                    gameInputs.staminaDrainRate = 10f;
                }
                if (gameManager.inventory[GameManager.ItemType.ExtraBattery] > 0)
                {
                    gameManager.inventory[GameManager.ItemType.ExtraBattery]--;
                    time = 30;
                }
                else
                {
                    time = 0;
                }
            }
        }
        else
        {
            eatPill.SetActive(false);
        }
    }


private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Setup")
        {
            isSetup = true;

            if (pillTake && gameManager.haveEverything)
            {
                eatPill.SetActive(true);
                interactUI.SetActive(false);
            }
            else
            {
                interactUI.SetActive(true);
                eatPill.SetActive(false);
            }
        }
        if (other.gameObject.tag == "Machine")
        {
            isMachine = true;
            interactUI.SetActive(true);
        }

        if (other.CompareTag("KillZone") && canTakeKillZoneDamage)
        {
            canTakeKillZoneDamage = false;
            Timer.Instance.Minus10Seconds();
            gameManager.TakeDamage(15);
            TeleportReset.Instance.TeleportToMax();

            Invoke(nameof(ResetKillZoneDamage), 1f);

            Narrator.Instance.PlayFallMessage();

        }
        if (other.gameObject.tag == "Bed")
        {
            isBed = true;
            interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Machine")
        {
            isMachine = false;

            if (!isBed && !isSetup)
                interactUI.SetActive(false);
        }

        if (other.gameObject.tag == "Bed")
        {
            isBed = false;

            if (!isMachine && !isSetup)
                interactUI.SetActive(false);
        }

        if (other.gameObject.tag == "Setup")
        {
            isSetup = false;

            eatPill.SetActive(false);

            if (!isMachine && !isBed)
                interactUI.SetActive(false);

            Camera.SetActive(false);
        }
    }
    private void TeleportDead()
    {
        TeleportReset.Instance.TeleportToDead();
    }
    public void AddDayDelayed()
    {
        pillTake = false;
        taskUI.SetActive(false);
        gameManager.AddDay();
        gameManager.dayUpdate();
        SaveManager.Instance.SaveGame();
    }
    public void ClearInstruction()
    {
        gameManager.instructionTxt.text = "";
    }

    public void EnableBlur()
    {
        depthOfField.active = true;
    }

    public void DisableBlur()
    {
        depthOfField.active = false;
    }

    private void ResetKillZoneDamage()
    {
        canTakeKillZoneDamage = true;
    }
    private void StoreIntroDone()
    {
        storeDone = true;
    }
    private void BedIntroDone()
    {
        bedDone = true;
    }
    private void TableDoneIntro()
    {
        tableDone = true;
    }
    public void inputon()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }
    public void inputoff()
    {
        playerInput.SwitchCurrentActionMap("BlockInput");
    }

    public void RefreshAfterLoad()
    {
        Debug.Log("Runned succesfully");
        tableDone = gameManager.gm_repairTableIntroPlayed;
        storeDone = gameManager.gm_storeIntroPlayed;
        bedDone = gameManager.gm_bedIntroPlayed;

        TableAnimator.SetBool("Intro", gameManager.gm_gameIntroPlayed);

        if (gameManager.gm_gameIntroPlayed)
        {
            AnimationComeplete.Instance.AnimationComplete2();
            inventoryUI.SetActive(true);
        }
    }
}
