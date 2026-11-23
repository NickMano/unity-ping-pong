using UnityEngine;

public class OponentBehaviour : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private BallBehaviour ball;
    [SerializeField][Min(0f)] private float speed;

    private Vector3 initPos;
    private float limit = 3.715f;

    private void Awake()
    {
        initPos = transform.position;
    }

    private void OnEnable()
    {
        if (session == null)
        {
            Debug.LogError("GameSession reference is missing in OponentBehaviour.");
            return;
        }

        session.OnStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        session.OnStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (session.State != GameState.Playing) return;

        if (ball == null)
        {
            Debug.LogError("Ball reference is missing in OponentBehaviour.");
            return;
        }

        if (ball.transform.position.y > transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed);
        }
        else if (ball.transform.position.y < transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y - speed, -limit, limit));
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState != GameState.Ready) return;
        transform.position = initPos;
    }
}
