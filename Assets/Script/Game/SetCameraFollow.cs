using UnityEngine;
using Unity.Cinemachine;
using System;

public class SetCameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vCam;

    private void SetFollow(GameObject player)
    {
        vCam.Follow = player.transform;
    }

    private void OnEnable()
    {
        GameController.OnPlayerSpawned += SetFollow;
    }


    private void OnDisable()
    {
        GameController.OnPlayerSpawned -= SetFollow;
    }
    
}
