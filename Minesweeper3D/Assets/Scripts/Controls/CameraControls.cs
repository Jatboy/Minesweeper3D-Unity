using Minesweeper;
using UnityEngine;
using Assets.Settings;

namespace Minesweeper {
    public class CameraControls : MonoBehaviour {
        public Minefield minefield;

        private float maxDistance;
        private float maxDistanceOffset = 6f;
        private float moveSpeed = 20;
        private bool movementModeEnabled = false;
        //private Bounds bounds;
        //public Vector3 offset;

        void Start() {
            SetMaxDistance();
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 1000f)) {
                    Debug.DrawLine(transform.position, hit.point, Color.white);
                    //hit.collider.transform.GetComponent<Renderer>().material.color = Random.ColorHSV(); // Test hit detection
                    Destroy(hit.collider.gameObject); // Alternate test hit detection
                }
            }
            MoveCamera();
            Inputs();
        }

        void FixedUpdate() {
            MoveCamera();
        }

        public void Inputs() {

        }

        public void MoveCamera() {
            // WIP New Code
            /*Vector3 velocity = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Horizontal"));
            transform.RotateAround(minefield.Middle, velocity, 20 * Time.deltaTime);*/

            CenterCamera();
            if (Input.GetKey(KeyBinding.MoveLeft) ^ Input.GetKey(KeyBinding.MoveRight)) {
                if (Input.GetKey(KeyBinding.MoveLeft)) {
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
                else {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                transform.LookAt(minefield.Middle);
            }
            if (Input.GetKey(KeyBinding.MoveUp) ^ Input.GetKey(KeyBinding.MoveDown)) {
                if ((transform.rotation.eulerAngles.x >= 0 && transform.rotation.eulerAngles.x < 89) || (transform.rotation.eulerAngles.x <= 360 && transform.rotation.eulerAngles.x > 271)) {
                    if (Input.GetKey(KeyBinding.MoveUp)) {
                        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                    }
                    else {
                        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                    }
                }
                transform.LookAt(minefield.Middle);
            }
        }



        /// <summary>
        /// Force the camera to update it's data depending on the minefield.
        /// </summary>
        public void UpdateCamera() {
            SetMaxDistance();
        }

        public void UpdateCameraDistance() {
            transform.Translate(new Vector3(0, 0, -maxDistance), minefield.Grid.transform.GetChild(0));

            /* Vector3 center = bounds.center;
             Vector3 newPosition = center + offset;
             transform.position = newPosition;*/
        }

        public void CenterCamera() {
            SetMaxDistance();
            transform.position = Vector3.Lerp(transform.position, new Vector3(minefield.Middle.x, minefield.Middle.y, minefield.Middle.z - maxDistance), Time.deltaTime);
            transform.LookAt(minefield.Middle);
        }

        private void SetMaxDistance() {
            maxDistance = minefield.TotalSize.x;
            if (minefield.TotalSize.y > maxDistance)
                maxDistance = minefield.TotalSize.y;
            if (minefield.TotalSize.y > maxDistance)
                maxDistance = minefield.TotalSize.z;

            maxDistance = (maxDistance / 2);
            maxDistance += (maxDistanceOffset * maxDistance);

            /*var bounds = new Bounds(minefield._Grid.transform.position, Vector3.zero);
            bounds.Encapsulate(minefield._Grid.GetComponent<BoxCollider>().bounds);
            this.bounds = bounds;*/
        }
    }
}
