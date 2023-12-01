using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    // serialize field of variables to tweek shake in inspector
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeStrength = 0.1f;
    [SerializeField] int shakeVibrato = 10;
    [SerializeField] float shakeRandomness = 90;
    [SerializeField] bool shakeFadeOut = true;
    [SerializeField] bool shakeSnapping = false;
    
    // bool to check if camera is already shaking
    bool isShaking = false;

    private void OnEnable()
    {
        // add the event to the camera
        JumpPadMain.LaunchPlayerEvent += ShakeCamera;
    }

    private void OnDisable()
    {
        // remove the event from the camera
        JumpPadMain.LaunchPlayerEvent -= ShakeCamera;
    }


    public void ShakeCamera()
    {
        // if camera is already shaking, return
        if (isShaking)
            return;

        isShaking = true;

        // shake the camera by moving it in a random direction using DOTween
        transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, shakeStrength, 0), shakeVibrato, shakeRandomness, shakeFadeOut, shakeSnapping).OnComplete(() => isShaking = false);
    }
}