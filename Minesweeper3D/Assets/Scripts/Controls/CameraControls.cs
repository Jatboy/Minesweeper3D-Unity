using Minesweeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control {
    public class CameraControls : MonoBehaviour {

        private Camera camera;
        public Minefield minefield;

        private float maxDistance;
        private float maxDistanceOffset = 6f;
        //private Bounds bounds;
        //public Vector3 offset;

        void Start() {
            camera = GetComponent<Camera>();
            if (camera == null || minefield == null)
                throw new MissingReferenceException();
            minefield.camera = camera;
            SetMaxDistance();
        }
        
        void Update() {

        }

        public void MoveCamera() {

        }

        /// <summary>
        /// Force the camera to update it's data depending on the minefield.
        /// </summary>
        public void UpdateCamera() {
            SetMaxDistance();
        }

        public void UpdateCameraDistance() {
            transform.Translate(new Vector3(0, 0, -maxDistance), minefield._Grid.transform);

            /* Vector3 center = bounds.center;
             Vector3 newPosition = center + offset;
             transform.position = newPosition;*/
        }

        public void CenterCamera() {
            SetMaxDistance();
            camera.transform.position = Vector3.zero + new Vector3(minefield._Grid.transform.position.x, minefield._Grid.transform.position.y, minefield._Grid.transform.position.z - maxDistance);
        }

        private void SetMaxDistance() {
            maxDistance = minefield.TotalWidth;
            if (minefield.TotalHeight > maxDistance)
                maxDistance = minefield.TotalHeight;
            if (minefield.TotalDepth > maxDistance)
                maxDistance = minefield.TotalDepth;

            maxDistance = (maxDistance / 2);
            maxDistance += (maxDistanceOffset * maxDistance);

            /*var bounds = new Bounds(minefield._Grid.transform.position, Vector3.zero);
            bounds.Encapsulate(minefield._Grid.GetComponent<BoxCollider>().bounds);
            this.bounds = bounds;*/
        }
    }
}
