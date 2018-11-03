using UnityEngine;
using UnityEditor;
using Minesweeper;

namespace Minesweeper {
    [CustomEditor(typeof(Minefield))]
    public class EditorMinefield : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            Minefield field = (Minefield)target;

            EditorGUILayout.Space();
            field.MinefieldSize = EditorGUILayout.Vector3Field("Minefield Size", field.MinefieldSize);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Minefield"))
                field.CreateMinefield();
            if (GUILayout.Button("Reset Minefield"))
                field.DestroyMinefield();
            EditorGUILayout.EndHorizontal();
        }

    }
}