using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DLACreate : MonoBehaviour
{
    public Transform parent;
    public GameObject inputPrefab;
    List<GameObject> allPrefabs = null;
    public int DLAgrowthcount = 1000;
    private GameObject activePrefab = null;
    private GameObject DLAGenerator = null;
    public float Seconds = 0;
    public bool is2D = false;
    public bool Horizontal = false;

    [SerializeField] GameObject DLAscript;


    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    private void Update()
    {

    }

    public IEnumerator RunDLA()
    {
        for (int i = 0; i < DLAgrowthcount; i++)
        {
            yield return new WaitForSeconds(Seconds);
            AddDLAAgent();
        }
    }

    public void SetupScene()
    {
        if (allPrefabs != null)
        {
            if (allPrefabs.Count > 0)
            {
                foreach (var pf in allPrefabs)
                {
                    Destroy(pf);
                }
                allPrefabs.Clear();
            }
        }

        allPrefabs = new List<GameObject>();
        GameObject newPrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
        newPrefab.transform.SetParent(parent, true);
        allPrefabs.Add(newPrefab);

        if (activePrefab == null)
        {
            activePrefab = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
            activePrefab.transform.SetParent(parent, true);
            activePrefab.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            activePrefab.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.15f, 0.15f);
        }
        activePrefab.transform.position = newPrefab.transform.position;

        if (DLAGenerator == null)
        {
            DLAGenerator = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
            DLAGenerator.transform.SetParent(parent, true);
            DLAGenerator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            DLAGenerator.GetComponent<MeshRenderer>().material.color = new Color(1.15f, 0.15f, 1.0f);
        }
    }

    void AddDLAAgent()
    {
        if (activePrefab != null)
        {
            Vector3 DLADirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 15;

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

    private GameObject GetClosestGeo(Vector3 testerVec, List<GameObject> allPrefabs)
    {
        GameObject closestGeo = null;
        if (allPrefabs.Count > 0)
        {
            float closestDistance = 1000f;
            for (int i = 0; i < allPrefabs.Count; i++)
            {
                GameObject currentGeo = allPrefabs[i];
                float dist = Vector3.Distance(testerVec, currentGeo.transform.position);
                if (dist < closestDistance && dist > 0.5f)
                {
                    closestGeo = currentGeo;
                    closestDistance = dist;
                }
            }
        }
        return closestGeo;
    }

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


