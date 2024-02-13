using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public int Level { get; set; }
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnOpenScene);
    }

    private void OnOpenScene()
    {
        SceneManager.LoadScene("Level " + Level);
    }
  
}
