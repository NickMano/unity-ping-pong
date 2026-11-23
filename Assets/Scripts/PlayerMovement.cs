using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameSession session;

    private SpriteRenderer playerSpriteRender;
    private float screenHeight, limit;
    private bool canMove = false;

    private void Awake()
    {
        playerSpriteRender = GetComponent<SpriteRenderer>();
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        limit = screenHeight - playerSpriteRender.bounds.extents.y;
    }

    private void OnEnable()
    {
        if (session == null)
        {
            Debug.LogError("GameSession reference is missing in PlayerMovement.");
            return;
        }

        session.OnStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        if (session == null) return;
        session.OnStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (!canMove) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(mousePos.y, -limit, limit), transform.position.z);
    }

    private void OnGameStateChanged(GameState newState)
    {
        canMove = newState == GameState.Playing;
    }
}
