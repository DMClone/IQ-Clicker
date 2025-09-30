using UnityEngine;

public class FrontPage : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject scrollViewContent;
    [SerializeField] GameObject perkImagePrefab;
    [SerializeField] GameObject perkRoundImagePrefab;
    private int totalHeight;
    private int contentSpacing;
    private int perkRoundHeight;

    private void Start()
    {
        Debug.Log("FrontPage started.");
        contentSpacing = Mathf.RoundToInt(scrollViewContent.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().spacing);
        perkRoundHeight = Mathf.RoundToInt(perkRoundImagePrefab.GetComponent<RectTransform>().sizeDelta.y);
        FillPerkList();
    }

    private void FillPerkList()
    {
        for (int i = 0; i < gameManager.gameSettings.perkList.Length; i++)
        {
            Instantiate(perkRoundImagePrefab, scrollViewContent.transform).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Round {i + 1}";
            HeightIncrease(perkRoundHeight);
            for (int j = 0; j < gameManager.gameSettings.perkList[i].perks.Length; j++)
            {
                PerkImage perkImage = Instantiate(perkImagePrefab, scrollViewContent.transform).GetComponent<PerkImage>();
                perkImage.GetComponent<PerkImage>().frontPage = this;
                perkImage.Initialize(gameManager.gameSettings.perkList[i].perks[j]);
            }
        }
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollViewContent.GetComponent<RectTransform>().sizeDelta.x, totalHeight);
    }

    public void HeightIncrease(int height)
    {
        totalHeight += height + contentSpacing;
    }
}
