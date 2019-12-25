using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace game.levelGeneration
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private LevelSO _levelSO;
        [SerializeField]
        private Slider _numberOfRoomsSlider = null;
        [SerializeField]
        private Slider _maxRoomHeight = null;
        [SerializeField]
        private Slider _maxRoomWidth = null;
        [SerializeField]
        private Slider _minRoomHeigth = null;
        [SerializeField]
        private Slider _minRoomWidth = null;
        [SerializeField]
        private Slider _maxPathLength = null;
        [SerializeField]
        private Slider _minPathLength = null;
        [SerializeField]
        private Slider _wallsHeigth = null;
        [SerializeField]
        private Slider _minCurveCount = null;
        [SerializeField]
        private Slider _maxCurveCount = null;

        private void Start()
        {
            SetSliderValues();
        }

        private void SetSliderValues()
        {
            _numberOfRoomsSlider.value = _levelSO.NumberOfRooms;
            _maxRoomHeight.value = _levelSO.MaxRoomHeight;
            _maxRoomWidth.value = _levelSO.MaxRoomWidth;
            _minRoomHeigth.value = _levelSO.MinRoomHeight;
            _minRoomWidth.value = _levelSO.MinRoomWidth;
            _maxPathLength.value = _levelSO.MaxPathLength;
            _minPathLength.value = _levelSO.MinPathLength;
            _wallsHeigth.value = _levelSO.WallsHeight;
            _minCurveCount.value = _levelSO.MinCurveCount;
            _maxCurveCount.value = _levelSO.MaxCurveCount;
        }

        public void GenerateLevel()
        {
            SceneManager.LoadScene(0);
            SetSliderValues();
        }

        public void SetNumberOfRoomsForSlider(float f)
        {
            _levelSO.NumberOfRooms = (int)f;
        }

        public void SetMaxRoomHeightForSlider(float f)
        {
            _levelSO.MaxRoomHeight = (int)f;
        }
        public void SetMaxRoomWidthForSlider(float f)
        {
            _levelSO.MaxRoomWidth = (int)f;
        }

        public void SetMinRoomHeightForSlider(float f)
        {
            _levelSO.MinRoomHeight = (int)f;
        }
        public void SetMinRoomWidthForSlider(float f)
        {
            _levelSO.MinRoomWidth = (int)f;
        }

        public void SetMaxPathLengthForSlider(float f)
        {
            _levelSO.MaxPathLength = (int)f;
        }
        public void SetMinPathLengthForSlider(float f)
        {
            _levelSO.MinPathLength = (int)f;
        }

        public void SetWallsHeightForSlider(float f)
        {
            _levelSO.WallsHeight = (int)f;
        }
        public void SetMinCurveCountForSlider(float f)
        {
            _levelSO.MinCurveCount = (int)f;
        }

        public void SetMaxCurveCountForSlider(float f)
        {
            _levelSO.MaxCurveCount = (int)f;
        }
    }
}
