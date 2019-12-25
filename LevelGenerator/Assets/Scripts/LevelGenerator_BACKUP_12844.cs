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

<<<<<<< HEAD
        private int _currentNumberOfRooms = 0;
        private GameObject _levelGO;
        private GameObject _pathGO;
        private List<Vector2> _emptyPathClosings;
        private List<Vector2> _takenPositions;
=======
        private List<Vector2> _takenPositions = new List<Vector2>();
>>>>>>> parent of 989f7c7... Added spawning path from first room

        private void Awake()
        {
            InjectDependenciesFromScriptableObject();
            _emptyPathClosings = new List<Vector2>();
            _takenPositions = new List<Vector2>();
            _emptyPathClosings.Add(new Vector2(0, 0));
        }

        private void Start()
        {
            GenerateLevel();
        }

        private void GenerateLevel()
        {
<<<<<<< HEAD
            _levelGO = new GameObject("Level");
            _levelGO.transform.parent = transform;
            _pathGO = new GameObject("Path");
            _pathGO.transform.parent = _levelGO.transform;
            do
            {
                for (int j = 0; j < _emptyPathClosings.Count; j++)
                {
                    SpawnRoom(_emptyPathClosings[j]);
                }
            } while (_currentNumberOfRooms < _numberOfRooms);
=======
            //Generate '_numberOfRooms' rooms, and then from their doors
            //generate a path to the next room
           // for(int i = 0; i < _numberOfRooms; i++)
            //{
                SpawnRoom();
             //   SpawnPath();
           // }
>>>>>>> parent of 989f7c7... Added spawning path from first room
        }

        private void SpawnRoom(Vector2 spawnPosition)
        {
            //Generate random room 
            int roomHeight = UnityEngine.Random.Range(_minRoomHeight, _maxRoomHeight + 1);
            int roomWidth = UnityEngine.Random.Range(_minRoomWidth, _maxRoomWidth + 1);
            int numberOfDoors = UnityEngine.Random.Range(1, 4);
            List<DoorPosition> doorPositionTypes = new List<DoorPosition>();
            Array values = Enum.GetValues(typeof(DoorPosition)); //Used for getting random value from enum
            for (int i = 0; i < numberOfDoors; i++)
            {
                //Get random value from enum
                System.Random random = new System.Random();
                DoorPosition doorPos = (DoorPosition)values.GetValue(random.Next(values.Length));
                doorPositionTypes.Add(doorPos);
            }
            Room room = new Room(roomHeight, roomWidth, doorPositionTypes);
            for(int i = 0; i < roomWidth; i++)
            {
                for(int j = 0; j < roomHeight; j++)
                {
<<<<<<< HEAD
                    SpawnSingleFloorTile(i + (int)spawnPosition.x, j + (int)spawnPosition.y, roomGO.transform);
                }
            }
            _currentNumberOfRooms++;
            SpawnPath(room);
=======
                    Instantiate(_floorPrefab, new Vector2(i, j), Quaternion.identity);
                }
            }
>>>>>>> parent of 989f7c7... Added spawning path from first room
        }

        private void SpawnPath()
        {
<<<<<<< HEAD
            _emptyPathClosings.Clear();
            for(int i = 0; i < room.DoorPositions.Count; i++)
            {
                if (_currentNumberOfRooms < _numberOfRooms)
                {
                    Vector2 pathClosingPosition = new Vector2();
                    for (int j = 0; j < _maxPathLength; j++)
                    {
                        Vector2 spawnPosition = new Vector2();
                        switch (room.DoorPositionTypes[i])
                        {
                            case (DoorPosition.Top):
                                spawnPosition = new Vector2(room.DoorPositions[i].x, room.DoorPositions[i].y + j);                               
                                break;
                            case (DoorPosition.Right):
                                spawnPosition = new Vector2(room.DoorPositions[i].x + j, room.DoorPositions[i].y);
                                break;
                            case (DoorPosition.Bottom):
                                spawnPosition = new Vector2(room.DoorPositions[i].x, room.DoorPositions[i].y - j);
                                break;
                            case (DoorPosition.Left):
                                spawnPosition = new Vector2((int)room.DoorPositions[i].x - j, (int)room.DoorPositions[i].y);
                                break;
                        }
                        pathClosingPosition = spawnPosition;
                        SpawnSingleFloorTile((int)spawnPosition.x, (int)spawnPosition.y, _pathGO.transform, "PathFloor");
                    }
                    _emptyPathClosings.Add(pathClosingPosition);
                }
            }
        }

        private void SpawnSingleFloorTile(int xPos, int yPos, Transform parent, string name = "Floor")
        {
            Vector2 spawnPosition = new Vector2(xPos, yPos);
            GameObject floorTile = Instantiate(_floorPrefab, spawnPosition, Quaternion.identity);
            floorTile.name = name;
            floorTile.transform.parent = parent;
            _takenPositions.Add(spawnPosition);
=======
            throw new NotImplementedException();
>>>>>>> parent of 989f7c7... Added spawning path from first room
        }

        private void InjectDependenciesFromScriptableObject()
        {
            _numberOfRooms = _levelSO.NumberOfRooms + 1;
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