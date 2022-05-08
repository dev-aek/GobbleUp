using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;


    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Boss)
        {
            cameras[1].Priority = 11;
        }
    }
}
