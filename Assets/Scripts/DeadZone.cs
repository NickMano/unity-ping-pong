using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeadZone : MonoBehaviour
{
    private AudioSource audioSource;

    public event Action OnBallEnterDeadZone;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out BallBehaviour ball)) return;

        OnBallEnterDeadZone?.Invoke();
        audioSource.Play();
    }
}
