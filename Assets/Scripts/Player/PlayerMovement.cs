using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameSession session;

    private SpriteRenderer playerSpriteRender;
    private float screenHeight, limit;

    private void Awake()
    {
        playerSpriteRender = GetComponent<SpriteRenderer>();
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        limit = screenHeight - playerSpriteRender.bounds.extents.y;
    }

    private void Start()
    {
        if (session == null)
            Debug.LogError("GameSession reference is missing in PlayerMovement.");
    }

    private void Update()
    {
        if (session.State != GameState.Playing) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(mousePos.y, -limit, limit), transform.position.z);
    }
}
