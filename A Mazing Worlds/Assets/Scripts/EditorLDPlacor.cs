using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorLDPlacor : EditorWindow
{
    private GameObject snapObject;

    private bool resetRotation = true;

    [MenuItem("Tools/Snapping Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EditorLDPlacor), false, "Snapping Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Snap Object", EditorStyles.boldLabel);

        // Allow the user to select the object to snap
        snapObject = EditorGUILayout.ObjectField("Snap Object", snapObject, typeof(GameObject), true) as GameObject;
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
                            // Move the object to the collision point
                            selectedObject.transform.position = hit.point;

                            if (resetRotation)
                                selectedObject.transform.rotation = Quaternion.FromToRotation(Vector3.right, hit.normal);
                        }
                    }
                }
            }
        }
    }
}
