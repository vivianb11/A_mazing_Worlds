using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorLDPlacor : EditorWindow
{
    private GameObject snapObject;
    private float snapOffset = 0;
    private bool resetRotation = true;

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
        resetRotation = EditorGUILayout.Toggle("Rotate to Normal", resetRotation);

        if (GUILayout.Button("Snap"))
        {
            if (snapObject != null && Selection.gameObjects.Length > 0)
            {
                Vector3 center = snapObject.transform.position;

                foreach (GameObject selectedObject in Selection.gameObjects)
                {
                    if (selectedObject != snapObject)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(selectedObject.transform.position, center - selectedObject.transform.position, out hit, Mathf.Infinity))
                        {
                            Debug.DrawLine(selectedObject.transform.position, hit.point, Color.red, 0.5f);

                            // Move the object to the collision point
                            selectedObject.transform.position = hit.point;

                            // rotate the object so it has the same up as the normal by doing the less changes possible to every other axis
                            if (resetRotation)
                            {
                                float initialZ = selectedObject.transform.eulerAngles.z;

                                selectedObject.transform.forward = hit.normal;
                                Vector3 rotation = selectedObject.transform.eulerAngles;
                                rotation.z = initialZ;
                                selectedObject.transform.eulerAngles = rotation;
                            }
                        }
                    }
                }
            }
        }
    }
}
