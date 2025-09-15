using System.Collections;
using UnityEngine;

public class IQStrain : MonoBehaviour
{
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
        // Change appearance to golden
    }

    void TurnFalse()
    {
        // Change appearance to false
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(GameManager.Instance.gameSettings.goldenStrainDuration);
        gameObject.SetActive(false);
    }
}
