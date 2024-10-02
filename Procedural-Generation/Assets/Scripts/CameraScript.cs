using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform cameraTransform;

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>();
        cameraTransform.position = position1.transform.position;
        cameraTransform.rotation = position1.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraTransform.position = position1.transform.position;
            cameraTransform.rotation = position1.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cameraTransform.position = position2.transform.position;
            cameraTransform.rotation = position2.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cameraTransform.position = position3.transform.position;
            cameraTransform.rotation = position3.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            cameraTransform.position = position4.transform.position;
            cameraTransform.rotation = position4.transform.rotation;
        }
    }
}
