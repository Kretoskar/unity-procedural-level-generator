using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.levelGeneration
{
    /// <summary>
    /// Handles move of the camera
    /// </summary>
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private int _speed = 100;

        /// <summary>
        /// Get user input and move the camera according to it
        /// </summary>
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            transform.Translate(h * Time.deltaTime * _speed, v * Time.deltaTime * _speed, 0);
        }
    }
}
