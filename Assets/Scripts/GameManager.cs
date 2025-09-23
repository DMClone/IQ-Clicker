using System.Collections;
using System.Dynamic;
using System.Linq;
using JetBrains.Annotations;
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
    public Scientist scientist;

    [HideInInspector] public GameSettings gameSettings;
    [HideInInspector] public GameData gameData;

    [SerializeField] private GameSettings defaultSettings;
    [SerializeField] private GameData defaultData;

    void Awake()
    {
        Instance = this;
        ResetVariables();
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(SpawnStrainInterval());
        StartCoroutine(ScientistInterval());
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
        if (!gameSettings.strainOnClick && gameSettings.goldenstrainsSpawned + gameSettings.falseStrainsSpawned < gameSettings.goldenStrainAmount + gameSettings.falseStrainAmount)
            StartCoroutine(SpawnStrain(false));
        else
            yield break;

        StartCoroutine(SpawnStrainInterval());
    }

    private IEnumerator SpawnStrain(bool forceGolden)
    {
        float randomTime = 0;
        if (!forceGolden)
            randomTime = Random.Range(0, gameSettings.strainRandom);
        bool strainIsGolden;

        if (forceGolden)
            strainIsGolden = true;
        else if (gameSettings.goldenstrainsSpawned == gameSettings.goldenStrainAmount)
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

        if (!forceGolden) yield return new WaitForSeconds(randomTime);

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
                break;
            case false:
                strain.GetComponent<IQStrain>().strainSettings.isGolden = false;
                strain.SetActive(true);
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
        int iqGained = gameSettings.iqPerClick;
        if (gameSettings.iqMultiplierPerGoldenStrain > 1)
        {
            iqGained = Mathf.RoundToInt(iqGained * (gameSettings.iqMultiplierPerGoldenStrain * gameData.goldenStrainsCollected));
            Debug.Log("Click IQ gain multiplied to: " + iqGained);
        }
        gameData.iq += iqGained;
        if (gameSettings.strainOnClick)
            StartCoroutine(SpawnStrain(true));
        gameData.brainClicks++;
        UpdateUI();
    }

    private IEnumerator ScientistInterval()
    {
        if (gameSettings.scientistRound >= gameSettings.perkList.Length) yield break;
        yield return new WaitForSeconds(gameSettings.scientistInterval);
        scientist.transform.parent.gameObject.SetActive(true);


        StartCoroutine(ScientistInterval());
        // To be implemented
    }

    public Perk[] GetPerks()
    {
        return gameSettings.perkList[gameSettings.scientistRound].perks;
    }

    public void AddPerk(int perkIndex)
    {
        Perk perk = gameSettings.perkList[gameSettings.scientistRound].perks[perkIndex];
        gameSettings.scientistRound++;
        gameData.perks.Add(perk);
        for (int i = 0; i < perk.modifiers.Length; i++)
        {
            StatModifier modifier = perk.modifiers[i];
            switch (modifier.statType)
            {
                case StatType.IQPerClick:
                    if (modifier.isMultiplier)
                        gameSettings.iqPerClick = Mathf.RoundToInt(gameSettings.iqPerClick * modifier.value);
                    else
                        gameSettings.iqPerClick = Mathf.RoundToInt(gameSettings.iqPerClick + modifier.value);
                    break;
                case StatType.StrainDuration:
                    if (modifier.isMultiplier)
                        gameSettings.strainDuration *= modifier.value;
                    else
                        gameSettings.strainDuration += modifier.value;
                    break;
                case StatType.StrainInterval:
                    if (modifier.isMultiplier)
                        gameSettings.strainInterval = Mathf.RoundToInt(gameSettings.strainInterval * modifier.value);
                    else
                        gameSettings.strainInterval = Mathf.RoundToInt(gameSettings.strainInterval + modifier.value);
                    break;
                case StatType.StrainAmount:
                    if (modifier.isMultiplier)
                    {
                        gameSettings.goldenStrainAmount = Mathf.RoundToInt(gameSettings.goldenStrainAmount * modifier.value);
                        gameSettings.falseStrainAmount = Mathf.RoundToInt(gameSettings.falseStrainAmount * modifier.value);
                    }
                    else
                    {
                        gameSettings.goldenStrainAmount = Mathf.RoundToInt(gameSettings.goldenStrainAmount + modifier.value);
                        gameSettings.falseStrainAmount = Mathf.RoundToInt(gameSettings.falseStrainAmount + modifier.value);
                    }
                    break;
                case StatType.StrainOnClick:
                    ToggleStrainOnClick();
                    break;
                case StatType.IQMultiplierPerGoldenStrain:
                    if (modifier.isMultiplier)
                        gameSettings.iqMultiplierPerGoldenStrain = gameSettings.iqMultiplierPerGoldenStrain * modifier.value;
                    else
                        gameSettings.iqMultiplierPerGoldenStrain = gameSettings.iqMultiplierPerGoldenStrain + modifier.value;
                    break;
            }
        }
    }

    private void ToggleStrainOnClick()
    {
        Debug.Log("Current scientist round: " + gameSettings.scientistRound);
        if (gameSettings.scientistRound == 3 && !gameSettings.strainOnClick) return;
        gameSettings.strainOnClick = !gameSettings.strainOnClick;
        Debug.Log("Strain on click: " + gameSettings.strainOnClick);
    }

    public void UpdateUI()
    {
        iqText.text = "IQ: " + gameData.iq.ToString();
    }

    private void ResetVariables()
    {
        gameSettings = ScriptableObject.Instantiate(defaultSettings);
        gameData = ScriptableObject.Instantiate(defaultData);
        UpdateUI();
    }

}
