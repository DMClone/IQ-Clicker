using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(2, 120)] public int gameDuration = 90;
    [Range(0, 50)] public int iqPerClick = 10;
    public float iqMultiplierPerGoldenStrain = 1;
    public bool strainOnClick = false;
    [Range(.1f, 4)] public float strainDuration = 1;
    [Range(0, 10)] public int goldenStrainAmount = 6;
    [Range(0, 10)] public int falseStrainAmount = 3;
    [Range(2, 40)] public int strainInterval = 10;
    [Range(10, 100)] public int goldenStrainReward = 20;
    [Range(10, 100)] public int falseStrainPenalty = 20;
    [Range(0, 5)][Tooltip("Amount of seconds randomly added for a strain appearance")] public int strainRandom = 4;
    [Range(0, 300)][Tooltip("Distance from brain (Always on the edge)")] public int strainSpawnRadius = 250;
    [Range(1, 10)] public int scientistDuration = 5;
    [Range(5, 60)] public int scientistInterval = 30;
    public int goldenstrainsSpawned = 0;
    public int falseStrainsSpawned = 0;
    public int scientistRound = 0;
    [SerializeField] public PerkRound[] perkList;

    [System.Serializable]
    public class PerkRound
    {
        public Perk[] perks;
    }
}
