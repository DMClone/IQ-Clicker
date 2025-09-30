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

        int breakCount = perkDescriptionText.text.Split('\n').Length - 1;
        int totalLines = breakCount + 1;
        int height = startHeight + (heightIncrement * totalLines - 1);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        frontPage.HeightIncrease(height);
    }
}
