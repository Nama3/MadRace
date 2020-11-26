using MightyAttributes;
using PathCreation;
using UnityEngine;

public abstract class BaseRunnerBehaviour : MonoBehaviour
{
    [SerializeField, GetComponent, ReadOnly] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    
    private PathCreator m_path;
    private float m_distanceTravelled;
    
    protected bool CanRun { get; set; }

    public void SetPath(PathCreator path) => m_path = path;

    public void Init()
    {
        m_distanceTravelled = 0;
        CanRun = false;
        SetPosition();
    }
    
    public void UpdateBehaviour() => UpdateRunner();

    public void FixedUpdateBehaviour()
    {
        if (!CanRun) return;
        
        m_distanceTravelled += _speed * Time.deltaTime;
        SetPosition();
        
        FixedUpdateRunner();
    }

    private void SetPosition()
    {
        var nextPosition = m_path.path.GetPointAtDistance(m_distanceTravelled);
        var nextRotation = m_path.path.GetRotationAtDistance(m_distanceTravelled);

        _rigidbody.MovePosition(nextPosition);
        _rigidbody.rotation = nextRotation;
    }

    protected abstract void UpdateRunner();
    protected abstract void FixedUpdateRunner();
}