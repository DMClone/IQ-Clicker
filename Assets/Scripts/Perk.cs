using UnityEngine;

[CreateAssetMenu(fileName = "Perk", menuName = "Scriptable Objects/Perk")]
public class Perk : ScriptableObject
{
    public string perkName => name;
    [TextArea] public string description;

    public StatModifier[] modifiers;
}
