using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Mission", menuName = "Scriptable Objects/Mission")]
public class Mission : ScriptableObject, IMission
{
    [field: SerializeField] public string NameMission { get; private set; }
    [Min(0)] [field: SerializeField] public float DelayStart { get; private set; }
    [field: SerializeField] public Mission PreviousMission{ get; private set; }
    [field: SerializeField] public Mission NextMission{ get; private set; }
    private Timer _timer;
    
    public UnityEvent OnStartedShow;
    public UnityEvent OnMissionPointReachedShow;
    public UnityEvent OnFinishedShow;
    public event Action OnStarted;
    public event Action OnMissionPointReached;
    public event Action OnFinished;

    void IMission.Start()
    {
        UpdateEvents();

        _timer = new Timer();
        _ = _timer.StartAsync((int)(DelayStart * 1000), Run);
    }

    private void UpdateEvents()
    {
        OnStarted = null;
        OnMissionPointReached = null;
        OnFinished = null;
        OnStarted += () =>
        {
            OnStartedShow?.Invoke();
            Debug.Log($"On Started: {NameMission}");
        };
        OnMissionPointReached += () =>
        {
            OnMissionPointReachedShow?.Invoke();
            Debug.Log($"On Mission Point Reached: {NameMission}");
        };
        OnFinished += () =>
        {
            OnFinishedShow?.Invoke();
            Debug.Log($"On Finished: {NameMission}");
        };
        OnFinished += StartNextMission;
        
        OnStarted?.Invoke();
    }

    public void Cancel()
    {
        _timer?.Cancel();
        if (PreviousMission != null)
        {
            ((IMission)PreviousMission).Start();
        }
    }

    private void StartNextMission()
    {
        if (NextMission != null)
        {
            ((IMission)NextMission).Start();
        }
    }
    private void Run()
    {
        OnMissionPointReached?.Invoke();
        OnFinished?.Invoke();
    }
}
