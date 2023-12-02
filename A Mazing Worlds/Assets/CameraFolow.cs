using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Autodesk.Fbx;
using System.Net.NetworkInformation;

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
    [SerializeField] float translationSpeed = 0.1f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float exponentialFactor = 0.1f;

    [Header("Smooth Mode : Linear")]
    [SerializeField] float translationBaseSpeed = 1f;
    [SerializeField] float rotationBaseSpeed = 1f;
    [SerializeField] float linearFactor = 0.1f;

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
    }

    void FixedUpdate()
    {

        SmoothTranslation();
        SmoothRotate();

        //transform.position = Vector3.Lerp(transform.position, target.position, translationCurve.Evaluate(Vector3.Distance(transform.position, target.position)) * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationCurve.Evaluate(Vector3.Distance(transform.position, target.position)) * Time.fixedDeltaTime);
        
        //transform.position = Vector3.Lerp(transform.position, target.position, translationSpeed * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationSpeed * Time.fixedDeltaTime);
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
                LerpFactor = translationBaseSpeed / (translationDistance * linearFactor) * Time.fixedDeltaTime;
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
                lerpFactor = rotationBaseSpeed / (rotationDifference * linearFactor) * Time.fixedDeltaTime;
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
}
