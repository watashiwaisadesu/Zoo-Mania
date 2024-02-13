using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GameOver : MonoBehaviour
{
    public GameObject starsBG;
    public GameObject starsHolder;
    public Image[] stars;

    private void Start ()
    {
        starsBG.SetActive(false);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }
    }

    public void ShowLose()
    {
        starsBG.SetActive(true);
        starsHolder.SetActive(true);
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOver");
        }
    }

    public void ShowWin(int score, int starCount)
    {
        starsBG.SetActive(true);
        starsHolder.SetActive(true);
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOver");
        }

        StartCoroutine(ShowWinCoroutine(starCount));
    }

    private IEnumerator ShowWinCoroutine(int starCount)
    {
        if (starCount <= stars.Length)
        {
            for (int i = 0; i < starCount; i++)
            {
                stars[i].enabled = true;
                yield return new WaitForSeconds(0.5f);
            }

        }
    }

    public void OnReplayClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnDoneClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }

}

