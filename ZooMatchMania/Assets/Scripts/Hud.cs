using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public Level level;
    public GameOver gameOver;
    public TextMeshProUGUI remainingText;
    public TextMeshProUGUI remainingSubText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI targetSubtext;
   
    public TextMeshProUGUI scoreText;
    private int _starIndex;

    public void Initialize()
    {
        level = FindObjectOfType<Level>();
    }
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
        int visibleStar = 0;

        if (score >= level.score1Star && score < level.score2Star)
        {
            visibleStar = 1;
        }
        else if  (score >= level.score2Star && score < level.score3Star)
        {
            visibleStar = 2;
        }
        else if (score >= level.score3Star)
        {
            visibleStar = 3;
        }
        _starIndex = visibleStar;
    }

    public void SetTarget(int target) => targetText.text = target.ToString();

    public void SetRemaining(int remaining) => remainingText.text = remaining.ToString();

    public void SetRemaining(string remaining) => remainingText.text = remaining;

    public void SetLevelType(LevelType type)
    {
        switch (type)
        {
            case LevelType.Moves:
                remainingSubText.text = "moves remaining";
                targetSubtext.text = "target score";
                break;
            case LevelType.Obstacle:
                remainingSubText.text = "moves remaining";
                targetSubtext.text = "bubbles remaining";
                break;
            case LevelType.Timer:
                remainingSubText.text = "time remaining";
                targetSubtext.text = "target score";
                break;
        }
    }

     public void OnGameWin(int score)
     {
         gameOver.ShowWin(score, _starIndex);
         if (_starIndex > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0))
         {
             PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, _starIndex);
         }

         PlayerPrefs.SetInt("UnlockedLevel",SceneManager.GetActiveScene().buildIndex+1);
         PlayerPrefs.Save();
     }

    public void OnGameLose() => gameOver.ShowLose();

    public void OnNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
        Destroy(FindObjectOfType<GameController>());
    }
    
    public void OnPrevLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex - 1);
        Destroy(FindObjectOfType<GameController>());
    }
}

