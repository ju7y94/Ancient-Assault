using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonCameraControl : MonoBehaviour
{
[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
PlayerWeapon playerWeapon;
private StarterAssetsInputs starterAssetsInputs;
PlayerMovement playerMovement;
[SerializeField] float normalSensitivity;
[SerializeField] float aimSensitivity;

private void Awake()
{
    starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    playerWeapon = GetComponent<PlayerWeapon>();
    playerMovement = GetComponent<PlayerMovement>();
}

private void Update() {
    if(starterAssetsInputs.aim && playerWeapon.GetGun())
    {
        playerMovement.SetSensitivity(aimSensitivity);
        aimVirtualCamera.gameObject.SetActive(true);
    }
    else
    {
        playerMovement.SetSensitivity(normalSensitivity);
        aimVirtualCamera.gameObject.SetActive(false);
    }
}
}
