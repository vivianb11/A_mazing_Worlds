using Codice.CM.Client.Differences;
using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorLDPlacor : EditorWindow
{
    private GameObject snapObject;
    private float snapOffset = 0;
    private bool resetRotation = true;
    private enum UpAxis { X, Y, Z };
    private UpAxis upAxis = UpAxis.Y;

    [MenuItem("Tools/Snapping Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EditorLDPlacor), false, "Snapping Editor");
    }
    void OnGUI()
    {
        GUILayout.Label("Snap Object", EditorStyles.boldLabel);

        // Allow the user to select the object to snap + the offset + if the object should rotate to the normal
        snapObject = EditorGUILayout.ObjectField("Snap Object", snapObject, typeof(GameObject), true) as GameObject;
        snapOffset = EditorGUILayout.FloatField("Offset", snapOffset);
        upAxis = (UpAxis)EditorGUILayout.EnumPopup("Up Axis", upAxis);
        resetRotation = EditorGUILayout.Toggle("Rotate to Normal", resetRotation);
        

        if (GUILayout.Button("Snap"))
        {
            if (snapObject != null && Selection.gameObjects.Length > 0)
            {

                Vector3 center = snapObject.transform.position;

                RaycastHit hit;

                foreach (GameObject selectedObject in Selection.gameObjects)
                {
                    if (selectedObject == snapObject)
                        continue;

                    Undo.RecordObject(selectedObject.transform, "Snap");

                    Vector3 direction = center - selectedObject.transform.position;

                    if (!Physics.Raycast(selectedObject.transform.position - direction.normalized * 0.000001f, direction, out hit, Mathf.Infinity))
                        continue;

                    Debug.DrawLine(selectedObject.transform.position - direction.normalized, hit.point, Color.red, 2.5f);

                    // Move the object to the collision point
                    selectedObject.transform.position = hit.point ;

                    selectedObject.transform.position += hit.normal.normalized * snapOffset;

                    // rotate the object so it has the same up as the normal by doing the less changes possible to every other axis
                    if (!resetRotation)
                        continue;

                    Vector3 initialRotation = selectedObject.transform.eulerAngles;

                    selectedObject.transform.rotation = Quaternion.identity;

                    //sets the forward to the normal depending on the enum
                    switch (upAxis)
                    {
                        case UpAxis.X:
                            selectedObject.transform.right = hit.normal;
                            break;
                        case UpAxis.Y:
                            selectedObject.transform.up = hit.normal;
                            break;
                        case UpAxis.Z:
                            selectedObject.transform.forward = hit.normal;
                            break;
                    }

                    Vector3 rotation = selectedObject.transform.eulerAngles;

                    switch (upAxis)
                    {
                        case UpAxis.X:
                            rotation.x = initialRotation.x;
                            break;
                        case UpAxis.Y:
                            rotation.y = initialRotation.y;
                            break;
                        case UpAxis.Z:
                            rotation.z = initialRotation.z;
                            selectedObject.transform.eulerAngles = rotation;
                            break;
                        default:
                            break;
                    }
                    
                    //selectedObject.transform.rotation.SetFromToRotation(selectedObject.transform.rotation.eulerAngles, rotation);
                }
            }
        }
    }
}