using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IQStrain : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    [System.Serializable]
    public struct StrainSettings
    {
        [ReadOnly] public bool isGolden;
        public Sprite[] sprites;
    }
    public StrainSettings strainSettings = new StrainSettings();
    void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
        switch (strainSettings.isGolden)
        {
            case true:
                TurnGolden();
                break;
            case false:
                TurnFalse();
                break;
        }
    }

    public void OnClick()
    {
        GameManager.Instance.CollectStrain(strainSettings.isGolden);
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    void TurnGolden()
    {
        image.sprite = strainSettings.sprites[0];
        // Change appearance to golden
    }

    void TurnFalse()
    {
        image.sprite = strainSettings.sprites[1];
        // Change appearance to false
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(GameManager.Instance.gameSettings.strainDuration);
        gameObject.SetActive(false);
    }
}
