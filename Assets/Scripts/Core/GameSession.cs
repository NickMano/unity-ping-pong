using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSession : MonoBehaviour
{
    private GameState currentGameState = GameState.Ready;
    private int playerScoreQuantity = 0;
    private int oponentScoreQuantity = 0;

    [SerializeField] private DeadZone leftDeadZone;
    [SerializeField] private DeadZone rightDeadZone;

    [SerializeField] private Text leftScoreText;
    [SerializeField] private Text rightScoreText;

    public event Action<GameState> OnStateChanged;
    public GameState State => currentGameState;

    private void OnEnable()
    {
        if (leftDeadZone == null || rightDeadZone == null)
        {
            Debug.LogError("DeadZone references are missing in GameSession.");
            return;
        }

        leftDeadZone.OnBallEnterDeadZone += LeafDeadZone_OnBallEnterDeadZone;
        rightDeadZone.OnBallEnterDeadZone += RightDeadZone_OnBallEnterDeadZone;
    }

    private void OnDisable()
    {
        if (leftDeadZone == null || rightDeadZone == null) return;

        leftDeadZone.OnBallEnterDeadZone -= LeafDeadZone_OnBallEnterDeadZone;
        rightDeadZone.OnBallEnterDeadZone -= RightDeadZone_OnBallEnterDeadZone;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
                Time.timeScale = 1f;
            }
            else if (currentGameState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0) && currentGameState == GameState.Ready)
        {
            ChangeState(GameState.Playing);
        }
    }

    private void LeafDeadZone_OnBallEnterDeadZone()
    {
        UpdateScore(ref oponentScoreQuantity, rightScoreText);
    }

    private void RightDeadZone_OnBallEnterDeadZone()
    {
        UpdateScore(ref playerScoreQuantity, leftScoreText);
    }

    private void UpdateScore(ref int scoreQuantity, Text scoreText)
    {
        scoreQuantity++;
        scoreText.text = scoreQuantity.ToString();

        ChangeState(GameState.Ready);

        CheckScore();
    }

    private void CheckScore()
    {
        if (playerScoreQuantity >= 3)
        {
            ChangeState(GameState.Win);
        }
        else if (oponentScoreQuantity >= 3)
        {
            ChangeState(GameState.GameOver);
        }
    }

    private void ChangeState(GameState newState)
    {
        currentGameState = newState;
        OnStateChanged?.Invoke(currentGameState);
    }
}
