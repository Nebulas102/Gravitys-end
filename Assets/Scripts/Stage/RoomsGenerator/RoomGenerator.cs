using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _room;

    [SerializeField]
    private int maxRooms = 15;

    private GameObject latestRoom;
    private Vector3 spawnPosition;
    private float delay = .5f;

    private List<GameObject> rooms = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GenerateRooms());
    }

    void FixedUpdate()
    {
        StartCoroutine(GenerateRooms());
    }

    IEnumerator GenerateRooms()
    {
        bool firstRoom = false;
        while (maxRooms > 0)
        {
            if (latestRoom != null)
            {
                latestRoom.GetComponent<Room>().rayCastReady = false;
            }

            if (firstRoom)
            {
                GameObject newRoom = Instantiate(_room);

                newRoom.GetComponent<Room>().id = latestRoom.GetComponent<Room>().id + 1;

                yield return new WaitForSeconds(delay);

                float newRoomWidth = GetSize(newRoom)[0];
                float newRoomHeight = GetSize(newRoom)[1];

                RoomDirection(newRoom, newRoomWidth, newRoomHeight);

                newRoom.transform.position = spawnPosition;
                newRoom.transform.rotation = Quaternion.identity;

                rooms.Add(newRoom);

                latestRoom = newRoom;
            }
            else
            {
                latestRoom = Instantiate(_room, new Vector3(0, 0, 0), Quaternion.identity);

                yield return new WaitForSeconds(delay);

                rooms.Add(latestRoom);

                firstRoom = true;
            }

            maxRooms--;
        }
    }

    private float[] GetSize(GameObject room)
    {
        float width = 0f;
        float height = 0f;

        if (room.GetComponent<MeshRenderer>() != null &&
            room.GetComponent<MeshFilter>().mesh != null &&
            room.GetComponent<MeshCollider>().sharedMesh != null)
        {
            width = room.GetComponent<MeshRenderer>().bounds.size.x;
            height = room.GetComponent<MeshRenderer>().bounds.size.z;
        }

        return new float[2] { width, height };
    }

    private void RoomDirection(GameObject newroom, float width, float height)
    {
        int newRoomDirection = Random.Range(0, 4);

        Room _latestRoom = latestRoom.GetComponent<Room>();
        Room _newRoom = newroom.GetComponent<Room>();

        // top side
        if (newRoomDirection == 0 && _latestRoom.openSides.Any(x => x == 0))
        {
            spawnPosition = new Vector3(latestRoom.transform.position.x, latestRoom.transform.position.y, latestRoom.transform.position.z + height);

            _latestRoom.openSides.RemoveAll(x => x == 0);
            _newRoom.openSides.RemoveAll(x => x == 2);

            Debug.Log("Top: remove " + _latestRoom.id + " top and " + _newRoom.id + " bottom");

            _newRoom.DisplayOpenSides();
        }
        // right side
        else if (newRoomDirection == 1 && _latestRoom.openSides.Any(x => x == 1))
        {
            spawnPosition = new Vector3(latestRoom.transform.position.x + width, latestRoom.transform.position.y, latestRoom.transform.position.z);

            _latestRoom.openSides.RemoveAll(x => x == 1);
            _newRoom.openSides.RemoveAll(x => x == 3);

            Debug.Log("Right: remove " + _latestRoom.id + " right and " + _newRoom.id + " left");

            _newRoom.DisplayOpenSides();
        }
        // bottom side
        else if (newRoomDirection == 2 && _latestRoom.openSides.Any(x => x == 2))
        {
            spawnPosition = new Vector3(latestRoom.transform.position.x, latestRoom.transform.position.y, latestRoom.transform.position.z - height);

            _latestRoom.openSides.RemoveAll(x => x == 2);
            _newRoom.openSides.RemoveAll(x => x == 0);

            Debug.Log("Bottom: remove " + _latestRoom.id + " bottom and " + _newRoom.id + " top");

            _newRoom.DisplayOpenSides();
        }
        // left side
        else if (newRoomDirection == 3 && _latestRoom.openSides.Any(x => x == 3))
        {
            spawnPosition = new Vector3(latestRoom.transform.position.x - width, latestRoom.transform.position.y, latestRoom.transform.position.z);

            _latestRoom.openSides.RemoveAll(x => x == 3);
            _newRoom.openSides.RemoveAll(x => x == 1);

            Debug.Log("Left: remove " + _latestRoom.id + " left and " + _newRoom.id + " right");

            _newRoom.DisplayOpenSides();
        }
        else if (_latestRoom.openSides.Count() == 0)
        {
            Debug.Log("No Sides");
            int randomListNumb = Random.Range(0, rooms.Count() + 1);
            GameObject randomRoom = rooms.Where(x => x != latestRoom && x != newroom).ToList()[randomListNumb];
            latestRoom = randomRoom;
            RoomDirection(latestRoom, GetSize(randomRoom)[0], GetSize(randomRoom)[1]);
        }
        else
        {
            Debug.Log("Regenerate Position");
            RoomDirection(newroom, width, height);
        }
    }
}
