﻿using System.Collections;
using UnityEngine;

/*******************************************************
 * Class:           AIConeDetection
 * Description:     Description begin here
 * 
 * Studio Leaves (c) 
 *******************************************************/
public class AIConeDetection : MonoBehaviour {

    public Vector3 coneOrigin;
    public float coneHeight;

    /* Fov Properties */
    public float m_fConeLength = 5.0f;
    public float m_fAngleOfView = 90.0f;
    public float m_vStartDistanceCone = 2.0f;
    public Material m_matVisibilityCone = null;
    public bool m_bHasStartDistance = true;
    public LayerMask m_MaskLayerToIgnore;
    private float m_fFixedCheckNextTime;

    /* Render Properties */
    public bool m_bShowCone = true;
    public int m_iConeVisibilityPrecision = 3;
    //public  float       m_fDistanceForRender        = 600.0f;

    private Mesh m_mConeMesh;
    private Vector3[] m_vVertices;
    private Vector2[] m_vUV;
    private Vector3[] m_vNormals;
    private int[] m_iTriangles;
    private GameObject m_goVisibilityCone = null;
    private int m_iVertMax = 120;
    private int m_iTrianglesMax = 120;

    private float m_fSpan;
    private float m_fStartRadians;
    private float m_fCurrentRadians;
    //private float 		m_fConeLengthFixed;

    private ArrayList m_goGameObjectIntoCone;
    public ArrayList GameObjectIntoCone => m_goGameObjectIntoCone;

    void Start() {
        InitAIConeDetection();
    }

    void Update() {
        DrawVisibilityCone();
    }

    private void InitAIConeDetection() {
        m_goGameObjectIntoCone = new ArrayList();
        m_goVisibilityCone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(m_goVisibilityCone.GetComponent<BoxCollider>());

        m_goVisibilityCone.name = name + "_VisConeMesh";
        m_mConeMesh = new Mesh();
        m_goVisibilityCone.GetComponent<MeshFilter>().mesh = m_mConeMesh;

        m_iVertMax = m_iConeVisibilityPrecision * 2 + 2;
        m_iTrianglesMax = m_iConeVisibilityPrecision * 2;

        m_vVertices = new Vector3[m_iVertMax];
        m_iTriangles = new int[m_iTrianglesMax * 3];
        m_vNormals = new Vector3[m_iVertMax];

        m_mConeMesh.vertices = m_vVertices;
        m_mConeMesh.triangles = m_iTriangles;
        m_mConeMesh.normals = m_vNormals;

        m_vUV = new Vector2[m_mConeMesh.vertices.Length];
        m_mConeMesh.uv = m_vUV;

        m_goVisibilityCone.GetComponent<Renderer>().material = m_matVisibilityCone;

        for(int i = 0; i < m_iVertMax; ++i) {
            m_vNormals[i] = Vector3.up;
        }

        m_fStartRadians = (360.0f - (m_fAngleOfView)) * Mathf.Deg2Rad;
        m_fCurrentRadians = m_fStartRadians;
        m_fSpan = (m_fAngleOfView) / m_iConeVisibilityPrecision;
        m_fSpan *= Mathf.Deg2Rad;
        m_fSpan *= 2.0f;
        m_fAngleOfView *= 0.5f;
    }

    public void DisableCone() {
        m_mConeMesh.Clear();
    }

    private RaycastHit m_rcInfo;
    private Ray m_rayDir = new Ray();

    private void DrawVisibilityCone() {
        coneOrigin = transform.position;
        coneOrigin += new Vector3(0, coneHeight, 0);

        m_goGameObjectIntoCone.Clear();
        m_fCurrentRadians = m_fStartRadians;
        Vector3 CurrentVector = transform.forward;
        Vector3 DrawVectorCurrent = transform.forward;

        int index = 0;
        for(int i = 0; i < m_iConeVisibilityPrecision + 1; ++i) {
            float newX = CurrentVector.x * Mathf.Cos(m_fCurrentRadians) - CurrentVector.z * Mathf.Sin(m_fCurrentRadians);
            float newZ = CurrentVector.x * Mathf.Sin(m_fCurrentRadians) + CurrentVector.z * Mathf.Cos(m_fCurrentRadians);
            //float newY = CurrentVector.y * Mathf.Sin( m_fCurrentRadians ) + CurrentVector.z * Mathf.Cos( m_fCurrentRadians );

            DrawVectorCurrent.x = newX;
            DrawVectorCurrent.y = 0.0f;
            DrawVectorCurrent.z = newZ;

            //float Angle       = 90.0f;
            //DrawVectorCurrent = Quaternion.Euler( 0.0f, 0.0f, Angle ) * DrawVectorCurrent;

            m_fCurrentRadians += m_fSpan;

            /* Calcoliamo dove arriva il Ray */
            float FixedLength = m_fConeLength;
            bool bFoundWall = false;
            /* Adattiamo la mesh alla superfice sulla quale tocca */

            m_rayDir.origin = coneOrigin;
            m_rayDir.direction = DrawVectorCurrent.normalized;

            if(Physics.Raycast(m_rayDir, out m_rcInfo, Mathf.Infinity, m_MaskLayerToIgnore)) {
                if(m_rcInfo.distance < m_fConeLength) {
                    bFoundWall = true;
                    FixedLength = m_rcInfo.distance;

                    bool bGOFound = false;
                    foreach(GameObject go in m_goGameObjectIntoCone) {
                        if(go.GetInstanceID() == m_rcInfo.collider.gameObject.GetInstanceID()) {
                            bGOFound = true;
                            break;
                        }
                    }
                    if(!bGOFound) {
                        m_goGameObjectIntoCone.Add(m_rcInfo.collider.gameObject);
                    }
                }
            }

            if(m_bHasStartDistance) {
                m_vVertices[index] = coneOrigin + DrawVectorCurrent.normalized * m_vStartDistanceCone;
            } else {
                m_vVertices[index] = coneOrigin;
            }

            m_vVertices[index + 1] = coneOrigin + DrawVectorCurrent.normalized * FixedLength;
            //m_vVertices[ index + 1 ].y  = coneOrigin.y;

            Color color;
            if(bFoundWall) {
                color = Color.red;
            } else {
                color = Color.yellow;
            }

            Debug.DrawLine(coneOrigin, m_vVertices[index + 1], color);
            index += 2;
        }

        if(m_bShowCone) {
            int localIndex = 0;
            for(int j = 0; j < m_iTrianglesMax * 3; j = j + 6) {
                m_iTriangles[j] = localIndex;
                m_iTriangles[j + 1] = localIndex + 3;
                m_iTriangles[j + 2] = localIndex + 1;

                m_iTriangles[j + 3] = localIndex + 2;
                m_iTriangles[j + 4] = localIndex + 3;
                m_iTriangles[j + 5] = localIndex;

                localIndex += 2;
            }

            m_mConeMesh.Clear();
            m_mConeMesh.vertices = m_vVertices;
            m_mConeMesh.triangles = m_iTriangles;
            m_mConeMesh.normals = m_vNormals;
            m_mConeMesh.RecalculateNormals();
            ;
        } else {
            m_mConeMesh.Clear();
        }

    }
}
