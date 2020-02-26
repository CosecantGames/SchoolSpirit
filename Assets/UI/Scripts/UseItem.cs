using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    RectTransform itemUsedPos;
    Vector3 itemUsedNewPos;
    private Vector3 itemUsedVelocity = Vector3.zero;
    public float speed = 0.5f;
    bool usePressed;
    public string useInput = "Use";

    // Start is called before the first frame update
    void Start()
    {
        itemUsedPos = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        usePressed = Input.GetButtonDown(useInput);

        itemUsedNewPos = new Vector3(-10.9f, -5.5f);
        if(usePressed)
        {
            itemUsedPos.localPosition = Vector3.SmoothDamp(itemUsedPos.localPosition, itemUsedNewPos, ref itemUsedVelocity, speed);
        }
    }
}
