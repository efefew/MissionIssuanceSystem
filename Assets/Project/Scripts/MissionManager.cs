using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private List<Mission> _startMissions;
    [SerializeField] private Button _startMissionButton;
    private void Awake()
    {
        _startMissionButton.onClick.AddListener(RunMissions);
    }

    private void RunMissions()
    {
        foreach (Mission mission in _startMissions)
        {
            _ = new Timer().StartAsync(0, ((IMission)mission).Start);
        }
    }
}
