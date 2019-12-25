using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.levelGeneration
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelSettings", fileName = "Level")]
    public class LevelSO : ScriptableObject
    {
        [Header("Room properties")]

        [SerializeField]
        [Range(0, 1000)]
        private int _numberOfRooms = 0;
        [SerializeField]
        [Range(0, 100)]
        private int _maxRoomHeight = 0;
        [SerializeField]
        [Range(0, 100)]
        private int _maxRoomWidht = 0;
        [SerializeField]
        [Range(0, 100)]
        private int _minRoomHeight = 0;
        [SerializeField]
        [Range(0, 100)]
        private int _minRoomWidth = 0;
        [SerializeField]
        [Range(1, 10)]
        private int _wallsHeight = 3;

        [Header("Path properties")]

        [SerializeField]
        [Range(0, 100)]
        private int _maxPathLength = 0;
        [SerializeField]
        [Range(0, 100)]
        private int _minPathLength = 0;
        [SerializeField]
        [Range(0, 10)]
        private int _minCurveCount = 0;
        [SerializeField]
        [Range(0, 10)]
        private int _maxCurveCount = 3;

        [Header("Prefabs")]

        [SerializeField]
        private GameObject _floorPrefab = null;
        [SerializeField]
        private GameObject _wallPrefab = null;

        public int NumberOfRooms { get => _numberOfRooms; set => _numberOfRooms = value; }
        public int MaxRoomHeight { get => _maxRoomHeight; set => _maxRoomHeight = value; }
        public int MaxRoomWidth { get => _maxRoomWidht; set => _maxRoomWidht = value; }
        public int MinRoomHeight { get => _minRoomHeight; set => _minRoomHeight = value; }
        public int MinRoomWidth { get => _minRoomWidth; set => _minRoomWidth = value; }
        public int MaxPathLength { get => _maxPathLength; set => _maxPathLength = value; }
        public int MinPathLength { get => _minPathLength; set => _minPathLength = value; }
        public GameObject FloorPrefab { get => _floorPrefab; set => _floorPrefab = value; }
        public GameObject WallPrefab { get => _wallPrefab; set => _wallPrefab = value; }
        public int WallsHeight { get => _wallsHeight; set => _wallsHeight = value; }
        public int MinCurveCount { get => _minCurveCount; set => _minCurveCount = value; }
        public int MaxCurveCount { get => _maxCurveCount; set => _maxCurveCount = value; }
    }
}
