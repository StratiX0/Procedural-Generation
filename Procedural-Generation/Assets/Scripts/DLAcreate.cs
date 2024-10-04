using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DLACreate : MonoBehaviour
{
    //Instantiate variable
    public Transform parent;
    public GameObject inputPrefab;
    private GameObject activePrefab = null;
    private GameObject DLAGenerator = null;
    List<GameObject> allPrefabs = null;
    public int DLAgrowthcount = 1000;
    public float Seconds = 0;
    public bool is2D = false;
    public bool Horizontal = false;

    [SerializeField] GameObject DLAscript;


    // Start is called before the first frame update
    void Start()
    {
        //Restart the Scene
        SetupScene();
    }

    //Update is called constantly
    private void Update()
    {

    }

    //Time to wait between each cube spawn
    public IEnumerator RunDLA()
    {
        for (int i = 0; i < DLAgrowthcount; i++)
        {
            yield return new WaitForSeconds(Seconds);
            AddDLAAgent();
        }
    }

    //
    public void SetupScene()
    {
        //Search if there is any prefabs in allPrefabs List
        if (allPrefabs != null)
        {
            if (allPrefabs.Count > 0)
            {
                //Destroy every prefab
                foreach (var pf in allPrefabs)
                {
                    Destroy(pf);
                }
                allPrefabs.Clear();
            }
        }

        //Create the main prefab in center an add it to the list.
        allPrefabs = new List<GameObject>();
        GameObject newPrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
        //Set parent GameObject as Parent of the newPrefab
        newPrefab.transform.SetParent(parent, true);
        allPrefabs.Add(newPrefab);

        //Create a Prefab "Hover" to see what is the last prefab whiwh has spawn.
        if (activePrefab == null)
        {
            activePrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
            //Set parent GameObject as Parent of the newPrefab
            activePrefab.transform.SetParent(parent, true);
            activePrefab.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            //Set color red
            activePrefab.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.15f, 0.15f);
        }
        activePrefab.transform.position = newPrefab.transform.position;

        //Spawn every cube create by the generator (fonction create after).
        if (DLAGenerator == null)
        {
            DLAGenerator = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
            //Set parent GameObject as Parent of the newPrefab
            DLAGenerator.transform.SetParent(parent, true);
            DLAGenerator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //Set the color pink.
            DLAGenerator.GetComponent<MeshRenderer>().material.color = new Color(1.15f, 0.15f, 1.0f);
        }
    }

    //Create cubes randomly.
    void AddDLAAgent()
    {
        if (activePrefab != null)
        {
            //Define Random vector.
            Vector3 DLADirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 15;

            //If there is less prefab than it should be, define the Direction and Create a cube then attach it to the closestgeo.
            if (allPrefabs.Count < DLAgrowthcount)
            {
                Vector3 DLAtester = DLADirection;
                if (is2D == false)
                {

                }
                else if (is2D == true)
                {
                    if (Horizontal == true)
                    {
                        DLAtester.x = 0;
                    }
                    else if (Horizontal == false)
                    {
                        DLAtester.y = 0;
                    }
                }
                DLAGenerator.transform.position = DLADirection;

                GameObject closestGeo = GetClosestGeo(DLAtester, allPrefabs);
                Vector3 growthDir = DLAtester - closestGeo.transform.position;
                AddOnePrefab(closestGeo, growthDir);
            }
        }
    }

    //Find the closest Prefab.
    private GameObject GetClosestGeo(Vector3 testerVec, List<GameObject> allPrefabs)
    {
        GameObject closestGeo = null;

        if (allPrefabs.Count > 0)
        {
            float closestDistance = 1000f;

            for (int i = 0; i < allPrefabs.Count; i++)
            {
                GameObject currentGeo = allPrefabs[i];
                //Vector of the distance between testerVec and any prefab.
                float dist = Vector3.Distance(testerVec, currentGeo.transform.position);
                //If the new distance is smaller but superior at 0.5f, then update the closestGeo Gameobject and closestDistance.
                if (dist < closestDistance && dist > 0.5f)
                {
                    closestGeo = currentGeo;
                    closestDistance = dist;
                }
            }
        }
        return closestGeo;
    }

    //Add one prefab at a position define in the function, and update the position of activePrefab.
    public void AddOnePrefab(GameObject testGameObj, Vector3 growDir)
    {
        growDir.Normalize();

        Vector3 newPos = testGameObj.transform.position + growDir;
        GameObject newPrefab = Instantiate(inputPrefab, newPos, Quaternion.identity);
        allPrefabs.Add(newPrefab);
        newPrefab.transform.SetParent(parent, true);

        activePrefab.transform.position = newPos;
    }



    //UI----------------------------


    public void SetAgrowthcount(float value)
    {
        DLAgrowthcount = Mathf.RoundToInt(value);
    }

    public void SetSeconds(float value)
    {
        Seconds = value;
    }

    public void is2Dactive()
    {
        is2D = !is2D;
    }

    public void Horizontalactive()
    {
        Horizontal = !Horizontal;
    }
}


