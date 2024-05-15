using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreUI;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onGameOver.AddListener(ActiveGameOverUI);
    }

    public void PlayButtonHandler()
    {
        gameManager.StartGame();
        
    }

    public void ContinueButtonHandler()
    {
        gameManager.ContinueGame();
        gameOverUI.SetActive(false);
    }


    public void ActiveGameOverUI()
    {
        gameOverUI.SetActive(true);
        gameOverScoreUI.text = "Score: " + gameManager.PrettyScore();
        gameOverHighScoreUI.text = "HighScore: " + gameManager.PrettyHighScore();
    }

    public void ExitButtonHandler()
    {
        Application.Quit();
    }
    private void OnGUI()
    {
        scoreUI.text = gameManager.PrettyScore();
    }
}
