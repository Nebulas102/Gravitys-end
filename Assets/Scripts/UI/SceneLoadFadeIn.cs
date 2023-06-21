using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadFadeIn : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject loadingScreen;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
        yield return null;
    }
}
