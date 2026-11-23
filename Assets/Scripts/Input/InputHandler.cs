using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameSession session;

    private InputSystem_Actions actions;

    public event Action PausePressed;
    public event Action InteractPressed;

    public Vector2 MoveAxis { get; private set; }

    private void Awake()
    {
        MoveAxis = Vector2.zero;

        if (session == null)
            Debug.LogError("GameSession reference is missing in InputHandler");
    }

    private void OnEnable()
    {
        actions ??= new InputSystem_Actions();
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        MoveAxis = Vector2.zero;
        actions.Player.Disable();
    }

    private void OnDestroy()
    {
        actions.Dispose();
    }

    void Update()
    {
        if (actions == null) return;

        if (actions.Player.Interact.WasPressedThisFrame())
            InteractPressed?.Invoke();

        bool canPlay = session.State == GameState.Playing;

        MoveAxis = canPlay
            ? Vector2.ClampMagnitude(
                actions.Player.Move.ReadValue<Vector2>(), 1f)
            : Vector2.zero;

        if (actions.Player.Pause.WasPressedThisFrame())
            PausePressed?.Invoke();
    }
}