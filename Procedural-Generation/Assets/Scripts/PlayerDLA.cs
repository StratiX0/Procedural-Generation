using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public DLACreate dlaCreate;

    // Update is called once per frame
    void Update()
    {
        //If the mouse bouton is clickedand start the coroutine.
        if (Input.GetMouseButtonDown(0))
        {
            //Find where it is clicked
            RaycastHit hit;
            var MousePosition = Input.mousePosition;
            var Ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.Raycast(Ray, out hit))
            {
                //If it is on the existant Prefab 
                var CurrentObject = hit.collider.gameObject;
                //Add a cube
                dlaCreate.AddOnePrefab(CurrentObject, hit.normal.normalized);
                //Start coroutine (DLA Algorithme)
                StartCoroutine(dlaCreate.RunDLA());
            }
        }

    }
}
