using Minesweeper;
using UnityEngine;
using Assets.Settings;

namespace Minesweeper {
    public class CameraControls : MonoBehaviour {
        public Minefield minefield;
        public float moveSpeed = 20;
        public GameObject textDisplay;
        public bool inputLocked = true;

        private float maxDistance;
        private float maxDistanceOffset = 6f;
        private bool isMovementMode = false;

        void Start() {
            CenterCamera();
        }

        void Update() {
            if (!inputLocked) {
                if (Input.GetMouseButtonDown(0) ^ Input.GetMouseButtonDown(1)) {
                    MouseInput();
                }
                Inputs();
            }
        }

        void FixedUpdate() {
            if(!inputLocked)
                MoveCamera();
        }

        public void MouseInput() {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 500f)) {
                //Debug.DrawLine(transform.position, hit.point, Color.white);
                GameObject obj = hit.collider.gameObject;
                Tile tile = obj.GetComponent<Tile>();
                if (Input.GetMouseButtonDown(0)) {
                    minefield.DeleteTile(tile._TileIndex);
                    Destroy(obj);
                }
                if (Input.GetMouseButtonDown(1)) {
                    tile.PlantFlag();
                }
                Debug.Log("Tile Info: " + tile.ToString());
                //Destroy(obj);
            }
        }

        public void Inputs() {
            Transform transform = minefield.Grid.transform;
            if (Input.GetKeyDown(KeyBinding.Preset1)) {
                transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            // TODO START
            if (Input.GetKeyDown(KeyBinding.Preset2)) { }
            if (Input.GetKeyDown(KeyBinding.Preset3)) { }
            if (Input.GetKeyDown(KeyBinding.Preset4)) { }
            if (Input.GetKeyDown(KeyBinding.Preset5)) { }
            if (Input.GetKeyDown(KeyBinding.Preset6)) { }
            // END
            if (Input.GetKeyDown(KeyBinding.MovementMode)) {
                isMovementMode = !isMovementMode;
                textDisplay.SetActive(isMovementMode);
            }
        }

        public void MoveCamera() {
            if(!isMovementMode) { 
                Vector3 toMove = new Vector3(0, 0, 0);
                if (Input.GetKey(KeyBinding.MoveLeft)) toMove += Vector3.up;
                if (Input.GetKey(KeyBinding.MoveRight)) toMove += Vector3.down;
                if (Input.GetKey(KeyBinding.MoveUp)) toMove += Vector3.left;
                if (Input.GetKey(KeyBinding.MoveDown)) toMove += Vector3.right;
                minefield.Grid.transform.Rotate(toMove * moveSpeed * Time.deltaTime);
                //CenterCamera(); // TODO - Readjust code to account for rotation and not translation or add a new method for this.
            } else {
                // TODO
            }
        }
        
        /// <summary>
        /// Force the camera to update its data depending on the Minefield.
        /// </summary>
        public void UpdateCamera() {
            SetMaxDistance();
        }

        public void UpdateCameraDistance() {
            transform.Translate(new Vector3(0, 0, -maxDistance), minefield.Grid.transform.GetChild(0));
        }

        public void CenterCamera() {
            SetMaxDistance();
            transform.position = Vector3.Lerp(transform.position, new Vector3(minefield.Middle.x, minefield.Middle.y, minefield.Middle.z - maxDistance), Time.deltaTime);
            transform.LookAt(minefield.Middle);
        }

        private void SetMaxDistance() {
            maxDistance = minefield.TotalSize.x;
            if (minefield.TotalSize.y > maxDistance) maxDistance = minefield.TotalSize.y;
            if (minefield.TotalSize.y > maxDistance) maxDistance = minefield.TotalSize.z;

            maxDistance = (maxDistance / 2);
            maxDistance += (maxDistanceOffset * maxDistance);
        }
    }
}
