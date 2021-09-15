using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    private int score = 0;

    public Text livesUI;
    public Text scoreUI;

    public GameObject restartPanel;
    public Text finishScore;

    private void Update()
    {
        livesUI.text = lives.ToString();
        scoreUI.text = score.ToString();
        if(lives == 0)
        {
            ShowRestartPanel();
        }
    }

    private void ShowRestartPanel()
    {
        finishScore.text = score.ToString();
        restartPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void DecreaseLives(int count)
    {
        lives -= count;
    }

    public void IncreaseScore(int count)
    {
        score += count;
    }
}
