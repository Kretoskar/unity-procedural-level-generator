using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace game.levelGeneration
{
    /// <summary>
    /// Updates sliders in the UI, 
    /// handles "Generate" button behaviour
    /// makes sliders in the UI responsive
    /// </summary>
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

        /// <summary>
        /// Update slider values to those from tyhe scriptable object
        /// </summary>
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

        /// <summary>
        /// "Generate" button behaviour, 
        /// reloads level and updates slider values to those from the scriptable object
        /// </summary>
        public void GenerateLevel()
        {
            SceneManager.LoadScene(0);
            SetSliderValues();
        }

        /// <summary>
        /// Updates scriptable object NumberOfRooms data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetNumberOfRoomsForSlider(float f)
        {
            _levelSO.NumberOfRooms = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MaxRoomHeight data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMaxRoomHeightForSlider(float f)
        {
            _levelSO.MaxRoomHeight = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MaxRoomWidth data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMaxRoomWidthForSlider(float f)
        {
            _levelSO.MaxRoomWidth = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MinRoomHeight data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMinRoomHeightForSlider(float f)
        {
            _levelSO.MinRoomHeight = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MinRoomWidth data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMinRoomWidthForSlider(float f)
        {
            _levelSO.MinRoomWidth = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MaxPathLength data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMaxPathLengthForSlider(float f)
        {
            _levelSO.MaxPathLength = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MinPathLength data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMinPathLengthForSlider(float f)
        {
            _levelSO.MinPathLength = (int)f;
        }

        /// <summary>
        /// Updates scriptable object WallsHeight data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetWallsHeightForSlider(float f)
        {
            _levelSO.WallsHeight = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MinCurveCount data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMinCurveCountForSlider(float f)
        {
            _levelSO.MinCurveCount = (int)f;
        }

        /// <summary>
        /// Updates scriptable object MaxCurveCount data 
        /// to the slider value 
        /// </summary>
        /// <param name="f">slider value</param>
        public void SetMaxCurveCountForSlider(float f)
        {
            _levelSO.MaxCurveCount = (int)f;
        }
    }
}
