using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.levelGeneration
{

    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        private LevelSO _levelSO;

        //Injected from scriptable object
        private int _numberOfRooms;
        private int _maxRoomHeight = 0;
        private int _maxRoomWidth = 0;
        private int _minRoomHeight = 0;
        private int _minRoomWidth = 0;
        private int _wallsHeight = 3;
        private int _maxPathLength = 0;
        private int _minPathLength = 0;
        private GameObject _floorPrefab = null;
        private GameObject _wallPrefab = null;

        private GameObject _levelGO;
        private GameObject _pathGO;
        private List<Vector2> _takenPositions = new List<Vector2>();

        private void Awake()
        {
            InjectDependenciesFromScriptableObject();
        }

        private void Start()
        {
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            _levelGO = new GameObject("Level");
            _levelGO.transform.parent = transform;
            _pathGO = new GameObject("Path");
            _pathGO.transform.parent = _levelGO.transform;
            SpawnRoom();
        }

        private void SpawnRoom()
        {
            GameObject roomGO = new GameObject("Room");
            roomGO.transform.parent = _levelGO.transform;
            //Generate random room 
            int roomHeight = UnityEngine.Random.Range(_minRoomHeight, _maxRoomHeight + 1);
            int roomWidth = UnityEngine.Random.Range(_minRoomWidth, _maxRoomWidth + 1);
            int numberOfDoors = UnityEngine.Random.Range(1, 5);
            List<DoorPosition> doorPositionTypes = new List<DoorPosition>();
            Array values = Enum.GetValues(typeof(DoorPosition)); //Used for getting random value from enum
            for (int i = 0; i < numberOfDoors; i++)
            {
                //Get random value from enum
                System.Random random = new System.Random();
                DoorPosition doorPos = (DoorPosition)values.GetValue(UnityEngine.Random.Range(0, values.Length));
                doorPositionTypes.Add(doorPos);
            }
            //Create and instantiate room
            Room room = new Room(roomHeight, roomWidth, doorPositionTypes);
            for(int i = 0; i < roomWidth; i++)
            {
                for(int j = 0; j < roomHeight; j++)
                {
                    SpawnSingleFloorTile(i, j, roomGO.transform);
                }
            }
            SpawnPath(room);
        }

        private void SpawnPath(Room room)
        {
            for(int i = 0; i < room.DoorPositions.Count; i++)
            {
                for(int j = 0; j < _maxPathLength; j++)
                {
                    switch(room.DoorPositionTypes[i])
                    {
                        case (DoorPosition.Top):
                            SpawnSingleFloorTile((int)room.DoorPositions[i].x, (int)room.DoorPositions[i].y + j, _pathGO.transform, "PathFloor");
                            break;
                        case (DoorPosition.Right):
                            SpawnSingleFloorTile((int)room.DoorPositions[i].x + j, (int)room.DoorPositions[i].y, _pathGO.transform, "PathFloor");
                            break;
                        case (DoorPosition.Bottom):
                            SpawnSingleFloorTile((int)room.DoorPositions[i].x, (int)room.DoorPositions[i].y - j, _pathGO.transform, "PathFloor");
                            break;
                        case (DoorPosition.Left):
                            SpawnSingleFloorTile((int)room.DoorPositions[i].x - j, (int)room.DoorPositions[i].y, _pathGO.transform, "PathFloor");
                            break;
                    }
                }
            }
        }

        private void SpawnSingleFloorTile(int xPos, int yPos, Transform parent, string name = "Floor")
        {
            GameObject floorTile = Instantiate(_floorPrefab, new Vector2(xPos, yPos), Quaternion.identity);
            floorTile.transform.parent = parent;
        }

        private void InjectDependenciesFromScriptableObject()
        {
            _numberOfRooms = _levelSO.NumberOfRooms;
            _maxRoomHeight = _levelSO.MaxRoomHeight;
            _maxRoomWidth = _levelSO.MaxRoomWidth;
            _minRoomHeight = _levelSO.MinRoomHeight;
            _minRoomWidth = _levelSO.MinRoomWidth;
            _wallsHeight = _levelSO.WallsHeight;
            _maxPathLength = _levelSO.MaxPathLength;
            _minPathLength = _levelSO.MinPathLength;
            _floorPrefab = _levelSO.FloorPrefab;
            _wallPrefab = _levelSO.WallPrefab;
        }
    }

    public enum DoorPosition
    {
        Top,
        Right,
        Bottom, 
        Left
    }
}