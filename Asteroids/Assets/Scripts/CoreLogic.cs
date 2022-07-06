using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreLogic : MonoBehaviour
{
    public static event Action OnChangeControl;

    private MenuButtons menuButtons;
    private Score score;
    public bool gameStarted;

    private void Start()
    {
        var scoreGameObject = GameObject.FindGameObjectWithTag("Score");
        score = scoreGameObject.GetComponent<Score>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        PlayerHealth.OnEndGame -= OnEndGame;
    }

    public void OnStartGame()
    {
        gameStarted = true;
    }
    private void OnEndGame()
    {
        score.SaveScore();
        SceneManager.LoadScene(0);
    }

    public void ChangeControl()
    {
        OnChangeControl?.Invoke();
    }

}
