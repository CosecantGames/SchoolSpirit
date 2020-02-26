using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player.Player;
using static Global;

namespace Player {
    public class PlrLean : MonoBehaviour {
        [Header("Lean Config")]
        public string leanInput = "Lean";
        public float leanAxis;

        public float leanAngle = 10f;
        public float leanDistance = 1f;
        public float leanSpeed = 2f;
        public float rotSpeed = 2f;

        Vector3 posTarget;
        Vector3 rotTarget;

        [Header("Wall Collision")]
        public bool baseHeadOnSphere = false;
        public float headRadius = 0.35f;
        public float distToWall;    //proportional to leanDistance; 0 = no distance to lean, 1 = can lean fully
        public bool camInsideWall;
        public float camRemovalFactor = 10f;

        public LayerMask leanLayers = 1 << 10;

        private void Start() {
            if(baseHeadOnSphere) {
                headRadius = Head.radius;
            } else {
                Head.radius = headRadius;
            }
        }

        // Update is called once per frame
        void Update() {
            leanAxis = Input.GetAxis(leanInput);

            SetDistToWall();
            CheckCamInsideWall();

            posTarget = Cam.transform.localPosition;
            posTarget.x = leanDistance * leanAxis * distToWall;

            rotTarget = Cam.transform.localEulerAngles;
            rotTarget.z = leanAngle * leanAxis * distToWall * -1;

            float totalSpeed = Time.deltaTime * leanSpeed;
            if(camInsideWall) {
                totalSpeed *= camRemovalFactor;
            }

            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, posTarget, totalSpeed);
            Cam.transform.localRotation = Quaternion.Lerp(Cam.transform.localRotation, Quaternion.Euler(rotTarget), totalSpeed);
        }

        void SetDistToWall() {
            //If ray traveling in lean direction hits wall...
            if(Physics.Raycast(Head.transform.position, Head.transform.right * leanAxis, out RaycastHit hitL, headRadius + leanDistance, leanLayers)) {
                // ...make distToWall a scalar proportional to how far from the wall the head is
                distToWall = Map(hitL.distance, headRadius, headRadius + leanDistance, 0, 1);
            } else {
                //If you didn't hit a wall, then scalar is 1 (doesn't affect leanDistance)
                distToWall = 1f;
            }
        }

        void CheckCamInsideWall() {
            camInsideWall = Physics.CheckSphere(Cam.transform.position, headRadius, leanLayers);
        }

        //private void OnDrawGizmosSelected() {
        //    Gizmos.DrawWireSphere(Cam.transform.position, headRadius);
        //}
    }
}