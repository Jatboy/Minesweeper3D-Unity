using Minesweeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Settings;

namespace Control
{
    public class CameraControls : MonoBehaviour
    {

        private Camera camera;
        public Minefield minefield;

        private float maxDistance;
        private float maxDistanceOffset = 6f;
        private float moveSpeed = 20;
        private bool movementModeEnabled = false;
        //private Bounds bounds;
        //public Vector3 offset;

        void Start()
        {
            camera = GetComponent<Camera>();
            if (camera == null || minefield == null)
                throw new MissingReferenceException();
            minefield.camera = camera;
            SetMaxDistance();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 1000f))
                {
                    Debug.DrawLine(camera.transform.position, hit.point, Color.white);
                    hit.collider.transform.GetComponent<Renderer>().material.color = Random.ColorHSV(); // Test hit detection
                    // Destroy(hit.collider.gameObject); // Alternate test hit detection
                }
            }
            MoveCamera();
            Inputs();
        }

        private void FixedUpdate()
        {
            MoveCamera();
        }

        public void Inputs()
        {

        }

        public void MoveCamera()
        {
            
            if(Input.GetKey(KeyBinding.MoveLeft) ^ Input.GetKey(KeyBinding.MoveRight))
            {
                camera.transform.LookAt(minefield.Middle);
                if (Input.GetKey(KeyBinding.MoveLeft))
                {
                    
                    camera.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
                else
                {
                    camera.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                camera.transform.LookAt(minefield.Middle);
            }
            if (Input.GetKey(KeyBinding.MoveUp) ^ Input.GetKey(KeyBinding.MoveDown))
            {

                if ((camera.transform.rotation.eulerAngles.x >= 0 && camera.transform.rotation.eulerAngles.x < 89) || (camera.transform.rotation.eulerAngles.x <= 360 && camera.transform.rotation.eulerAngles.x > 271))
                {
                    if (Input.GetKey(KeyBinding.MoveUp))
                    {
                            camera.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        camera.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                    }
                }
                camera.transform.LookAt(minefield.Middle);
            }

        }



        /// <summary>
        /// Force the camera to update it's data depending on the minefield.
        /// </summary>
        public void UpdateCamera()
        {
            SetMaxDistance();
        }

        public void UpdateCameraDistance()
        {
            transform.Translate(new Vector3(0, 0, -maxDistance), minefield._Grid.transform);

            /* Vector3 center = bounds.center;
             Vector3 newPosition = center + offset;
             transform.position = newPosition;*/
        }

        public void CenterCamera()
        {
            SetMaxDistance();
            camera.transform.position = new Vector3(minefield.Middle.x, minefield.Middle.y, minefield.Middle.z - maxDistance);
            camera.transform.LookAt(minefield.Middle);
        }

        private void SetMaxDistance()
        {
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
