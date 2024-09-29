using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRoomPlacement : MonoBehaviour
{
    public GameObject room;
    public GameObject corridorPrefab;
    public GameObject simpleRoomGenerator;

    public int mapSizeX = 100;
    public int mapSizeY = 100;

    public int minRoomSize = 10;
    public int maxRoomSize = 50;
    public int roomSpace = 1;

    public int roomNumbers = 8;

    private Vector3[] roomPositions;
    private Vector3[] roomSize;
    public List<GameObject> rooms;

    // Start is called before the first frame update
    private void Start()
    {
        roomPositions = new Vector3[roomNumbers];
        roomSize = new Vector3[roomNumbers];
        CreateRoom();
    }

    private void CreateRoom()
    {
        for (int i = 0; i < roomNumbers; i++)
        {
            Vector3 position;
            Vector3 scale;
            bool validPosition;

            do
            {
                position = new Vector3(Random.Range(0, mapSizeX), 0, Random.Range(0, mapSizeY));
                // position = new Vector3(Random.Range(-(mapSizeX / 2f), mapSizeX / 2f), 0, Random.Range(-(mapSizeY / 2f), mapSizeY / 2f));
                scale = new Vector3(Random.Range(minRoomSize, maxRoomSize), 3, Random.Range(minRoomSize, maxRoomSize));
                validPosition = true;

                for (int j = 0; j < i; j++)
                {
                    Vector3 otherPosition = roomPositions[j];
                    Vector3 otherScale = roomSize[j];

                    float minDistanceX = (scale.x + otherScale.x) / 2 + roomSpace;
                    float minDistanceZ = (scale.z + otherScale.z) / 2 + roomSpace;

                    if (Mathf.Abs(position.x - otherPosition.x) < minDistanceX && Mathf.Abs(position.z - otherPosition.z) < minDistanceZ)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            roomPositions[i] = position;
            roomSize[i] = scale;

            GameObject newRoom = Instantiate(room, simpleRoomGenerator.transform);
            newRoom.name = "Room " + i;
            newRoom.transform.localScale = scale;
            newRoom.transform.localPosition = position;
            newRoom.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f));
            newRoom.tag = "Room";

            rooms.Add(newRoom);
        }
        rooms = rooms.OrderBy(x => x.transform.localPosition.x).ToList();
        CreateCorridor();
    }

    private void CreateCorridor()
    {
        var closestRoom = rooms[0];

        for (int i = 0; i < rooms.Count; i++)
        {
            var distance = 99999999999999999f;
            for (int j = 0; j < rooms.Count; j++)
            {
                if (rooms[j] == rooms[i])
                    continue;

                if (Vector3.Distance(rooms[j].transform.localPosition, closestRoom.transform.localPosition) <= distance)
                {
                    distance = Vector3.Distance(rooms[j].transform.localPosition, closestRoom.transform.localPosition);
                    closestRoom = rooms[j];
                }
            }
            
            GameObject corridor = Instantiate(corridorPrefab, simpleRoomGenerator.transform);
            corridor.name = "Corridor " + i;
            corridor.tag = "Corridor";
            
            if (rooms[i].transform.localPosition.x - closestRoom.transform.localPosition.x < rooms[i].transform.localPosition.z - closestRoom.transform.localPosition.z || closestRoom.transform.localPosition.x - rooms[i].transform.localPosition.x < rooms[i].transform.localPosition.z - closestRoom.transform.localPosition.z)
            {
                if (rooms[i].transform.localPosition.x < closestRoom.transform.localPosition.x)
                {
                    corridor.transform.localScale = new Vector3(closestRoom.transform.localPosition.x - rooms[i].transform.localPosition.x, 3, 2);
                    corridor.transform.localPosition = new Vector3(rooms[i].transform.localPosition.x + corridor.transform.localScale.x / 2, rooms[i].transform.localPosition.y, rooms[i].transform.localPosition.z);
                    corridor.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    corridor.transform.localScale = new Vector3(rooms[i].transform.localPosition.x - closestRoom.transform.localPosition.x, 3, 2);
                    corridor.transform.localPosition = new Vector3(rooms[i].transform.localPosition.x - corridor.transform.localScale.x / 2, rooms[i].transform.localPosition.y, rooms[i].transform.localPosition.z);
                    corridor.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                
            }
            else
            {
                if (rooms[i].transform.localPosition.z < closestRoom.transform.localPosition.z)
                {
                    corridor.transform.localScale = new Vector3(2, 3, closestRoom.transform.localPosition.z - rooms[i].transform.localPosition.z);
                    corridor.transform.localPosition = new Vector3(rooms[i].transform.localPosition.x, rooms[i].transform.localPosition.y, rooms[i].transform.localPosition.z + corridor.transform.localScale.z / 2);
                    corridor.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    corridor.transform.localScale = new Vector3(2, 3, rooms[i].transform.localPosition.z - closestRoom.transform.localPosition.z);
                    Debug.Log("Room actuelle : " + rooms[i].name);
                    Debug.Log("Room la plus proche : " + closestRoom.name);
                    Debug.Log(corridor.name);
                    Debug.Log(corridor.transform.localScale);
                    corridor.transform.localPosition = new Vector3(rooms[i].transform.localPosition.x, rooms[i].transform.localPosition.y, rooms[i].transform.localPosition.z - corridor.transform.localScale.z / 2);
                    corridor.GetComponent<MeshRenderer>().material.color = Color.magenta;
                }
            }
        }
    }
}
