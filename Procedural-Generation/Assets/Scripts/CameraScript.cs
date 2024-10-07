using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{


    private Transform cameraTransform;

    [Header("Camera Positions")]
    [SerializeField] GameObject position1;
    [SerializeField] GameObject position2;
    [SerializeField] GameObject position3;
    [SerializeField] GameObject position4;
    [SerializeField] GameObject position5;

    private int index = 0;

    [Header("Methods Menu")]
    [SerializeField] GameObject perlinNoiseMenu;
    [SerializeField] GameObject perlinNoiseMenu2;
    [SerializeField] GameObject simpleroomMenu;
    [SerializeField] GameObject dlaMenu;
    [SerializeField] GameObject cellularAutomataMenu;

    [Header("Methods Game Object")]
    [SerializeField] GameObject perlinNoise;
    [SerializeField] GameObject perlinNoise2;
    [SerializeField] GameObject simpleroom;
    [SerializeField] GameObject dla;
    [SerializeField] GameObject cellularAutomata;

    List<GameObject> menus;
    List<GameObject> procGenList;

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
        menus.Add(dlaMenu);
        menus.Add(cellularAutomataMenu);

        procGenList = new List<GameObject>();
        procGenList.Add(perlinNoise);
        procGenList.Add(perlinNoise2);
        procGenList.Add(simpleroom);
        procGenList.Add(dla);
        procGenList.Add(cellularAutomata);

        SetMenus(index);
        SetActiveProcGen(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
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

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            index = 4;
            cameraTransform.position = position5.transform.position;
            cameraTransform.rotation = position5.transform.rotation;
        }

        SetMenus(index);
        SetActiveProcGen(index);
    }

    // Fonction qui rend actif le menu de la methode actuelle et desactive celles qui ne le sont pas
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

    // Fonction qui rend actif le Game Object de la methode actuelle et desactive celles qui ne le sont pas
    private void SetActiveProcGen(int index)
    {
        for (int i = 0; i < procGenList.Count; i++)
        {
            if (i == index)
            {
                procGenList[i].SetActive(true);
            }
            else procGenList[i].SetActive(false);
        }
    }
}
