
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text ScoreText;
    [SerializeField] private GameObject LosePanel; 

    int _Score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int score)
    {
        _Score += score;
        ScoreText.text = _Score.ToString();
        if(_Score != 0 && _Score%2 == 0)
        {
            GameManager.instance.MakeGameHarder();
        }
    }

    public void ActiveGameOverMenu()
    {
        LosePanel.SetActive(true);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
