using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class StateClass : MonoBehaviour
{
    public enum EState { Idle, Launching, Cooldown };

    // different states of the jump pad
    private EState _currentState;
    protected EState currentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            switch (_currentState)
            {
                case EState.Idle:
                    onIdle?.Invoke();
                    break;
                case EState.Launching:
                    onLaunching?.Invoke();
                    break;
                case EState.Cooldown:
                    onCooldown?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    [Space]
    [Foldout("Events")]
    public UnityEvent onIdle;
    [Foldout("Events")]
    public UnityEvent onLaunching;
    [Foldout("Events")]
    public UnityEvent onCooldown;
}