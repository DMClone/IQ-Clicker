using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PerkSelect : MonoBehaviour
{
    [SerializeField] private Scientist scientist;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button button;
    [ReadOnly] public Perk heldPerk;
    [ReadOnly] public int perkIndex;

    private void OnEnable()
    {
        button.interactable = true;
    }

    public void Disable()
    {
        button.interactable = false;
    }

    public void RecievePerk(Perk perk, int index)
    {
        heldPerk = perk;
        perkIndex = index;
        descriptionText.text = perk.description;
    }

    public void OnClick()
    {
        scientist.SelectPerk(perkIndex);
    }

}
