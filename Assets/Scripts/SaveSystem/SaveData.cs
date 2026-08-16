using System;

[Serializable]
public class SaveData
{
    // Player Progress
    public int day;
    public int money;
    public int health;
    public int reward;

    // Mission
    public string problem1;
    public string problem2;
    public string problem3;
    public string problem4;

    // Inventory
    public int[] inventory = new int[24];

    // Animation Bool
    public bool gameIntroPlayed;
    public bool bedIntroPlayed;
    public bool storeIntroPlayed;
    public bool repairTableIntroPlayed;

    public bool rewardGiven;
}