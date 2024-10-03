using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRoomPlacement : MonoBehaviour
{
    public bool createNewRooms = false;
    public float timeToCreate = 0.1f;

    public GameObject simpleRoomGenerator;
    public GameObject roomPrefab;
    public GameObject roomGenerator;
    public GameObject corridorGenerator;

    public int minMapSize = 100;
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


    [SerializeField] GameObject timeText;
    [SerializeField] GameObject MapSizeXText;
    [SerializeField] GameObject MapSizeYText;
    [SerializeField] GameObject minRoomSizeText;
    [SerializeField] GameObject maxRoomSizeText;
    [SerializeField] GameObject roomSpaceText;
    [SerializeField] GameObject roomNumbersText;

    // Start is called before the first frame update
    private void Start()
    {
        InitSimpleRoom();
    }

    private void Update()
    {
        if (createNewRooms)
        {
            InitSimpleRoom();
            createNewRooms = false;
        }
    }

    private void InitSimpleRoom()
    {
        StopAllCoroutines();
        foreach (Transform child in simpleRoomGenerator.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                GameObject.Destroy(child2.gameObject);
            }
        }
        roomPositions = new Vector3[roomNumbers];
        roomSize = new Vector3[roomNumbers];
        rooms = new List<GameObject>();
        roomsOrdered = new List<GameObject>();

        StartCoroutine(CreateRoom());
    }

    // Creer des salles qui ne se chevauchent pas avec une distance minimale
    private IEnumerator CreateRoom()
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

            GameObject newRoom = Instantiate(roomPrefab, roomGenerator.transform);
            newRoom.name = "Room " + i;
            newRoom.transform.localScale = scale;
            newRoom.transform.localPosition = position;
            newRoom.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f));
            newRoom.tag = "Room";

            rooms.Add(newRoom);

            yield return new WaitForSeconds(timeToCreate);
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

                GameObject corridor = Instantiate(roomPrefab, corridorGenerator.transform);
                
                Vector3 point2 = room2.transform.localPosition;
                
                if (point.x - point2.x < point.z - point2.z || point2.x - point.x > point2.z - point.z)
                {
                    corridor.name = "Corridor Droite - " + i;
                    corridor.tag = "Corridor";

                    corridor.transform.localScale =  new Vector3(Mathf.Abs(point2.x - point.x), 1, 4);

                    corridor.transform.localPosition = new Vector3(point.x < point2.x ? point.x + corridor.transform.localScale.x / 2 : point.x - corridor.transform.localScale.x / 2, point.y, point.z);
                    
                    corridor.GetComponent<MeshRenderer>().material.color = Color.green;

                    if (corridor.transform.localPosition.z + corridor.transform.localScale.z / 2 > (room2.transform.localPosition.z + room2.transform.localScale.z / 2) || corridor.transform.localPosition.z - corridor.transform.localScale.z / 2 < (room2.transform.localPosition.z - room2.transform.localScale.z / 2))
                    {
                        GameObject corridor2 = Instantiate(roomPrefab, corridorGenerator.transform);
                        corridor2.name = "Corridor Bas - " + i;
                        corridor2.tag = "Corridor";
                        
                        corridor2.transform.localScale =  new Vector3(4, 1, Mathf.Abs(point2.z > point.z ? point2.z - point.z : point.z - point2.z));
                    
                        corridor2.transform.localPosition = new Vector3(point.x < point2.x ? corridor.transform.localPosition.x + corridor.transform.localScale.x / 2 : corridor.transform.localPosition.x - corridor.transform.localScale.x / 2, point.y, point.z > point2.z ? point.z - corridor2.transform.localScale.z / 2 : point2.z - corridor2.transform.localScale.z / 2);
                    
                        corridor2.GetComponent<MeshRenderer>().material.color = Color.magenta;
                    }
                }
                else
                {
                    corridor.name = "Corridor Haut - " + i;
                    corridor.tag = "Corridor";

                    corridor.transform.localScale = new Vector3(4, 1, Mathf.Abs(point2.z > point.z ? point2.z - point.z : point.z - point2.z));
                    
                    corridor.transform.localPosition = new Vector3(point.x, point.y, point.z + corridor.transform.localScale.z / 2);
                    
                    corridor.GetComponent<MeshRenderer>().material.color = Color.red;

                    if (corridor.transform.localPosition.x + corridor.transform.localScale.x / 2 < (room2.transform.localPosition.x - room2.transform.localScale.x / 2) ||
                        corridor.transform.localPosition.x - corridor.transform.localScale.x / 2 > (room2.transform.localPosition.x + room2.transform.localScale.x / 2))
                    {
                        GameObject corridor2 = Instantiate(roomPrefab, corridorGenerator.transform);
                        corridor2.name = "Corridor Gauche - " + i;
                        corridor2.tag = "Corridor";
                        
                        corridor2.transform.localScale =  new Vector3(Mathf.Abs(point2.x > point.x ? point2.x - point.x : point.x - point2.x), 1, 4);
                    
                        corridor2.transform.localPosition = new Vector3(point.x > point2.x ? point.x - corridor2.transform.localScale.x / 2 : point2.x - corridor2.transform.localScale.x / 2, point.y, corridor.transform.localPosition.z + corridor.transform.localScale.z / 2);
                    
                        corridor2.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                }
            }
        }
    }

    private void OnValidate()
    {
        if (mapSizeX < minMapSize) mapSizeX = minMapSize;
        if (mapSizeY < minMapSize) mapSizeY = minMapSize;

        if (roomSpace > mapSizeX * 0.05f) roomSpace = (int)(mapSizeX * 0.05f);
        if (roomSpace > mapSizeY * 0.05f) roomSpace = (int)(mapSizeY * 0.05f);

        if (maxRoomSize > mapSizeX * 0.1f) maxRoomSize = (int)(mapSizeX * 0.1f);
        if (maxRoomSize > mapSizeY * 0.1f) maxRoomSize = (int)(mapSizeY * 0.1f);
        if (maxRoomSize < 1) minRoomSize = 1;

        if (minRoomSize > maxRoomSize) minRoomSize = maxRoomSize;
        if (minRoomSize < 1) minRoomSize = 1;
    }

    public void CreateRoomsButton()
    {
        createNewRooms = true;
    }

    public void SetTime(float value)
    {
        timeToCreate = value;
        if (timeText != null) timeText.GetComponent<TextMeshProUGUI>().text = timeToCreate.ToString("F2");
    }

    public void SetMaxMapSizeX(float value)
    {
        mapSizeX = Mathf.RoundToInt(value);
        if (MapSizeXText != null) MapSizeXText.GetComponent<TextMeshProUGUI>().text = mapSizeX.ToString();
    }

    public void SetMaxMapSizeY(float value)
    {
        mapSizeY = Mathf.RoundToInt(value);
        if (MapSizeYText != null) MapSizeYText.GetComponent<TextMeshProUGUI>().text = mapSizeY.ToString();
    }

    public void SetMinRoomSize(float value)
    {
        minRoomSize = Mathf.RoundToInt(value);
        if (minRoomSizeText != null) minRoomSizeText.GetComponent<TextMeshProUGUI>().text = minRoomSize.ToString();
    }

    public void SetMaxRoomSize(float value)
    {
        maxRoomSize = Mathf.RoundToInt(value);
        if (maxRoomSizeText != null) maxRoomSizeText.GetComponent<TextMeshProUGUI>().text = maxRoomSize.ToString();
    }

    public void SetRoomSpace(float value)
    {
        roomSpace = Mathf.RoundToInt(value);
        if (roomSpaceText != null) roomSpaceText.GetComponent<TextMeshProUGUI>().text = roomSpace.ToString();
    }

    public void SetRoomNumber(float value)
    {
        roomNumbers = Mathf.RoundToInt(value);
        if (roomNumbersText != null) roomNumbersText.GetComponent<TextMeshProUGUI>().text = roomNumbers.ToString();
    }
}
