using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public GameObject Camera;
    public void drinking()
    {
        TeleportReset.Instance.TeleportToMax();
        Timer.Instance.StartTimer();
    }
    public void sleeping()
    {
        Camera.SetActive(false);
    }
}
