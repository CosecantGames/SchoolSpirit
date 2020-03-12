//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(VisLight))]
//public class EffectRadiusEditor : Editor {
//    public void OnSceneGUI() {
//        VisLight t = (target as VisLight);

//        EditorGUI.BeginChangeCheck();
//        float rNear = Handles.RadiusHandle(Quaternion.identity, t.anchorPoint, t.rNear);
//        float rMid = Handles.RadiusHandle(Quaternion.identity, t.anchorPoint, t.rMid);
//        float rFar = Handles.RadiusHandle(Quaternion.identity, t.anchorPoint, t.rFar);
//        if(EditorGUI.EndChangeCheck()) {
//            Undo.RecordObject(target, "Changed Area Of Effect");
//            t.rNear = rNear;
//            t.rMid = rMid;
//            t.rFar = rFar;
//            t.light.range = rFar;
//            t.FindFloorBelow();
//        }
//    }
//}