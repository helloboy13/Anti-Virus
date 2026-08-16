using UnityEngine;

public class ResetObjective : MonoBehaviour
{
    public static ResetObjective Instance;

    public GameObject BlockedUSB_Dust;
    public GameObject BrokenFuse_Fuse;
    public GameObject CrackedCiruit_Patch;
    public GameObject DeadBattery_Battery;
    public GameObject DustNest_Dust;
    public GameObject LooseWire_Cable;

    public DustNest dustNest;
    public Battery battery;
    public CircuitPath circuitPath;
    public BrokenFuse brokenFuse;
    public USBTunnel usbTunnel;
    public LooseWire looseWire;
    public ByteBugs byteBugs;
    public CookieThieves cookieThieves;

    private void Awake()
    {
        Instance = this;
    }

    public void resetObjective()
    {
        BlockedUSB_Dust.SetActive(true);
        BrokenFuse_Fuse.SetActive(false);
        CrackedCiruit_Patch.SetActive(false);
        DeadBattery_Battery.SetActive(false);
        DustNest_Dust.SetActive(true);
        LooseWire_Cable.SetActive(false);

        dustNest.done = false;
        battery.done = false;
        circuitPath.done = false;
        brokenFuse.done = false;
        usbTunnel.done = false;
        looseWire.done = false;
        byteBugs.done = false;
        cookieThieves.done = false;
    }
}
