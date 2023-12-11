using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class CameraFolow : MonoBehaviour
{
    //object to copy position and rotation smoothly
    [SerializeField] Transform target;
    enum SmoothMode { Curve, Exponential, linear, constant }
    [SerializeField] SmoothMode smoothMode = SmoothMode.Curve;

    [Header("Smooth Mode : Curve")]
    [SerializeField] AnimationCurve translationCurve;
    [SerializeField] AnimationCurve rotationCurve;

    [Header("Smooth Mode : Exponential")]
    [SerializeField] float exponentialFactor = 2f;

    [Header("Smooth Mode : Linear"), InfoBox("The differnece * the factor")]
    [SerializeField] float linearFactor = 1f;

    [Header("Smooth Mode : Constant")]
    [SerializeField] float translationSpeedConstant = 0.1f;
    [SerializeField] float rotationSpeedConstant = 0.1f;

    private void Awake()
    {
        if(GameObject.Find("ControlerPoint"))
            target = GameObject.Find("ControlerPoint").transform;
        else
            print("ControlerPoint not found : Please add the CameraPivot Prefab");
    }

    private void Start()
    {
        //sets the position and rotation of the camera to the target
        SetCameraPosition(target);
        SetCameraRotation(target);
    }

    void FixedUpdate()
    {
        SmoothTranslation();
        SmoothRotate();
    }

    private void SmoothTranslation()
    {
        float translationDistance = Vector3.Distance(transform.position, target.position);
        float LerpFactor = 0;


        switch (smoothMode)
        {
            case SmoothMode.Curve:
                LerpFactor = translationCurve.Evaluate(translationDistance) * Time.fixedDeltaTime;
                break;
            case SmoothMode.Exponential:
                LerpFactor = Mathf.Pow(exponentialFactor, translationDistance) * Time.fixedDeltaTime;
                break;
            case SmoothMode.linear:
                LerpFactor = translationDistance * linearFactor * Time.fixedDeltaTime;
                break;
            case SmoothMode.constant:
                LerpFactor = translationSpeedConstant * Time.fixedDeltaTime;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, target.position, LerpFactor);
    }

    private void SmoothRotate()
    {
        float rotationDifference = Quaternion.Angle(transform.rotation, target.rotation);
        float lerpFactor = 0;

        switch (smoothMode)
        {
            case SmoothMode.Curve:
                lerpFactor = rotationCurve.Evaluate(rotationDifference) * Time.fixedDeltaTime;
                break;
            case SmoothMode.Exponential:
                lerpFactor = Mathf.Pow(exponentialFactor, rotationDifference) * Time.fixedDeltaTime;
                break;
            case SmoothMode.linear:
                lerpFactor = rotationDifference * linearFactor * Time.fixedDeltaTime;
                break;
            case SmoothMode.constant:
                lerpFactor = rotationSpeedConstant * Time.fixedDeltaTime;
                break;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, lerpFactor);
    }

    public void SetCameraPosition(Transform position)
    {
        this.transform.position = position.position;
    }

    public void SetCameraRotation(Transform rotation)
    {
        this.transform.rotation = rotation.rotation;
    }
}
