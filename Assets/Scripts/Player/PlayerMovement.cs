using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameSession Session;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField, Min(1f)] private float Speed;

    public Vector2 Direction { get; private set; }
    private SpriteRenderer playerSpriteRender;
    private Rigidbody2D rigidbody2D;
    private float screenHeight, limit;

    private void Awake()
    {
        Direction = Vector2.zero;
        playerSpriteRender = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        limit = screenHeight - playerSpriteRender.bounds.extents.y;
    }

    private void OnEnable()
    {
        if (Session == null)
        {
            Debug.LogError("GameSession reference is missing in PlayerMovement.");
            return;
        }

        Session.OnStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        StopMovement();
        Session.OnStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        if (Session == null)
            Debug.LogError("GameSession reference is missing in PlayerMovement.");

        if (inputHandler == null)
            Debug.LogError("InputHandler reference is missing in PlayerMovement.");
    }

    private void FixedUpdate()
    {
        if (Session.State != GameState.Playing) return;
        if (inputHandler == null) return;

        Direction = inputHandler.MoveAxis;
        rigidbody2D.linearVelocity = Speed * Direction;

        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -limit, limit));
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.GameOver || newState == GameState.Win)
        {
            StopMovement();
            return;
        }

        if (newState == GameState.Ready)
        {
            StopMovement();
            transform.position = new Vector2(transform.position.x, 0);
        }
    }

    private void StopMovement()
    {
        Direction = Vector2.zero;
        rigidbody2D.linearVelocity = Vector2.zero;
    }
}
