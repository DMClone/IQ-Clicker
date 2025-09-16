using System.Collections;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(GameManager.Instance.gameSettings.scientistDuration);
        animator.Play("Retract");
    }

    public void SetInactive()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
