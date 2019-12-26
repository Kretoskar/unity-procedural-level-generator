using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace game.levelGeneration
{

    /// <summary>
    /// Generates the whole level
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        private LevelSO _levelSO;

        #region injectedFromSO
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
        #endregion

        private int _currentNumberOfRooms = 0;          //how many rooms where generated
        private Vector2 _currentFlorPosition;           //position of newly generated floor
        private GameObject _levelGO;                    //parent object of the whole level
        private GameObject _pathGO;                     //parent object of paths
        private GameObject _wallsGO;                    //parent object of walls
        private List<Vector2> _emptyPathClosings;       //end of a path, where a new room should be generated
        private List<Vector2> _takenPositions;          //positions where a floor was generated
        private List<int> _currentRoomCurvesIndexes;    //path index, where a curve should occur

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

        /// <summary>
        /// Generates the whole level, both rooms, paths and walls
        /// </summary>
        private void GenerateLevel()
        {
            //Generate parent objects
            _levelGO = new GameObject("Level");
            _levelGO.transform.parent = transform;
            _pathGO = new GameObject("Path");
            _pathGO.transform.parent = _levelGO.transform;
            _wallsGO = new GameObject("Walls");
            _wallsGO.transform.parent = _levelGO.transform;

            //Spawn rooms and paths, until there are '_numberOfRooms' rooms
            //first room is spawned at (0,0) next rooms are spawned at the end of paths
            do
            {
                for (int j = 0; j < _emptyPathClosings.Count; j++)
                {
                    SpawnRoomAndPath(_emptyPathClosings[j]);
                }
            } while (_currentNumberOfRooms < _numberOfRooms);

            SpawnWalls();
        }

        /// <summary>
        /// Spawns a room at a given position, then spawns paths from each of the doors
        /// </summary>
        /// <param name="spawnPosition">position of where to spawn the room</param>
        private void SpawnRoomAndPath(Vector2 spawnPosition)
        {
            //Spawn the paren GameObject
            GameObject roomGO = new GameObject("Room");
            roomGO.transform.parent = _levelGO.transform;

            //Generate random room properties 
            int roomHeight = UnityEngine.Random.Range(_minRoomHeight, _maxRoomHeight + 1);
            int roomWidth = UnityEngine.Random.Range(_minRoomWidth, _maxRoomWidth + 1);
            int numberOfDoors = UnityEngine.Random.Range(1, 4);

            //Prepare a list of door positions to then generate doors at random positions
            List<DoorPosition> doorPositionTypes = new List<DoorPosition>();
            Array values = Enum.GetValues(typeof(DoorPosition)); //Used for getting random value from enum

            //Generate doors at random positions
            for (int i = 0; i < numberOfDoors; i++)
            {
                //Get random value from enum
                System.Random random = new System.Random();
                DoorPosition doorPos = (DoorPosition)values.GetValue(UnityEngine.Random.Range(0, values.Length));
                //Add a random door
                doorPositionTypes.Add(doorPos);
            }

            //Create and instantiate the room
            Room room = new Room(roomHeight, roomWidth, doorPositionTypes);
            for(int i = 0; i < roomWidth; i++)
            {
                for(int j = 0; j < roomHeight; j++)
                {
                    SpawnSingleFloorTile(i + (int)spawnPosition.x, j + (int)spawnPosition.y, roomGO.transform);
                }
            }
            _currentNumberOfRooms++;

            //Spawn path from each of the room's doors
            SpawnPath(room);
        }

        /// <summary>
        /// Spawn path from each of the room's doors
        /// </summary>
        /// <param name="room">Room from which door's to spawn the paths</param>
        private void SpawnPath(Room room)
        {
            //As the rooms where generated at the path endings, 
            //we can clear the list of empty path closings
            _emptyPathClosings.Clear();

            //Spawn a path from each of the room's doors
            for(int i = 0; i < room.DoorPositions.Count; i++)
            {
                //Only if there is still a room to add path to
                if (_currentNumberOfRooms < _numberOfRooms)
                {
                    //Randomize path properties
                    Vector2 pathClosingPosition = new Vector2();
                    int numberOfCurves = UnityEngine.Random.Range(_minCurveCount, _maxCurveCount + 1);
                    int pathLength = UnityEngine.Random.Range(_minPathLength, _maxPathLength + 1);
                    _currentRoomCurvesIndexes = new List<int>();

                    //Randomize curves indexes
                    for (int k = 0; k < numberOfCurves; k++)
                    {
                        int curveIndex = UnityEngine.Random.Range(1, pathLength);
                        _currentRoomCurvesIndexes.Add(curveIndex);
                    }
                    _currentRoomCurvesIndexes = _currentRoomCurvesIndexes.Distinct().ToList();

                    //Generate 'pathLength' of floor tiles
                    for (int j = 0; j < pathLength; j++)
                    {
                        //change path if the index of instantiated floor is equal to curveIndex
                        foreach (int curveIndex in _currentRoomCurvesIndexes)
                        {
                            if(j == curveIndex)
                            {
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
                        //calculate spawn position
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

                        //instantiate the floor tile at spawnPosition
                        SpawnSingleFloorTile((int)spawnPosition.x, (int)spawnPosition.y, _pathGO.transform, "PathFloor");
                    }
                    //add an empty path closing at the position of last path tile
                    _emptyPathClosings.Add(pathClosingPosition);
                }
            }
        }

        /// <summary>
        /// Spawn walls surrounding the floor tiles
        /// </summary>
        private void SpawnWalls()
        {
            //Check if it's a horizontal path, vertical path, or a corner
            foreach (var takenPosition in _takenPositions)
            {
                int x = (int)takenPosition.x;
                int y = (int)takenPosition.y;
                List<Vector2> neighbourPositions = new List<Vector2>();
                //Add positions surrounding the current tile to neighbourPositions
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            continue;
                        neighbourPositions.Add(new Vector2(x - 1 + i, y - 1 + j));
                    }
                }

                //Check if neighbour positions are taken
                foreach (var neighbourPosition in neighbourPositions)
                {
                    //If not, spawn a single wall tile
                    if (!_takenPositions.Contains(neighbourPosition))
                    {
                        for (int i = 0; i < _wallsHeight; i++)
                        {
                            SpawnSingleWallTile(new Vector3(neighbourPosition.x, neighbourPosition.y, -i));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spawn a single floor tile at a given position,
        /// with given parent transform and a name
        /// </summary>
        /// <param name="xPos">x position of floor tile</param>
        /// <param name="yPos">y position of floor tile</param>
        /// <param name="parent">transform of the parent GameObject</param>
        /// <param name="name">tile GameObject name</param>
        private void SpawnSingleFloorTile(int xPos, int yPos, Transform parent, string name = "Floor")
        {
            Vector2 spawnPosition = new Vector2(xPos, yPos);
            GameObject floorTile = Instantiate(_floorPrefab, spawnPosition, Quaternion.identity);
            floorTile.name = name;
            floorTile.transform.parent = parent;
            _takenPositions.Add(spawnPosition);
        }

        /// <summary>
        /// Spawn a single wall tile at a given position
        /// </summary>
        /// <param name="pos">position on where to spawn</param>
        private void SpawnSingleWallTile(Vector3 pos)
        {
            for (int i = 0; i < _wallsHeight; i++)
            {
                GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.identity);
                wall.name = "Wall";
                wall.transform.parent = _wallsGO.transform;
            }
        }

        /// <summary>
        /// Get data from _levelSO Scriptable Object
        /// </summary>
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

    /// <summary>
    /// Possible positions of the room's door
    /// </summary>
    public enum DoorPosition
    {
        Top,
        Right,
        Bottom, 
        Left
    }
}