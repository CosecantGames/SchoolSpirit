using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundTest : MonoBehaviour
{
    public Vector3 startDir = new Vector3(0f, 0f, 0f);
    public float startAngle = 0f;
    public Vector3 transformForward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        transformForward = transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, startDir * 15);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, (Quaternion.Euler(0f, 30f, 0f) * Quaternion.Euler(0f, startAngle, 0f) * startDir).normalized * 30);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, (Quaternion.Euler(0f, 30f, 0f) * Quaternion.Euler(0f, -startAngle, 0f) * startDir).normalized * 30);
    }
}
