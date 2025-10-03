using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scientist : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PerkSelect[] perkButtons;
    [SerializeField] private Animator animator;
    [SerializeField] private Image timer;
    [SerializeField] private float totalTime;
    [SerializeField] private float timeRemaining;
    [SerializeField] private int perkButtonSelected;

    private void OnEnable()
    {
        totalTime = gameManager.gameSettings.scientistInterval / 2 - animator.GetCurrentAnimatorStateInfo(0).length * 2;
        timeRemaining = totalTime;
        FillButtons();
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(gameManager.gameSettings.scientistDuration);
        ConfirmPerk();
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].Disable();
        }
        animator.Play("Retract");
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timer.fillAmount = timeRemaining / totalTime;
        }
    }

    public void SetInactive()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void FillButtons()
    {
        Perk[] perks = gameManager.GetPerks();
        if (perks.Length < perkButtons.Length)
        {
            Debug.LogWarning("Not enough perks to fill all buttons");
            return;
        }
        List<int> perksSent = new List<int>();

        for (int i = 0; i < perks.Length; i++)
        {
            int randomPerk = Random.Range(0, perks.Length);
            while (perksSent.Contains(randomPerk))
            {
                randomPerk = Random.Range(0, perks.Length);
            }
            perksSent.Add(randomPerk);
            perkButtons[i].RecievePerk(perks[randomPerk], randomPerk);
        }
    }

    public void SelectPerk(int perkIndex)
    {
        perkButtonSelected = perkIndex;
    }

    private void ConfirmPerk()
    {
        // if (perkButtonSelected == 0) perkButtonSelected = 1; // Default to first perk if none selected

        gameManager.AddPerk(perkButtonSelected);
    }
}
