using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Transform brainPos;
    private Coroutine strainIntervalCoroutine;
    private Coroutine strainCoroutine;

    [System.Serializable]
    public struct GameSettings
    {
        [Range(60, 120)] public int gameDuration;
        [Range(0, 4)] public float goldenStrainDuration;
        [Range(0, 4)] public int goldenStrainAmount;
        [Range(0, 4)] public int falseStrainAmount;
        [Range(2, 40)] public int strainInterval;
        [Range(10, 100)] public int goldenStrainReward;
        [Range(10, 100)] public int falseStrainPenalty;
        [Range(0, 5)][Tooltip("Amount of seconds randomly added for a strain appearance")] public int strainRandom;
        [ReadOnly] public int goldenstrainsSpawned;
        [ReadOnly] public int falseStrainsSpawned;

        public GameObject[] strains;
    }
    public GameSettings gameSettings = new GameSettings();

    [System.Serializable]
    public struct GameData
    {
        public int iq;
        public int goldenStrainsCollected;
        public int falseStrainsCollected;
    }
    [ReadOnly] public GameData gameData = new GameData();

    public UnityEvent onGameRestart;


    void Awake()
    {
        Instance = this;
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(SpawnStrainInterval());
    }

    public void FinishGame()
    {
        StopAllCoroutines();

        // Send data to database
    }

    public void RestartGame()
    {
        onGameRestart.Invoke();
        StartGame();
    }

    private IEnumerator SpawnStrainInterval()
    {
        if (gameSettings.goldenstrainsSpawned + gameSettings.falseStrainsSpawned < gameSettings.goldenStrainAmount + gameSettings.falseStrainAmount)
            StartCoroutine(SpawnStrain());
        else
            yield break;

        yield return new WaitForSeconds(gameSettings.strainInterval);
        StartCoroutine(SpawnStrainInterval());
    }

    private IEnumerator SpawnStrain()
    {
        float randomTime = Random.Range(0, gameSettings.strainRandom);

        bool strainIsGolden;

        if (gameSettings.goldenstrainsSpawned == gameSettings.goldenStrainAmount)
            strainIsGolden = false;
        else if (gameSettings.falseStrainsSpawned == gameSettings.falseStrainAmount)
            strainIsGolden = true;
        else
            strainIsGolden = (Random.Range(0, 2) == 0);

        switch (strainIsGolden)
        {
            case true:
                gameSettings.goldenstrainsSpawned++;
                break;
            case false:
                gameSettings.falseStrainsSpawned++;
                break;
        }

        Vector2 spawnPosition = Random.insideUnitCircle.normalized * 300;

        yield return new WaitForSeconds(randomTime);

        GameObject strain = null;

        for (int i = 0; i < gameSettings.strains.Length; i++)
        {
            if (!gameSettings.strains[i].activeSelf)
            {
                strain = gameSettings.strains[i];
                strain.transform.localPosition = spawnPosition;
                break;
            }
        }
        if (strain == null)
        {
            Debug.LogWarning("No available strain to spawn.");
            yield break;
        }

        switch (strainIsGolden)
        {
            case true:
                strain.GetComponent<IQStrain>().strainSettings.isGolden = true;
                strain.SetActive(true);

                Debug.Log("Spawn Golden Strain");
                break;
            case false:
                strain.GetComponent<IQStrain>().strainSettings.isGolden = false;
                strain.SetActive(true);
                Debug.Log("Spawn False Strain");
                break;
        }
    }

    public void CollectStrain(bool isGolden)
    {
        switch (isGolden)
        {
            case true:
                gameData.iq += gameSettings.goldenStrainReward;
                gameData.goldenStrainsCollected++;
                break;
            case false:
                gameData.iq -= gameSettings.falseStrainPenalty;
                gameData.falseStrainsCollected++;
                break;
        }
    }
}
