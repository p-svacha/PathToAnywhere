using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform FocussedObject;

    public void FocusObject(Transform t)
    {
        FocussedObject = t;
    }

    // Update is called once per frame
    void Update()
    {
        if(FocussedObject != null)
        {
            transform.position = FocussedObject.transform.position + new Vector3(0, 0, -10);
        }
    }
}
