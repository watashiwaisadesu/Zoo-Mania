using UnityEngine;
using UnityEngine.UI;

public class LevelCount : MonoBehaviour
{
    public LevelSelector[] levelSelectors;
    public Button[] buttons;
    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < levelSelectors.Length; i++)
        {
            levelSelectors[i].Level = i;
            
        }
    }
}
