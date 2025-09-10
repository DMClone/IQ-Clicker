using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private coroutine strainCoroutine;

    [System.Serializable]
    public class GameSettings
    {
        [Range(60, 120)] public int gameDuration = 90;
        [Range(0, 4)] public float goldenStrainDuration = 2f;
        [Range(0, 4)] public int goldenStrainAmount = 4;
        [Range(0, 4)] public int falseStrainAmount = 2;
        [Range(5, 40)] public int goldenStrainInterval = 20;
        [Range(10, 100)] public int goldenStrainReward = 50;
        [Range(10, 100)] public int falseStrainPenalty = 50;
        [Range(0, 5)][Tooltip("Amount of seconds randomly added or subtracted for a strain appearance")] private int strainRandom;
        public int strainsSpawned = 0;
    }
    public GameSettings gameSettings = new GameSettings();

    [System.Serializable]
    public class GameData
    {
        public int iq;
        public int goldenStrainsCollected;
        public int falseStrainsCollected;
    }
    [ReadOnly] public GameData gameData { get; private set; } = new GameData();

    public UnityEvent onGameRestart;


    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        // strainCoroutine = startcoroutine(SpawnStrain());
        // Additional start logic can be added here
    }

    public void FinishGame()
    {
        if (strainCoroutine != null)
        {
            stopcoroutine(strainCoroutine);
            strainCoroutine = null;
        }
        // Send data to database
    }

    public void RestartGame()
    {

        // Additional reset logic can be added here
        onGameRestart.Invoke();
        StartGame();
    }

    // private ienumerator SpawnStrain()
    // {
    //     if (!gameSettings.strainsSpawned >= gameSettings.goldenStrainAmount + gameSettings.falseStrainAmount)
    //         return null
    // }
}
