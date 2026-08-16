using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance;

    [SerializeField] private TextMeshProUGUI instructionTxt;
    [SerializeField] private AudioSource narratorAudioSource;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManager gameManager;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] introAudio;
    [SerializeField] private AudioClip[] fallAudio;
    [SerializeField] private AudioClip[] startAudio;
    [SerializeField] private AudioClip[] bedAudio;
    [SerializeField] private AudioClip[] storeAudio;
    [SerializeField] private AudioClip[] repairTableAudio;
    [SerializeField] private AudioClip[] lowHealthAudio;
    [SerializeField] private AudioClip[] deathAudio;
    [SerializeField] private AudioClip[] incompletePurchase;
    [SerializeField] private AudioClip[] notEnoughMoney;
    [SerializeField] private AudioClip[] losingAudio;
    [SerializeField] private AudioClip[] timeOutAudio;
    [SerializeField] private AudioClip[] winningAudio;

    private int messageId = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!gameManager.gm_storeIntroPlayed)
        {
            PlayIntroMessage();
            playerInput.SwitchCurrentActionMap("BlockInput");
        }
    }

    #region Messages

    private string[] intro =
    {
        "Hello. I'm Mickey. I'll help you complete challenges, fix computers, and occasionally explain why what you just did was a terrible idea."
    };

    private string[] fallMessages =
    {
        "Stay on the table. The viruses are inside the computer, not under it.",
        "Gravity is working perfectly. Thanks for dedicating your career to testing it.",
        "The client sent a computer, not a circus performer.",
        "The antivirus pill shrinks you. It doesn't lower your IQ. That part is natural.",
        "Remarkable. You've somehow become the problem you're supposed to fix.",
        "I had low expectations. Yet here we are.",
        "I can explain the objective again if you'd like. Slowly this time."
    };

    private string[] startMessages =
    {
        "The Workstation, Vending Machine, and Bed are all interactive. I figured I'd mention it before you spend ten minutes staring at a wall.",
        "Somewhere in this room is progress. Try the Workstation, Vending Machine, or Bed. I believe in you. Barely.",
        "I'd explain your job, but the Workstation, Vending Machine, and Bed can do that for me. Try not to disappoint them.",
        "Go press buttons on the Workstation, Vending Machine, or Bed. It's how professionals operate. Probably.",
        "Go interact with something. The Workstation, Vending Machine, or Bed would be ideal. The floor has already had enough attention.",
        "I was told to provide guidance. So here it is: Workstation, Vending Machine, Bed. The rest is apparently your responsibility.",
        "Amazing. You've already reached the tutorial and we're both still alive. Continue with the Workstation, Vending Machine, or Bed."
    };

    private string[] bedMessages =
    {
        "Already thinking about sleep? Impressive work ethic.",
        "Tired already? You've been employed for like five minutes.",
        "The bed lets you end the day. Not that you've earned it yet.",
        "Feeling exhausted? You haven't even fought a virus yet.",
        "The bed won't fix computers for you. I checked.",
        "Interesting choice. Most people start with the workstation.",
        "Remember: sleep is important. So is doing your job."
    };

    private string[] storeMessages =
    {
        "Everything you need is in here. Everything you can afford is a different question.",
        "Take your time. Poor financial decisions are permanent.",
        "Every item here has a purpose. Unlike some purchases you're about to make.",
        "Remember: buying everything isn't a strategy. It's a cry for help.",
        "You'll need supplies to survive. Judging by your performance so far, lots of supplies.",
        "Everything here is approved by professionals. You are not included in that statement.",
        "Take a look around. The machine can't fix incompetence, but it can sell accessories for it."
    };

    private string[] repairTableMessages =
    {
        "This is the Repair Table. The computer's problems are listed here, unlike yours.",
        "The Repair Table diagnoses the system. It can't diagnose whatever you're doing.",
        "Check the reported issues before buying supplies. I can't believe I have to say that.",
        "This table tells you what's wrong with the computer. The narrator handles what's wrong with you.",
        "The problems are right here. Solving them is the part I'm worried about.",
        "A good technician studies the problem first. A bad one learns by exploding things.",
        "The answers are literally on the table. This should be your easiest challenge today."
    };
    private string[] lowHealthMessages =
    {
        "Your health bar is starting to look as concerned as I am.",
        "At this rate, the computer might outlive you.",
        "Try not to die. The paperwork is annoying.",
        "The client paid for computer repair, not for your funeral.",
        "I would recommend not taking any more damage. Revolutionary advice, I know.",
        "Good news: you're still alive. Let's not push our luck.",
        "You have dangerously low health. I'm sure this is part of a plan."
    };
    private string[] deathMessages =
    {
        "Congratulations. You've successfully reduced your health to zero.",
        "You died exactly how you lived: concerningly.",
        "Good news: your mistakes can no longer get worse today.",
        "I leave you alone for five minutes...",
        "You died. The good news is that this is still going better than some technicians I've met.",
        "Cause of death: skill issue.",
        "Achievement Unlocked: Becoming The Problem."
    };
    private string[] incompletePurchaseMessages =
    {
        "You checked the problem list, looked at the store, and somehow decided that was enough. Bold strategy.",
        "I notice we're leaving without everything we need. I assume panic is part of the plan.",
        "The required items were highlighted for your convenience. Apparently convenience has limits.",
        "You missed a few required items. I admire the confidence, if not the competence.",
        "You bought exactly enough items to create new problems.",
        "The repair process is going remarkably well for someone ignoring half of it.",
        "The computer has unmet requirements. So do I."
    };
    private string[] notEnoughMoneyMessages =
    {
        "Interesting. You appear to be shopping with money you don't have.",
        "You don't have enough money. A detail that continues to surprise you.",
        "You can't afford that. Let's try living within the laws of mathematics.",
        "The machine accepts money. Confidence is not a valid currency.",
        "The numbers aren't adding up. Unlike your mistakes, which are multiplying.",
        "Not enough money. Have you tried being richer?",
        "Congratulations. You've discovered the store's most expensive feature: consequences."
    };
    private string[] losingMessages = 
    {
        "Congratulations. You've somehow lost a battle against basic budgeting.",
        "You spent money like future-you was someone else's problem.",
        "The repair was possible. Then you got involved.",
        "You didn't lose to the viruses. You lost to economics.",
        "You had enough money to win. Past tense is doing a lot of work there.",
        "The mission failed due to insufficient funds and excessive confidence.",
        "You've managed to bankrupt a computer repair shop. That's almost impressive."

    };
    private string[] timeOutMessages =
    {
        "Time's up... The system is now non-recoverable. The client is charging you a fine. Frankly, they're showing remarkable restraint.",
        "Time's up... The computer gave up waiting for you. Unfortunately, the client hasn't.",
        "The system is beyond recovery. The good news is you've learned an expensive lesson. The bad news is you're paying for it.",
        "Congratulations... You've managed to transform a paying customer into a refund request.",
        "The system is now non-recoverable. On the bright side, you've identified something else you can't fix.",
        "The computer died waiting for you to finish. That's not a figure of speech.",
        "Time's up... The client is charging you a fine. Considering the outcome, that's surprisingly polite."
    };
    private string[] winningMessages =
    {
        "You actually fixed it? I was already preparing my 'I told you so' speech.",
        "I hate admitting this... but that repair was almost impressive.",
        "You actually understood the assignment. That's suspicious.",
        "I'll update my notes: 'Player occasionally possesses a functioning brain.",
        "Well done.But, I still don't trust you.",
        "You've got talent. It's hiding really well, but it's there.",
        "I've reduced your insult quota for today... by one."
    };
    #endregion

    private float PlayMessage(string key, string[] messages, AudioClip[] audioClips)
    {
        if (messages == null || messages.Length == 0)
            return 0f;

        messageId++;
        int currentMessageId = messageId;

        int index = GetRandomIndex(key, messages.Length);

        instructionTxt.text = messages[index];

        StopAllCoroutines();

        float duration = 5f;

        if (audioClips != null &&
            index < audioClips.Length &&
            audioClips[index] != null)
        {
            if (narratorAudioSource.isPlaying)
                narratorAudioSource.Stop();

            narratorAudioSource.clip = audioClips[index];
            narratorAudioSource.Play();

            duration = audioClips[index].length;
        }

        StartCoroutine(ClearAfterDelay(duration, currentMessageId));

        return duration;
    }
    private IEnumerator ClearAfterDelay(float delay, int id)
    {
        yield return new WaitForSeconds(delay);

        if (id == messageId)
        {
            instructionTxt.text = "";
        }
    }
    private IEnumerator EnablePlayerAfterStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerInput.SwitchCurrentActionMap("Player");
    }

    public void PlayStartMessage()
    {
        float duration = PlayMessage("Start",startMessages, startAudio);

        StartCoroutine(EnablePlayerAfterStart(duration));
    }

    public void PlayFallMessage()
    {
        PlayMessage("Fall",fallMessages, fallAudio);
    }

    public void PlayBedMessage()
    {
        PlayMessage("Bed",bedMessages, bedAudio);
    }

    public void PlayStoreMessage()
    {
        PlayMessage("Store",storeMessages, storeAudio);
    }

    public void PlayRepairTableMessage()
    {
        PlayMessage("RepairTable",repairTableMessages, repairTableAudio);
    }

    public void PlayLowHealthMessage()
    {
        PlayMessage("LowHealth",lowHealthMessages, lowHealthAudio);
    }
    public void PlayIncompletePurchaseMessage()
    {
        PlayMessage("IncompletePurchase",incompletePurchaseMessages, incompletePurchase);
    }
    public void PlayNotEnoughMoneyMessage()
    {
        PlayMessage("NotEnoughMoney",notEnoughMoneyMessages, notEnoughMoney);
    }
    public void PlayDeathMessage()
    {
        PlayMessage("Death",deathMessages, deathAudio);
    }
    public void PlayLosingMessage()
    {
        PlayMessage("Losing",losingMessages, losingAudio);
    }
    public void PlayWinningMessage()
    {
        PlayMessage("Winning", winningMessages, winningAudio);
    }
    public void PlayTimeOutMessage()
    {
        PlayMessage("Timeout",timeOutMessages, timeOutAudio);
    }
    public void PlayIntroMessage()
    {
        float duration = PlayMessage("Intro",intro, introAudio);

        StartCoroutine(PlayStartAfterIntro(duration));
    }
    private IEnumerator PlayStartAfterIntro(float delay)
    {
        yield return new WaitForSeconds(delay + 0.25f);

        PlayStartMessage();
    }
    public void ClearMessage()
    {
        messageId++;

        if (narratorAudioSource.isPlaying)
            narratorAudioSource.Stop();

        instructionTxt.text = "";
    }
    private readonly Dictionary<string, Queue<int>> messageQueues = new();

    private int GetRandomIndex(string key, int length)
    {
        if (!messageQueues.TryGetValue(key, out Queue<int> queue))
        {
            queue = new Queue<int>();
            messageQueues[key] = queue;
        }

        if (queue.Count == 0)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < length; i++)
                indices.Add(i);

            // Fisher-Yates Shuffle
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (indices[i], indices[j]) = (indices[j], indices[i]);
            }

            foreach (int i in indices)
                queue.Enqueue(i);
        }

        return queue.Dequeue();
    }
}