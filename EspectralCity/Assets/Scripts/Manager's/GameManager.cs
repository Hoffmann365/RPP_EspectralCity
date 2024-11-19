using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject pauseObj;
    public GameObject gameOverObj;
    public bool isPaused;
    public bool isGameOver;
    
    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        GameObserver.NextScene += CarregarCena;
        GameObserver.GameOver += GameOver;
    }

    private void OnDisable()
    {
        GameObserver.NextScene -= CarregarCena;
        GameObserver.GameOver -= GameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }
    

    public void CarregarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    public void PauseGame()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
                pauseObj.SetActive(isPaused);
            }

            if (isPaused)
            {
                Time.timeScale = 0;
                AudioObserver.OnPauseMusicEvent();
            }
            else
            {
                Time.timeScale = 1;
                AudioObserver.OnUnpauseMusicEvent();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        AudioObserver.OnStopMusicEvent();
        gameOverObj.SetActive(true);
        Time.timeScale = 0;
        AudioObserver.OnPlaySfxEvent("gameover");
    }

    public void RestartGame()
    {
        gameOverObj.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
        AudioObserver.OnPlayMusicEvent();
    }
}
