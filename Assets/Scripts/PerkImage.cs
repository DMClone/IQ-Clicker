using UnityEngine;

public class PerkImage : MonoBehaviour
{
    [HideInInspector] public FrontPage frontPage;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMPro.TextMeshProUGUI perkNameText;
    [SerializeField] private TMPro.TextMeshProUGUI perkDescriptionText;
    [SerializeField] private int startHeight;
    [SerializeField] private int heightIncrement;

    public void Initialize(Perk perk)
    {
        perkNameText.text = perk.perkName;
        perkDescriptionText.text = perk.description;
        perkDescriptionText.ForceMeshUpdate();

        int totalLines = perkDescriptionText.textInfo.lineCount;
        int height = startHeight + (heightIncrement * totalLines);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        frontPage.HeightIncrease(height);
    }
}
