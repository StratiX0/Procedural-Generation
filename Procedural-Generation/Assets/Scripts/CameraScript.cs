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

    private int index = 0;

    [SerializeField] GameObject perlinNoiseMenu;
    [SerializeField] GameObject perlinNoiseMenu2;
    [SerializeField] GameObject simpleroomMenu;
    [SerializeField] GameObject cellularAutomataMenu;

    List<GameObject> menus;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>();
        cameraTransform.position = position1.transform.position;
        cameraTransform.rotation = position1.transform.rotation;

        menus = new List<GameObject>();

        menus.Add(perlinNoiseMenu);
        menus.Add(perlinNoiseMenu2);
        menus.Add(simpleroomMenu);
        menus.Add(cellularAutomataMenu);

        SetMenus(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
            perlinNoiseMenu.SetActive(true);
            cameraTransform.position = position1.transform.position;
            cameraTransform.rotation = position1.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
            cameraTransform.position = position2.transform.position;
            cameraTransform.rotation = position2.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            cameraTransform.position = position3.transform.position;
            cameraTransform.rotation = position3.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
            cameraTransform.position = position4.transform.position;
            cameraTransform.rotation = position4.transform.rotation;
        }

        SetMenus(index);
    }

    private void SetMenus(int index)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == index)
            {
                menus[i].SetActive(true);
            }
            else menus[i].SetActive(false);
        }
    }
}
