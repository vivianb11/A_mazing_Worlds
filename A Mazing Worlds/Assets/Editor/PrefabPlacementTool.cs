using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabPlacementTool : EditorWindow
{
    private GameObject prefab;
    private GameObject previewObject;
    private Material previewMaterial;
    private bool isPlacing = false;

    [MenuItem("Tools/Prefab Placement Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabPlacementTool));
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
        CreatePreviewMaterial();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
        DestroyPreviewMaterial();
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Placement Tool", EditorStyles.boldLabel);

        prefab = EditorGUILayout.ObjectField("Prefab:", prefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Place Prefab"))
        {
            isPlacing = true;
        }

        if (Event.current.type == EventType.Layout && isPlacing)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        if (isPlacing)
        {
            HandleUtility.Repaint();

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (previewObject == null)
                {
                    previewObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    previewObject.hideFlags = HideFlags.HideAndDontSave;
                }

                // Set the position of the preview object
                previewObject.transform.position = hit.point;

                // Align the up direction with the surface normal
                previewObject.transform.up = hit.normal;

                // Draw transparent plane as preview
                DrawTransparentPlane(hit.point, previewObject.transform.rotation);

                // Handle mouse events
                HandleMouseEvents(hit.point);

                Event.current.Use();
            }
        }
    }

    private void CreatePreviewMaterial()
    {
        Shader shader = Shader.Find("Standard");
        previewMaterial = new Material(shader);
        previewMaterial.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void DestroyPreviewMaterial()
    {
        if (previewMaterial != null)
        {
            DestroyImmediate(previewMaterial);
        }
    }

    private void DrawTransparentPlane(Vector3 position, Quaternion rotation)
    {
        Handles.DrawSolidRectangleWithOutline(
            new Vector3[]
            {
                position + rotation * new Vector3(-1f, 0f, -1f),
                position + rotation * new Vector3(1f, 0f, -1f),
                position + rotation * new Vector3(1f, 0f, 1f),
                position + rotation * new Vector3(-1f, 0f, 1f),
            },
            previewMaterial.color,
            Color.clear
        );
    }

    private void HandleMouseEvents(Vector3 position)
    {
        float rotationSpeed = 5f;

        if (Event.current.type == EventType.ScrollWheel)
        {
            float scrollDelta = Event.current.delta.y;
            float rotationAmount = scrollDelta * rotationSpeed;

            // Get the rotation axis by projecting the world up vector onto the object's normal
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, previewObject.transform.up);

            // Rotate the object around the calculated axis
            previewObject.transform.Rotate(rotationAxis, rotationAmount, Space.World);
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // Place the prefab
            GameObject placedObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(placedObject, "Place Prefab");
            placedObject.transform.position = position;
            placedObject.transform.rotation = previewObject.transform.rotation;

            isPlacing = false;
            DestroyImmediate(previewObject);
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            // Cancel placement
            isPlacing = false;
            DestroyImmediate(previewObject);
        }
    }

}
