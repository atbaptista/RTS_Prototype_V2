/*using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{
    Color color;

    [MenuItem("Tools/Colorizer")]
    public static void ShowWindow() {
        GetWindow<ExampleWindow>("Colorizer");
    }

    void OnGUI() {
        GUILayout.Label("Colorize", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if(GUILayout.Button("Colorize Selected!")) {
            Colorize();
        }
        if (GUILayout.Button("Colorize Only Dinos!")) {
            ColorizeDinos();
        }
    }

    void Colorize() {
        foreach (GameObject obj in UnityEditor.Selection.gameObjects) {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();

            if (renderer != null) {
                Undo.RecordObject(renderer.sharedMaterial, "change color");
                renderer.sharedMaterial.color = color;
            }
        }
    }

    void ColorizeDinos() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Dino")) {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();

            if (renderer != null) {
                Undo.RecordObject(renderer.sharedMaterial, "change color");
                renderer.sharedMaterial.color = color;
            }
        }
    }
}
*/