using UnityEngine;

public enum StatType
{
    IQPerClick,
    StrainReward,
    StrainDuration,
    StrainInterval,
    StrainAmount,
    StrainOnClick,
    IQMultiplierPerGoldenStrain
}

[System.Serializable]
public class StatModifier
{
    public StatType statType;
    public float value;
    public bool isMultiplier;
}
