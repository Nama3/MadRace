using MightyAttributes;
using PathCreation;
using UnityEngine;

public abstract class BaseRunnerBehaviour : MonoBehaviour
{
    public enum RunnerState : byte
    {
        Init,
        Playing,
        Stopped
    }
    
    private const float MIN_HEIGHT = -1.5f;

    [SerializeField, GetComponent, ReadOnly]
    private Rigidbody _rigidbody;
    [SerializeField, GetComponentInChildren, ReadOnly]
    private Animator _animator;

    [SerializeField, AnimatorParameter("Running"), ReadOnly]
    private int _runningID;

    [SerializeField] private float _speed;

    private PathCreator m_path;
    private float m_distanceTravelled;
    private bool m_canRun;

    protected bool CanRun
    {
        get => m_canRun;
        set
        {
            if (m_canRun == value) return;
            
            m_canRun = value;
            _animator.SetBool(_runningID, value);
        }
    }

    public RunnerState State { get; protected set; }

    public void SetPath(PathCreator path) => m_path = path;
    
    public void Init()
    {
        State = RunnerState.Init;
        CanRun = false;

        m_distanceTravelled = 0;
        
        ResetRigidbody();

        SetPosition();
        
        OnInit();
    }
    
    public void Play()
    {
        State = RunnerState.Playing;
        
        OnPlay();
    }

    public void Stop()
    {
        State = RunnerState.Stopped;
        CanRun = false;
    }

    protected void ResetRigidbody()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void UpdateBehaviour()
    {
        if (State == RunnerState.Stopped) return;
        OnUpdate();
    }

    public void FixedUpdateBehaviour()
    {
        if (_rigidbody.position.y < MIN_HEIGHT)
        {
            Stop();
            OnFallOutOfMap();
        }
        
        if (State == RunnerState.Stopped) return;

        if (!CanRun) return;

        m_distanceTravelled += _speed * Time.deltaTime;
        SetPosition();

        OnFixedUpdate();
    }

    private void SetPosition()
    {
        var nextPosition = m_path.path.GetPointAtDistance(m_distanceTravelled);
        var nextRotation = m_path.path.GetRotationAtDistance(m_distanceTravelled);

        _rigidbody.MovePosition(nextPosition);
        _rigidbody.rotation = nextRotation;
    }

    protected abstract void OnInit();
    protected abstract void OnPlay();
    
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();

    protected abstract void OnFallOutOfMap();

    private void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if (layer != StaticLayers.FINISH_LINE) return;

        LevelManager.Instance.FinishLevel(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        var layer = other.gameObject.layer;
        if (layer != StaticLayers.OBSTACLE) return;

        _rigidbody.useGravity = true;
        // _rigidbody.AddForce(other.relativeVelocity.normalized * EJECT_FORCE, ForceMode.Impulse);

        Stop();
    }
}