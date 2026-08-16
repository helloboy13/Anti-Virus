using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        continueButton.SetActive(SaveManager.Instance.HasSave());
    }
    public void continueGame()
    {
        SaveManager.Instance.LoadGame();

        SceneManager.LoadScene("Game");
    }
    public void newGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void exitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
