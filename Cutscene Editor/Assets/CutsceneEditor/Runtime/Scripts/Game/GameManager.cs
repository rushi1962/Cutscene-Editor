using System;
using StarterAssets;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public event Action<bool> SetPlayerControllerEnabledEvent;

    private void Awake()
    {
        if(Instance!= null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void SetPlayerControllerEnabled(bool isEnabled)
    {
        SetPlayerControllerEnabledEvent?.Invoke(isEnabled);
    }
}
