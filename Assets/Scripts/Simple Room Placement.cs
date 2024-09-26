using UnityEngine;

public class SimpleRoomPlacement : MonoBehaviour
{
    public GameObject room;

    public int mapSizeX = 20;
    public int mapSizeY = 20;

    public int roomNumbers = 8;

    private Vector3[] roomPositions;
    private Vector3[] roomSize;

    // Start is called before the first frame update
    void Start()
    {
        roomPositions = new Vector3[roomNumbers];
        roomSize = new Vector3[roomNumbers];
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateRoom()
    {
        
        for (int i = 0; i < roomNumbers; i++)
        {
            Vector3 position = new Vector3(Random.Range(0, mapSizeX), 1, Random.Range(0, mapSizeY));
            Vector3 scale = new Vector3(Random.Range(2, 9), 1, Random.Range(2, 9));
            roomPositions[i] = position;
            roomSize[i] = room.transform.localScale;

            GameObject newRoom = Instantiate(room, position, Quaternion.identity);
            newRoom.transform.localScale = scale;
        }
    }
}
