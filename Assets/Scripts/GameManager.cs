using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform brainPos;
    public GameObject[] strains;
    public TMPro.TextMeshProUGUI iqText;
    public Timer timer;

    public GameSettings gameSettings;
    public GameData gameData;

    void Awake()
    {
        Instance = this;
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(SpawnStrainInterval());
        timer.BeginTimer();
    }

    public void FinishGame()
    {
        StopAllCoroutines();
        // Send data to database
        ResetVariables();
    }

    public void RestartGame()
    {
        StartGame();
    }

    private IEnumerator SpawnStrainInterval()
    {
        yield return new WaitForSeconds(gameSettings.strainInterval);
        if (gameSettings.goldenstrainsSpawned + gameSettings.falseStrainsSpawned < gameSettings.goldenStrainAmount + gameSettings.falseStrainAmount)
            StartCoroutine(SpawnStrain());
        else
            yield break;

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

        Vector2 spawnPosition = new Vector2(brainPos.transform.position.x, brainPos.transform.position.y) + (Random.insideUnitCircle.normalized * gameSettings.strainSpawnRadius);

        yield return new WaitForSeconds(randomTime);

        GameObject strain = null;

        for (int i = 0; i < strains.Length; i++)
        {
            if (!strains[i].activeSelf)
            {
                strain = strains[i];
                strain.transform.position = spawnPosition;
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
        UpdateUI();
    }

    public void OnBrainClick()
    {
        gameData.iq += 1;
        gameData.brainClicks++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        iqText.text = "IQ: " + gameData.iq.ToString();
    }

    private void ResetVariables()
    {
        gameSettings = ScriptableObject.CreateInstance<GameSettings>();
        gameData = ScriptableObject.CreateInstance<GameData>();
        UpdateUI();
    }
}
