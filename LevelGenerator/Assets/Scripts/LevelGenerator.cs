using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private int _minCurveCount = 0;
        private int _maxCurveCount = 3;
        private GameObject _floorPrefab = null;
        private GameObject _wallPrefab = null;

        private int _currentNumberOfRooms = 0;
        private Vector2 _currentFlorPosition;
        private GameObject _levelGO;
        private GameObject _pathGO;
        private GameObject _wallsGO;
        private List<Vector2> _emptyPathClosings;
        private List<Vector2> _takenPositions;
        private List<int> _currentRoomCurvesIndexes;

        private void Awake()
        {
            InjectDependenciesFromScriptableObject();
            _emptyPathClosings = new List<Vector2>();
            _takenPositions = new List<Vector2>();
            _emptyPathClosings.Add(new Vector2(0, 0));
            _currentFlorPosition = new Vector2();
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
            _wallsGO = new GameObject("Walls");
            _wallsGO.transform.parent = _levelGO.transform;
            do
            {
                for (int j = 0; j < _emptyPathClosings.Count; j++)
                {
                    SpawnRoom(_emptyPathClosings[j]);
                }
            } while (_currentNumberOfRooms < _numberOfRooms);
            SpawnWalls();
        }

        private void SpawnWalls()
        {
            //Check if it's a horizontal path, vertical path, or a corner
            foreach (var takenPosition in _takenPositions)
            {
                int x = (int)takenPosition.x;
                int y = (int)takenPosition.y;
                List<Vector2> neighbourPositions = new List<Vector2>();
                for(int i = 0; i < 3; i ++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            continue;
                        neighbourPositions.Add(new Vector2(x - 1 + i, y - 1 + j));
                    }
                }
                foreach (var neighbourPosition in neighbourPositions) {
                    if (!_takenPositions.Contains(neighbourPosition))
                    {
                        for (int i = 0; i < _wallsHeight; i++)
                        {
                            SpawnWall(new Vector3(neighbourPosition.x, neighbourPosition.y, -i));
                        }
                    }
                }
            }
        }

        private void SpawnWall(Vector3 pos)
        {
            for (int i = 0; i < _wallsHeight; i++)
            {
                GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.identity);
                wall.name = "Wall";
                wall.transform.parent = _wallsGO.transform;
            }
        }

        private void SpawnRoom(Vector2 spawnPosition)
        {
            GameObject roomGO = new GameObject("Room");
            roomGO.transform.parent = _levelGO.transform;
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
                DoorPosition doorPos = (DoorPosition)values.GetValue(UnityEngine.Random.Range(0, values.Length));
                doorPositionTypes.Add(doorPos);
            }
            //Create and instantiate room
            Room room = new Room(roomHeight, roomWidth, doorPositionTypes);
            for(int i = 0; i < roomWidth; i++)
            {
                for(int j = 0; j < roomHeight; j++)
                {
                    SpawnSingleFloorTile(i + (int)spawnPosition.x, j + (int)spawnPosition.y, roomGO.transform);
                }
            }
            _currentNumberOfRooms++;
            SpawnPath(room);
        }

        private void SpawnPath(Room room)
        {
            _emptyPathClosings.Clear();
            for(int i = 0; i < room.DoorPositions.Count; i++)
            {
                if (_currentNumberOfRooms < _numberOfRooms)
                {
                    Vector2 pathClosingPosition = new Vector2();
                    int numberOfCurves = UnityEngine.Random.Range(_minCurveCount, _maxCurveCount + 1);
                    int pathLength = UnityEngine.Random.Range(_minPathLength, _maxPathLength + 1);
                    _currentRoomCurvesIndexes = new List<int>();
                    for (int k = 0; k < numberOfCurves; k++)
                    {
                        int curveIndex = UnityEngine.Random.Range(1, pathLength);
                        _currentRoomCurvesIndexes.Add(curveIndex);
                    }
                    _currentRoomCurvesIndexes = _currentRoomCurvesIndexes.Distinct().ToList();
                    for (int j = 0; j < pathLength; j++)
                    {
                        foreach(int curveIndex in _currentRoomCurvesIndexes)
                        {
                            if(j == curveIndex)
                            {
                                //change path
                                if (room.DoorPositionTypes[i] == DoorPosition.Top || room.DoorPositionTypes[i] == DoorPosition.Bottom)
                                {
                                    room.DoorPositionTypes[i] = UnityEngine.Random.Range(0, 2)  == 0 ? DoorPosition.Right : DoorPosition.Left;
                                }
                                else
                                {
                                    room.DoorPositionTypes[i] = UnityEngine.Random.Range(0, 2) == 0 ? DoorPosition.Top : DoorPosition.Bottom;
                                }
                            }
                        }
                        Vector2 spawnPosition = new Vector2();
                        switch (room.DoorPositionTypes[i])
                        {
                            case (DoorPosition.Top):
                                spawnPosition = new Vector2(_currentFlorPosition.x, _currentFlorPosition.y + 1);                               
                                break;
                            case (DoorPosition.Right):
                                spawnPosition = new Vector2(_currentFlorPosition.x + 1, _currentFlorPosition.y);
                                break;
                            case (DoorPosition.Bottom):
                                spawnPosition = new Vector2(_currentFlorPosition.x, _currentFlorPosition.y - 1);
                                break;
                            case (DoorPosition.Left):
                                spawnPosition = new Vector2(_currentFlorPosition.x - 1, _currentFlorPosition.y);
                                break;
                        }
                        pathClosingPosition = spawnPosition;
                        _currentFlorPosition = spawnPosition;
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
            _minCurveCount = _levelSO.MinCurveCount;
            _maxCurveCount = _levelSO.MaxCurveCount;
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