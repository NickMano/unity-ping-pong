using UnityEngine;

public class OponentBehaviour : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private BallBehaviour ball;
    [SerializeField][Min(0f)] private float speed;

    private Vector3 initPos;
    private float limit = 3.715f;
    private bool canMove = false;

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
        if (!canMove)
        {
            transform.position = transform.position;
            return;
        }

        if (ball == null)
        {
            Debug.LogError("Ball reference is missing in OponentBehaviour.");
            return;
        }

        if (ball.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
        else if (ball.transform.position.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - speed, -limit, limit), transform.position.z);
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Ready)
        {
            transform.position = initPos;
        }

        canMove = newState == GameState.Playing;
    }
}
