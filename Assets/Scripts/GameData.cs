using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    public int iq;
    public int brainClicks;
    public int goldenStrainsCollected;
    public int falseStrainsCollected;
    public List<Perk> perks;

    public int sessionId;
}
