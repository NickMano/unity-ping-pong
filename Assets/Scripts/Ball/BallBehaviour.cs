using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField][Min(1f)] private float velocity;
    private Rigidbody2D rbBall;
    private int launchDirection;
    private Vector3 initialPosition;

    private bool isLaunched = false;

    private void Awake()
    {
        rbBall = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        launchDirection = Random.Range(-50, 50);
    }

    private void OnEnable()
    {
        if (session == null)
        {
            Debug.LogError("GameSession reference is missing in BallBehaviour.");
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
            case GameState.Ready:
                ResetBall();
                break;
            case GameState.Playing:
                if (!isLaunched)
                {
                    Launch();
                    isLaunched = true;
                }
                break;
        }
    }

    private void ResetBall()
    {
        rbBall.linearVelocity = Vector2.zero;
        transform.position = initialPosition;
        launchDirection = Random.Range(-50, 50);
        isLaunched = false;
    }

    private void Launch()
    {
        rbBall.linearVelocity = new Vector2(launchDirection <= 0 ? -velocity : velocity, Random.Range(-velocity, velocity));
    }
}
