using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRoomPlacement : MonoBehaviour
{
    public GameObject roomPrefab;
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
    public List<GameObject> roomsOrdered;

    // Start is called before the first frame update
    private void Start()
    {
        roomPositions = new Vector3[roomNumbers];
        roomSize = new Vector3[roomNumbers];
        CreateRoom();
    }

    // Creer des salles qui ne se chevauchent pas avec une distance minimale
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

            GameObject newRoom = Instantiate(roomPrefab, simpleRoomGenerator.transform);
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

    // Creer des couloirs vers la salle la plus proche, pas parfait mais suffisant pour une démo
    private void CreateCorridor()
    {
        if (rooms == null) return;

        HashSet<GameObject> connected = new HashSet<GameObject>();

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject room = rooms[i];

            Dictionary<int, float> roomDistance = new Dictionary<int, float>();

            Vector3 point = room.transform.localPosition;

            for (int j = 0; j < rooms.Count - 1; j++)
            {
                if (i == j) continue;

                if (connected.Contains(rooms[j])) continue;

                roomDistance.Add(j, Vector3.Distance(rooms[j].transform.localPosition, point));

            }

            if (roomDistance.Count <= 0) continue;
            
            else
            {
                connected.Add(room);
                roomDistance.OrderBy(v => v.Value);
                var room2 = rooms[roomDistance.Keys.ElementAt(0)];
                Debug.Log("Room actuelle = " + room.name + " Room la plus proche = " + room2.name);

                GameObject corridor = Instantiate(roomPrefab, simpleRoomGenerator.transform);
                corridor.name = "Corridor A - " + i;
                corridor.tag = "Corridor";
                
                Vector3 point2 = room2.transform.localPosition;
                
                if (point.x - point2.x < point.z - point2.z)
                {
                    corridor.transform.localScale =  new Vector3(Mathf.Abs(point2.x - point.x), 1, 4);

                    corridor.transform.localPosition = new Vector3(point.x < point2.x ? point.x + corridor.transform.localScale.x / 2 : point.x - corridor.transform.localScale.x / 2, point.y, point.z);
                    
                    corridor.GetComponent<MeshRenderer>().material.color = Color.green;

                    if (corridor.transform.localPosition.z + corridor.transform.localScale.z / 2 > (room2.transform.localPosition.z + room2.transform.localScale.z / 2) || corridor.transform.localPosition.z - corridor.transform.localScale.z / 2 < (room2.transform.localPosition.z - room2.transform.localScale.z / 2))
                    {
                        GameObject corridor2 = Instantiate(roomPrefab, simpleRoomGenerator.transform);
                        corridor2.name = "Corridor B - " + i;
                        corridor2.tag = "Corridor";
                        
                        corridor2.transform.localScale =  new Vector3(4, 1, Mathf.Abs(point2.z > point.z ? point2.z - point.z : point.z - point2.z));
                    
                        corridor2.transform.localPosition = new Vector3(point.x < point2.x ? corridor.transform.localPosition.x + corridor.transform.localScale.x / 2 : corridor.transform.localPosition.x - corridor.transform.localScale.x / 2, point.y, point.z > point2.z ? point.z - corridor2.transform.localScale.z / 2 : point2.z - corridor2.transform.localScale.z / 2);
                    
                        corridor2.GetComponent<MeshRenderer>().material.color = Color.magenta;
                    }
                }
                else
                {
                    corridor.transform.localScale = new Vector3(4, 1, Mathf.Abs(point2.z > point.z ? point2.z - point.z : point.z - point2.z));
                    
                    corridor.transform.localPosition = new Vector3(point.x, point.y, point.z + corridor.transform.localScale.z / 2);
                    
                    corridor.GetComponent<MeshRenderer>().material.color = Color.red;

                    if (corridor.transform.localPosition.x + corridor.transform.localScale.x / 2 < (room2.transform.localPosition.x - room2.transform.localScale.x / 2) ||
                        corridor.transform.localPosition.x - corridor.transform.localScale.x / 2 > (room2.transform.localPosition.x + room2.transform.localScale.x / 2))
                    {
                        GameObject corridor2 = Instantiate(roomPrefab, simpleRoomGenerator.transform);
                        corridor2.name = "Corridor B - " + i;
                        corridor2.tag = "Corridor";
                        
                        corridor2.transform.localScale =  new Vector3(Mathf.Abs(point2.x > point.x ? point2.x - point.x : point.x - point2.x), 1, 4);
                    
                        corridor2.transform.localPosition = new Vector3(point.x > point2.x ? point.x - corridor2.transform.localScale.x / 2 : point2.x - corridor2.transform.localScale.x / 2, point.y, corridor.transform.localPosition.z + corridor.transform.localScale.z / 2);
                    
                        corridor2.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                }
            }
        }
    }
    
    
}
