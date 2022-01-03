using UnityEditor;
[CustomEditor(typeof(TutorialElement), true)]
public class TutorialElementEditor : Editor {
    TutorialElement te;

    void OnEnable()
    {
        te = (TutorialElement) target;
    }
    public override void OnInspectorGUI() 
    {
        serializedObject.Update();
        if(te.lightController)
        {
            for(int i = 0; i < te.lightPositions.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EndHorizontal();
            }

            for(int i = 0; i < te.scripts.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EndHorizontal();
            }
            if(te.scriptPosController)
            {
                DrawDefaultInspector();
            }
            else
            {
                DrawPropertiesExcluding(serializedObject, "scriptPositions");
            }
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, "lightPositions", "scripts", "scriptText", "globalLightIntensity", 
                                                    "pointLightIntensity", "pointLightInnerRadius", "pointLightOuterRadius", "scriptPositions");
        }
        serializedObject.ApplyModifiedProperties();
    }
}
