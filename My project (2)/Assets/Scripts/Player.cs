using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public DLACreate dlaCreate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var MousePosition = Input.mousePosition;
            var Ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.Raycast(Ray, out hit))
            {
                var CurrentObject = hit.collider.gameObject;
                dlaCreate.AddOnePrefab(CurrentObject, hit.normal.normalized);
                StartCoroutine(dlaCreate.RunDLA());
            }
        }

    }
}
