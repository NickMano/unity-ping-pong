using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameSession session;

    private void OnEnable()
    {
        if (session == null)
        {
            Debug.LogError("GameSession reference is missing in SceneChanger.");
            return;
        }

        session.OnStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        if (session == null) return;
        session.OnStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.GameOver:
                ChangeSceneTo("GameOverScene");
                break;
            case GameState.Win:
                ChangeSceneTo("WinScene");
                break;
        }
    }

    private void ChangeSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        ChangeSceneTo("GameScene");
    }
}