using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace game.levelGeneration
{
    public class Room
    {
        private int _roomHeight;
        private int _roomWidth;
        private List<Vector2> _doorPositions = new List<Vector2>();
        private List<DoorPosition> _doorPositionTypes;

        public Room(int roomHeight, int roomWidth, List<DoorPosition> doorPositionTypes)
        {
            RoomHeight = roomHeight;
            RoomWidth = roomWidth;
            //get rid of duplicates
            DoorPositionTypes = doorPositionTypes.Distinct().ToList();
            GenerateDoors();
        }

        public int RoomHeight { get => _roomHeight; set => _roomHeight = value; }
        public int RoomWidth { get => _roomWidth; set => _roomWidth = value; }
        public List<Vector2> DoorPositions { get => _doorPositions; set => _doorPositions = value; }
        public List<DoorPosition> DoorPositionTypes { get => _doorPositionTypes; set => _doorPositionTypes = value; }

        private void GenerateDoors()
        {
            foreach(var doorPosition in DoorPositionTypes)
            {
                switch(doorPosition)
                {
                    case (DoorPosition.Top):
                        int xPos = UnityEngine.Random.Range(1, RoomWidth);
                        int yPos = RoomHeight - 1;
                        Vector2 spawnPosition = new Vector2(xPos, yPos);
                        DoorPositions.Add(spawnPosition);
                        Debug.Log("Top door: " + spawnPosition);
                        break;
                    case (DoorPosition.Right):
                        int xPosR = RoomWidth - 1;
                        int yPosR = UnityEngine.Random.Range(0,RoomHeight);
                        Vector2 spawnPositionR = new Vector2(xPosR, yPosR);
                        DoorPositions.Add(spawnPositionR);
                        Debug.Log("Roght door: " + spawnPositionR);
                        break;
                    case (DoorPosition.Bottom):
                        int xPosB = UnityEngine.Random.Range(0, RoomWidth);
                        int yPosB = 0;
                        Vector2 spawnPositionB = new Vector2(xPosB, yPosB);
                        DoorPositions.Add(spawnPositionB);
                        Debug.Log("Bottom door: " + spawnPositionB);
                        break;
                    case (DoorPosition.Left):
                        int xPosL = 0;
                        int yPosL = UnityEngine.Random.Range(0, RoomHeight);
                        Vector2 spawnPositionL = new Vector2(xPosL, yPosL);
                        DoorPositions.Add(spawnPositionL);
                        Debug.Log("Left door: " + spawnPositionL);
                        break;
                }
            }
        }
    }
}
