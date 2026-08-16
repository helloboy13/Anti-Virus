using UnityEngine;

public class TeleportReset : MonoBehaviour
{
    public static TeleportReset Instance;

    public GameObject Max;
    public GameObject Min;
    public GameObject Dead;

    [SerializeField] private CharacterController cc;

    private void Awake()
    {
        Instance = this;
    }

    public void TeleportToMax()
    {
        cc.enabled = false;
        transform.position = Max.transform.position;
        cc.enabled = true;
    }

    public void TeleportToMin()
    {
        cc.enabled = false;
        transform.position = Min.transform.position;
        cc.enabled = true;
    }
    public void TeleportToDead()
    {
        cc.enabled = false;
        transform.position = Dead.transform.position;
        cc.enabled=true;
    }
}