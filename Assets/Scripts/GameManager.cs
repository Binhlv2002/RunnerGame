using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    public float currentScore = 0f;
    public Data data;
    public bool isPlaying = false;
    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    private bool gameEnded = false;

    private void Start()
    {
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else { data = new Data(); }
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }
    }   

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        currentScore = 0;

    }
    public void GameOver()
    {
        onGameOver.Invoke();
        isPlaying = false;
        if (data.highScore < currentScore)
        {
            data.highScore = currentScore;
            string saveString = JsonUtility.ToJson(data);
            SaveSystem.Save("save", saveString);
        }
        
        
    }

    public void ContinueGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        gameEnded = false;
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public string PrettyScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }
    public string PrettyHighScore()
    {
        return Mathf.RoundToInt(data.highScore).ToString();
    }
}
