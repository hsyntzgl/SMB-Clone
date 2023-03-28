#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Pipe))]
[CanEditMultipleObjects]
public class PipeCustomInspector : Editor
{
    SerializedProperty activePipe;
    Transform secretZoneEntranceTransform;
    GameObject exitPipeGameObject;

    Transform secretZoneCameraTransform;

    Pipe.PipeType pipeType;

    Pipe pipeScript;

    bool guiEnabled = true;

    string buttonText = "Save";

    private void OnEnable()
    {
        activePipe = serializedObject.FindProperty("activePipe");
        pipeScript = (Pipe)target;

    }
    public override void OnInspectorGUI()
    {
        activePipe.boolValue = pipeScript.activePipe;

        activePipe.boolValue = EditorGUILayout.Toggle("Active Pipe", activePipe.boolValue);

        if (activePipe.boolValue)
        {
            pipeScript.activePipe = activePipe.boolValue;

            pipeType = pipeScript.pipeType;

            pipeType = (Pipe.PipeType)EditorGUILayout.EnumPopup("Pipe Type", pipeType);
            pipeScript.pipeType = pipeType;

            switch (pipeType)
            {
                case Pipe.PipeType.EntrancePipeExitPipe:
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Exit Pipe");

                    exitPipeGameObject = pipeScript.exitPipeGameObject;

                    exitPipeGameObject = (GameObject)EditorGUILayout.ObjectField(exitPipeGameObject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();

                    pipeScript.exitPipeGameObject = exitPipeGameObject;

                    EditorUtility.SetDirty(pipeScript);
                    break;
                case Pipe.PipeType.EntrancePipeExitSecretZone:
                    EditorGUILayout.BeginHorizontal();

                    secretZoneEntranceTransform = pipeScript.secretZoneEntranceTransform;

                    GUILayout.Label("Secret Zone Entrance Transform");
                    secretZoneEntranceTransform = (Transform)EditorGUILayout.ObjectField(secretZoneEntranceTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();

                    secretZoneCameraTransform = pipeScript.secretZoneCameraTransform;

                    GUILayout.Label("Secret Zone Camera Transform");
                    secretZoneCameraTransform = (Transform)EditorGUILayout.ObjectField(secretZoneCameraTransform, typeof(Transform), true);
                    EditorGUILayout.EndHorizontal();
                    pipeScript.secretZoneEntranceTransform = secretZoneEntranceTransform;
                    pipeScript.secretZoneCameraTransform = secretZoneCameraTransform;

                    EditorUtility.SetDirty(pipeScript);
                    break;
                case Pipe.PipeType.ExitPipe:
                    EditorUtility.SetDirty(pipeScript);
                    break;
            }
            GUI.enabled = guiEnabled;

            if (GUILayout.Button(buttonText))
            {
                EditorUtility.SetDirty(pipeScript);
                EditorSceneManager.MarkAllScenesDirty();

                guiEnabled = false;
                buttonText = "Saved!";
            }
        }
        else
        {
            pipeScript.activePipe = activePipe.boolValue;

            EditorUtility.SetDirty(pipeScript);
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
#endif