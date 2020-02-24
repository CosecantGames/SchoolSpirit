using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player {
    public class PlrVisibility : MonoBehaviour {
        public GameObject target;
        public List<GameObject> hitList;

        public Player player;
        public TextMeshProUGUI visMeter;

        [Range(0f, 1f)]
        public float vis;

        private void Awake() {
            player = GetComponent<Player>();
        }

        // Start is called before the first frame update
        void Start() {
            visMeter = GameObject.Find("VisMeter").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update() {
            visMeter.text = "Visibility: " + FormatVis() + "%";

            if(Input.GetKeyDown(KeyCode.T)) {
                SeeTest();
            }
        }

        float FormatVis() {
            float prettyVis = vis;
            if(prettyVis > 1) {
                prettyVis = 1;
            }

            prettyVis = Mathf.Round(vis * 100f);

            return prettyVis;
        }

        void FindLights() {
            GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        }

        void SeeTest() {
            hitList.Clear();
            Debug.Log(this.sees(target));
            RaycastHit[] hits = Physics.RaycastAll(
                transform.position,
                target.transform.position - transform.position,
                (target.transform.position - transform.position).magnitude,
                ~6148);

            foreach(RaycastHit hit in hits) {
                hitList.Add(hit.transform.gameObject);
            }
        }
    }
}