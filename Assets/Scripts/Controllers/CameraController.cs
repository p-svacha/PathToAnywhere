using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform FocussedObject;

    // Zoom
    private float CurrentZoom = 8f;
    private float MinZoom = 4f;
    private float MaxZoom = 20f;
    private float ZoomSpeed = 200f;

    public void FocusObject(Transform t)
    {
        FocussedObject = t;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            CurrentZoom -= Input.mouseScrollDelta.y * ZoomSpeed * Time.deltaTime;
            CurrentZoom = Mathf.Clamp(CurrentZoom, MinZoom, MaxZoom);
            GetComponent<Camera>().orthographicSize = CurrentZoom;
        }

        if(FocussedObject != null)
        {
            transform.position = FocussedObject.transform.position + new Vector3(0, 0, -10);
        }
    }
}
