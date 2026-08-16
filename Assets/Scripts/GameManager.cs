using StarterAssets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int day = 0;
    public int money = 0;
    [SerializeField] private int maxHealth;
    public int currentHealth;
    [SerializeField] private InteractionSystem interactionSystem;

    

    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private TextMeshProUGUI moneyTxtMachine;
    [SerializeField] private TextMeshProUGUI dayTxt;
    [SerializeField] private TextMeshProUGUI rewardtxt;
    [SerializeField] private TextMeshProUGUI objProblem1Txt;
    [SerializeField] private TextMeshProUGUI objProblem2Txt;
    [SerializeField] private TextMeshProUGUI objProblem3Txt;
    [SerializeField] private TextMeshProUGUI objProblem4Txt;
    [SerializeField] private TextMeshProUGUI objMoneyTxt;
    [SerializeField] public TextMeshProUGUI instructionTxt;


    [SerializeField] public StarterAssetsInputs gamesInputs;


    [SerializeField] private TextMeshProUGUI item1Txt;
    [SerializeField] private TextMeshProUGUI item2Txt;
    [SerializeField] private TextMeshProUGUI item3Txt;
    [SerializeField] private TextMeshProUGUI item4Txt;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private Slider slider;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject losingScreeen;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject HealUI;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference healAction;
    private bool missionRewardGiven = false;

    private List<string> currentProblems = new List<string>();
    private int previousHealth;
    public int reward;
    private bool lowHealthTriggered = false;
    public bool isLosing = false;
    public bool haveEverything = false;
    public bool healUsable = false;


    public bool gm_gameIntroPlayed;
    public bool gm_bedIntroPlayed;
    public bool gm_storeIntroPlayed;
    public bool gm_repairTableIntroPlayed;
    public enum ItemType
    {
        // Special Items
        NPAVPill,
        SleepToken,
        MedKit,
        EnergyDrink,
        ExtraBattery,
        InsuranceChip,

        // Tools
        ReplacementFuse,
        CircuitPatch,
        CoolingGel,
        SpareCable,
        DataContainer,
        BatteryCell,
        VentCleaner,
        ConveyorWrench,
        USBCleaner,

        // Weapons
        AntivirusBat,
        DebuggerGun,
        BugSpray,
        PacketBlaster,
        VirusVacuum,
        QuarantineGun,
        FirewallLauncher,
        DeepScanRifle
    }

    [System.Serializable]
    public class ProblemObject
    {
        public string problemName;
        public GameObject problemObject;
    }

    [SerializeField] private List<ProblemObject> problemObjects;

    public Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>()
    {
        { ItemType.NPAVPill, 0 },
        { ItemType.SleepToken, 0 },
        { ItemType.MedKit, 0 },
        { ItemType.EnergyDrink, 0 },
        { ItemType.ExtraBattery, 0 },
        { ItemType.InsuranceChip, 0 },

        { ItemType.ReplacementFuse, 0 },
        { ItemType.CircuitPatch, 0 },
        { ItemType.CoolingGel, 0 },
        { ItemType.SpareCable, 0 },
        { ItemType.DataContainer, 0 },
        { ItemType.BatteryCell, 0 },
        { ItemType.VentCleaner, 0 },
        { ItemType.ConveyorWrench, 0 },
        { ItemType.USBCleaner, 0 },

        { ItemType.AntivirusBat, 0 },
        { ItemType.DebuggerGun, 0 },
        { ItemType.BugSpray, 0 },
        { ItemType.PacketBlaster, 0 },
        { ItemType.VirusVacuum, 0 },
        { ItemType.QuarantineGun, 0 },
        { ItemType.FirewallLauncher, 0 },
        { ItemType.DeepScanRifle, 0 }
    };


    private Dictionary<ItemType, int> itemPrices = new Dictionary<ItemType, int>()
{
    // Special Items
    { ItemType.NPAVPill, 100 },
    { ItemType.SleepToken, 250 },
    { ItemType.MedKit, 75 },
    { ItemType.EnergyDrink, 50 },
    { ItemType.ExtraBattery, 150 },
    { ItemType.InsuranceChip, 300 },

    // Tools
    { ItemType.ReplacementFuse, 60 },
    { ItemType.CircuitPatch, 120 },
    { ItemType.CoolingGel, 140 },
    { ItemType.SpareCable, 50 },
    { ItemType.DataContainer, 130 },
    { ItemType.BatteryCell, 180 },
    { ItemType.VentCleaner, 40 },
    { ItemType.ConveyorWrench, 170 },
    { ItemType.USBCleaner, 90 },

    // Weapons
    { ItemType.AntivirusBat, 0 },
    { ItemType.DebuggerGun, 250 },
    { ItemType.BugSpray, 120 },
    { ItemType.PacketBlaster, 500 },
    { ItemType.VirusVacuum, 600 },
    { ItemType.QuarantineGun, 700 },
    { ItemType.FirewallLauncher, 850 },
    { ItemType.DeepScanRifle, 1000 }
};

    private Dictionary<string, int> problemCosts = new Dictionary<string, int>()
{
    // Easy
    { "Loose Wire", 50 },            // Spare Cable
    { "Dust Nest", 40 },             // Vent Cleaner
    { "Broken Fuse", 60 },           // Replacement Fuse
    { "Byte Bugs", 0 },
    { "Cookie Thieves", 0 },

    // Medium
    { "Overheated Chip", 140 },      // Cooling Gel
    { "Leaking Data Packet", 130 },  // Data Container
    { "Cracked Circuit Path", 120 }, // Circuit Patch
    { "Blocked USB Tunnel", 90 },    // USB Cleaner
    { "Pop-Up Slimes", 0 },

    // Hard
    { "Dead Battery Cell", 180 },    // Battery Cell
    { "Stuck Data Conveyor", 170 },  // Conveyor Wrench
    { "Jammed Cooling Fan", 200 },   // Fan Repair Kit
    { "Ad Worms", 0 },
    { "Corrupted Pixel Swarm", 250 } // Pixel Cleaner
};

    private Dictionary<string, string> problemItems = new Dictionary<string, string>()
{
    { "Loose Wire", "Spare Cable" },
    { "Dust Nest", "Vent Cleaner" },
    { "Broken Fuse", "Replacement Fuse" },
    { "Byte Bugs", "Bug Spray" },
    { "Cookie Thieves", "Antivirus Bat" },

    { "Overheated Chip", "Cooling Gel" },
    { "Leaking Data Packet", "Data Container" },
    { "Cracked Circuit Path", "Circuit Patch" },
    { "Blocked USB Tunnel", "USB Cleaner" },
    { "Pop-Up Slimes", "Virus Vacuum" },

    { "Dead Battery Cell", "Battery Cell" },
    { "Stuck Data Conveyor", "Conveyor Wrench" },
    { "Jammed Cooling Fan", "Cooling Gel" },
    { "Ad Worms", "Debugger Gun" },
    { "Corrupted Pixel Swarm", "Firewall Launcher" }
};
    public void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasPendingLoad())
        {
            Load(SaveManager.Instance.GetLoadedData());
        }
        else
        {
            GenerateRandomProblems();
            reward = GetMissionReward();
            objMoneyTxt.text = "$" + reward;
        }

        playerInput.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Update()
    {
        CheckHaveEverything();

        moneyTxt.text = money.ToString() + "$";
        dayTxt.text = "Day : " + day.ToString();
        //Debug.Log(Mouse.current.position.ReadValue());
        moneyTxtMachine.text = moneyTxt.text;
        slider.value = (float)currentHealth / maxHealth;

        if (inventory[GameManager.ItemType.MedKit] > 0 && currentHealth < 90)
        {
            healUsable = true;
            HealUI.SetActive(true);
        }
        else
        {
            healUsable = false;
            HealUI.SetActive(false);
        }

        if (healAction.action.WasPerformedThisFrame() && inventory[ItemType.MedKit] > 0 && currentHealth < maxHealth && healUsable)
        {
            inventory[ItemType.MedKit]--;
            Heal(25);
        }

        if (isLosing)
        {
            losingScreeen.SetActive(true);
        }

        if (currentHealth <= 25)
        {
            // Health dropped while already under 25
            if (currentHealth != previousHealth)
            {
                Narrator.Instance.PlayLowHealthMessage();
            }

            lowHealthTriggered = true;
        }
        else
        {
            lowHealthTriggered = false;
        }

        previousHealth = currentHealth;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
    public void AddDay()
    {
        day++;
        currentHealth = 100;
        ResetObjective.Instance.resetObjective();
    }

    public void GenerateRandomProblems()
    {
        currentProblems.Clear();
        missionRewardGiven = false;
        List<string> availableProblems = new List<string>();

        if (day <= 5)
        {
            availableProblems.AddRange(new List<string>
        {
            "Loose Wire",
            "Dust Nest",
            "Broken Fuse",
            "Byte Bugs",
            "Cookie Thieves"
        });
        }
        else if (day <= 7)
        {
            availableProblems.AddRange(new List<string>
        {
            "Loose Wire",
            "Dust Nest",
            "Broken Fuse",
            "Byte Bugs",
            "Cookie Thieves",
            "Overheated Chip",
            "Leaking Data Packet",
            "Cracked Circuit Path",
            "Blocked USB Tunnel",
            "Pop-Up Slimes"
        });
        }
        else if (day <= 13)
        {
            availableProblems.AddRange(new List<string>
        {
            "Overheated Chip",
            "Leaking Data Packet",
            "Cracked Circuit Path",
            "Blocked USB Tunnel",
            "Pop-Up Slimes"
        });
        }
        else if (day <= 15)
        {
            availableProblems.AddRange(new List<string>
        {
            "Overheated Chip",
            "Leaking Data Packet",
            "Cracked Circuit Path",
            "Blocked USB Tunnel",
            "Pop-Up Slimes",
            "Dead Battery Cell",
            "Stuck Data Conveyor",
            "Jammed Cooling Fan",
            "Ad Worms",
            "Corrupted Pixel Swarm"
        });
        }
        else
        {
            availableProblems.AddRange(new List<string>
        {
            "Dead Battery Cell",
            "Stuck Data Conveyor",
            "Jammed Cooling Fan",
            "Ad Worms",
            "Corrupted Pixel Swarm"
        });
        }

        int index = Random.Range(0, availableProblems.Count);
        objProblem1Txt.text = availableProblems[index];
        currentProblems.Add(availableProblems[index]);
        availableProblems.RemoveAt(index);

        index = Random.Range(0, availableProblems.Count);
        objProblem2Txt.text = availableProblems[index];
        currentProblems.Add(availableProblems[index]);
        availableProblems.RemoveAt(index);

        index = Random.Range(0, availableProblems.Count);
        objProblem3Txt.text = availableProblems[index];
        currentProblems.Add(availableProblems[index]);
        availableProblems.RemoveAt(index);

        index = Random.Range(0, availableProblems.Count);
        objProblem4Txt.text = availableProblems[index];
        currentProblems.Add(availableProblems[index]);

        item1Txt.text = problemItems[objProblem1Txt.text];
        item2Txt.text = problemItems[objProblem2Txt.text];
        item3Txt.text = problemItems[objProblem3Txt.text];
        item4Txt.text = problemItems[objProblem4Txt.text];

        SetMissionTimer();

        UpdateProblemObjects();
    }

    public int CalculateMinimumMissionCost()
    {
        int total = 0;

        foreach (string problem in currentProblems)
        {
            total += problemCosts[problem];
        }

        return total;
    }

    public int GetRequiredMoneyForDay()
    {
        int missionCost = CalculateMinimumMissionCost();

        int essentials =
            itemPrices[ItemType.NPAVPill] +
            itemPrices[ItemType.SleepToken];

        return missionCost + essentials;
    }

    public void CheckLoseCondition()
    {
        if (!CanStillCompleteMission())
        {
            Narrator.Instance.PlayLosingMessage();
            playerInput.SwitchCurrentActionMap("BlockInput");
            Invoke(nameof(ShowLoseScreen), 0.1f);
        }
    }
    private void ShowLoseScreen()
    {
        isLosing = true;
        Time.timeScale = 0f;
    }
    public int GetMissionReward()
    {
        int missionCost = GetRequiredMoneyForDay();

        int minReward;
        int maxReward;

        if (day <= 5) // Easy
        {
            minReward = Mathf.RoundToInt(missionCost * 1.25f); // 25% profit
            maxReward = Mathf.RoundToInt(missionCost * 1.75f); // 75% profit
        }
        else if (day <= 13) // Medium
        {
            minReward = Mathf.RoundToInt(missionCost * 1.5f); // 50% profit
            maxReward = Mathf.RoundToInt(missionCost * 2.0f); // 100% profit
        }
        else // Hard
        {
            minReward = Mathf.RoundToInt(missionCost * 1.75f); // 75% profit
            maxReward = Mathf.RoundToInt(missionCost * 2.25f); // 125% profit
        }

        return Random.Range(minReward, maxReward + 1);
    }

    public void dayUpdate()
    {
        GenerateRandomProblems();
        reward = GetMissionReward();
        objMoneyTxt.text = "$" + reward;
    }

    public void AddItem(int itemId)
    {
        ItemType item = (ItemType)itemId;

        if (money >= itemPrices[item])
        {
            inventory[item]++;
            money -= itemPrices[item];

            priceText.text = "-" + itemPrices[item] + "$";
            CancelInvoke(nameof(ClearPrice));
            Invoke(nameof(ClearPrice), 3f);
            CheckLoseCondition();
        }
        else
        {
            Narrator.Instance.PlayNotEnoughMoneyMessage();
            CancelInvoke(nameof(ClearInstruction));
            Invoke(nameof(ClearInstruction), 2f);
        }
    }

    private void ClearInstruction()
    {
        instructionTxt.text = "";
    }
    private void ClearPrice()
    {
        priceText.text = "";
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Damage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player Died");       
    }

    public int GetItemQuantity(int itemId)
    {
        ItemType item = (ItemType)itemId;

        if (inventory.ContainsKey(item))
            return inventory[item];

        return 0;
    }
    public void CheckHaveEverything()
    {
        haveEverything = true;

        // Mandatory items
        if (inventory[ItemType.NPAVPill] < 1)
        {
            haveEverything = false;
            return;
        }

        string[] requiredItems =
        {
        item1Txt.text,
        item2Txt.text,
        item3Txt.text,
        item4Txt.text
    };

        foreach (string itemName in requiredItems)
        {
            ItemType item;

            if (System.Enum.TryParse(itemName.Replace(" ", ""), out item))
            {
                if (inventory[item] < 1)
                {
                    haveEverything = false;
                    return;
                }
            }
        }

        haveEverything = true;
    }
    public bool CanStillCompleteMission()
    {
        int requiredCost = 0;

        // Check mission items
        string[] requiredItems =
        {
        item1Txt.text,
        item2Txt.text,
        item3Txt.text,
        item4Txt.text
    };

        foreach (string itemName in requiredItems)
        {
            ItemType item;

            if (System.Enum.TryParse(itemName.Replace(" ", ""), out item))
            {
                if (inventory[item] < 1)
                {
                    requiredCost += itemPrices[item];
                }
            }
        }

        // Check mandatory items
        if (inventory[ItemType.NPAVPill] < 1)
            requiredCost += itemPrices[ItemType.NPAVPill];


        return money >= requiredCost;
    }

    public void Save(SaveData data)
    {
        data.day = day;
        data.money = money;
        data.health = currentHealth;
        data.reward = reward;

        int index = 0;

        foreach (ItemType item in System.Enum.GetValues(typeof(ItemType)))
        {
            data.inventory[index] = inventory[item];
            index++;
        }

        data.problem1 = objProblem1Txt.text.Replace("<s>", "").Replace("</s>", "");
        data.problem2 = objProblem2Txt.text.Replace("<s>", "").Replace("</s>", "");
        data.problem3 = objProblem3Txt.text.Replace("<s>", "").Replace("</s>", "");
        data.problem4 = objProblem4Txt.text.Replace("<s>", "").Replace("</s>", "");

        data.gameIntroPlayed = true;
        data.bedIntroPlayed = true;
        data.repairTableIntroPlayed = true;
        data.storeIntroPlayed = true;

        data.rewardGiven = missionRewardGiven;
    }

    public void Load(SaveData data)
    {
        Debug.Log("LOAD CALLED");

        day = data.day;
        money = data.money;
        currentHealth = data.health;
        reward = data.reward;

        objMoneyTxt.text = "$" + reward;

        int index = 0;

        foreach (ItemType item in System.Enum.GetValues(typeof(ItemType)))
        {
            inventory[item] = data.inventory[index];
            index++;
        }

        objProblem1Txt.text = data.problem1;
        objProblem2Txt.text = data.problem2;
        objProblem3Txt.text = data.problem3;
        objProblem4Txt.text = data.problem4;

        Debug.Log(objProblem1Txt.text);
        Debug.Log(objProblem2Txt.text);
        Debug.Log(objProblem3Txt.text);
        Debug.Log(objProblem4Txt.text);

        item1Txt.text = problemItems[objProblem1Txt.text];
        item2Txt.text = problemItems[objProblem2Txt.text];
        item3Txt.text = problemItems[objProblem3Txt.text];
        item4Txt.text = problemItems[objProblem4Txt.text];

        gm_bedIntroPlayed = data.bedIntroPlayed;
        gm_gameIntroPlayed = data.gameIntroPlayed;
        gm_repairTableIntroPlayed = data.repairTableIntroPlayed;
        gm_storeIntroPlayed = data.storeIntroPlayed;

        missionRewardGiven = data.rewardGiven;

        interactionSystem.RefreshAfterLoad();
        ResetObjective.Instance.resetObjective();
        UpdateProblemObjects();
        SetMissionTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateProblemObjects()
    {
        // Disable everything first
        foreach (ProblemObject problem in problemObjects)
        {
            if (problem.problemObject != null)
                problem.problemObject.SetActive(false);
        }

        // Enable only today's problems
        foreach (ProblemObject problem in problemObjects)
        {
            if (problem.problemName == objProblem1Txt.text ||
                problem.problemName == objProblem2Txt.text ||
                problem.problemName == objProblem3Txt.text ||
                problem.problemName == objProblem4Txt.text)
            {
                if (problem.problemObject != null)
                    problem.problemObject.SetActive(true);
            }
        }
    }

    public void CheckMissionCompletedAfterMission()
    {
        if (missionRewardGiven)
            return;

        if (StrikeThrough.Instance.IsCompleted(objProblem1Txt) &&
            StrikeThrough.Instance.IsCompleted(objProblem2Txt) &&
            StrikeThrough.Instance.IsCompleted(objProblem3Txt) &&
            StrikeThrough.Instance.IsCompleted(objProblem4Txt))
        {
            missionRewardGiven = true;

            money += reward;

            rewardtxt.text = "+" + reward + "$";

            CancelInvoke(nameof(ClearRewardInstruction));
            Invoke(nameof(ClearRewardInstruction), 3f);

            Narrator.Instance.PlayWinningMessage();

            Timer.Instance.StopTimer();

            TeleportReset.Instance.TeleportToMin();

            gamesInputs.staminaDrainRate = 30;
            gamesInputs.staminaRegenRate = 15;
        }
    }

    private void ClearRewardInstruction()
    {
        rewardtxt.text = "";
    }

    public void SetMissionTimer()
    {
        if (day <= 5)
        {
            Timer.Instance.SetTimer(2, 0);
        }
        else if (day <= 7)
        {
            Timer.Instance.SetTimer(2, 30);
        }
        else if (day <= 13)
        {
            Timer.Instance.SetTimer(3, 0);
        }
        else if (day <= 15)
        {
            Timer.Instance.SetTimer(3, 30);
        }
        else
        {
            Timer.Instance.SetTimer(4, 0);
        }
    }
}
