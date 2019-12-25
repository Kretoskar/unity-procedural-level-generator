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
            //Generate '_numberOfRooms' rooms, and then from their doors
            //generate a path to the next room
           // for(int i = 0; i < _numberOfRooms; i++)
            //{
                SpawnRoom();
             //   SpawnPath();
           // }
        }

        private void SpawnRoom()
        {
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
                DoorPosition doorPos = (DoorPosition)values.GetValue(random.Next(values.Length));
                doorPositionTypes.Add(doorPos);
            }
            Room room = new Room(roomHeight, roomWidth, doorPositionTypes);
            for(int i = 0; i < roomWidth; i++)
            {
                for(int j = 0; j < roomHeight; j++)
                {
                    Instantiate(_floorPrefab, new Vector2(i, j), Quaternion.identity);
                }
            }
        }

        private void SpawnPath()
        {
            throw new NotImplementedException();
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