using TMPro;
using UnityEngine;

public class StrikeThrough : MonoBehaviour
{
    public static StrikeThrough Instance;

    [SerializeField] private TextMeshProUGUI objProblem1Txt;
    [SerializeField] private TextMeshProUGUI objProblem2Txt;
    [SerializeField] private TextMeshProUGUI objProblem3Txt;
    [SerializeField] private TextMeshProUGUI objProblem4Txt;

    private void Awake()
    {
        Instance = this;
    }

    public void CompleteMission(string missionName)
    {
        Strike(objProblem1Txt, missionName);
        Strike(objProblem2Txt, missionName);
        Strike(objProblem3Txt, missionName);
        Strike(objProblem4Txt, missionName);
    }

    private void Strike(TextMeshProUGUI missionText, string missionName)
    {
        if (missionText == null)
            return;

        string currentText = missionText.text
            .Replace("<s>", "")
            .Replace("</s>", "");

        if (currentText == missionName)
        {
            missionText.text = $"<s>{currentText}</s>";
        }
    }

    public void ResetMission(TextMeshProUGUI missionText)
    {
        if (missionText == null)
            return;

        missionText.text = missionText.text
            .Replace("<s>", "")
            .Replace("</s>", "");
    }

    public bool IsCompleted(TextMeshProUGUI missionText)
    {
        if (missionText == null)
            return false;

        return missionText.text.Contains("<s>");
    }
}