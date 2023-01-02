/*using UnityEngine;
using UnityEditor;

public class AssetSpawner : EditorWindow
{
    GameObject assetToSpawn;
    float assetScale = 4;
    Vector2 screenPosition;

    [MenuItem("Tools/Asset Spawner")]
    public static void ShowWindow() {
        GetWindow<AssetSpawner>("Asset Spawner");
    }

    private void OnGUI() {
        GUILayout.Label("Spawn Environmental Assets", EditorStyles.boldLabel);

        assetScale = EditorGUILayout.Slider("Scale", assetScale, .5f, 6f);

        assetToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", assetToSpawn, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn Asset")) {
            SpawnAsset();
        }
       
    }
  
    private void SpawnAsset() {
        if(assetToSpawn == null) {
            Debug.Log("Asset to spawn cannot be null");
            return;
        }

        var view = SceneView.lastActiveSceneView;

        // convert GUI coordinates to screen coordinates
        screenPosition.y = view.camera.pixelHeight / 2;
        screenPosition.x = view.camera.pixelWidth / 2;
        Ray ray = view.camera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            // instantiate object at middle of scene camere
            GameObject newObject = Instantiate(assetToSpawn, hit.point, Quaternion.identity);
            Undo.RegisterCreatedObjectUndo(newObject, "Instantiated Environment Object");

            //scale object
            newObject.transform.localScale *= assetScale;

            //all of the environment objects are rotated 90 degrees so fix it
            newObject.transform.position = new Vector3(newObject.transform.position.x, 0, newObject.transform.position.z);
            newObject.transform.Rotate(-90,0,0);


            //check if environment folder exists
            GameObject environment = GameObject.Find("Environment");
            if (environment != null) {
                //it exists
                newObject.transform.SetParent(environment.transform);
            } else {
                //folder doesnt exist, create one
                GameObject Environment = new GameObject("Environment");
                Undo.RegisterCreatedObjectUndo(newObject, "Created environemtn folder");

                newObject.transform.SetParent(Environment.transform);
            }
        }
    }
}
*/