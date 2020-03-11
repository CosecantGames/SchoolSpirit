using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(VisLight))]
//public class EffectRadiusEditor : Editor
//{
//    public void OnSceneGUI() {
//        VisLight t = (target as VisLight);

//        EditorGUI.BeginChangeCheck();
//        float lowLightRadius = Handles.RadiusHandle(Quaternion.identity, t.floorBelow, t.lowLightRadius);
//        float midLightRadius = Handles.RadiusHandle(Quaternion.identity, t.floorBelow, t.midLightRadius);
//        float highLightRadius = Handles.RadiusHandle(Quaternion.identity, t.floorBelow, t.highLightRadius);
//        if(EditorGUI.EndChangeCheck()) {
//            Undo.RecordObject(target, "Changed Area Of Effect");
//            t.lowLightRadius = lowLightRadius;
//            t.midLightRadius = midLightRadius;
//            t.highLightRadius = highLightRadius;
//        }
//    }
//}