using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StealthLight))]
public class EffectRadiusEditor : Editor {
    public void OnSceneGUI() {
        StealthLight t = (target as StealthLight);

        EditorGUI.BeginChangeCheck();
        float rNear = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.rNear);
        float rMid = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.rMid);
        float rFar = Handles.RadiusHandle(Quaternion.identity, t.transform.position, t.rFar);
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Changed Area Of Effect");
            t.rNear = rNear;
            t.nearLight.range = rNear;

            t.rMid = rMid;
            t.midLight.range = rMid;

            t.rFar = rFar;
            t.farLight.range = rFar;

        }
    }
}