using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisLight))]
public class EffectRadiusEditor : Editor
{
    public void OnSceneGUI() {
        VisLight t = (target as VisLight);

        EditorGUI.BeginChangeCheck();
        float minLightRadius = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.minLightRadius);
        float maxLightRadius = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.maxLightRadius);
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Changed Area Of Effect");
            t.minLightRadius = minLightRadius;
            t.maxLightRadius = maxLightRadius;
        }
    }
}