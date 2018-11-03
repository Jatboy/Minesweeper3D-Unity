using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper;

namespace Minesweeper {
    [RequireComponent(typeof(BoxCollider))]
    public class Minefield : MonoBehaviour {
        public GameObject _Grid; // Parent object of all Generation code.
        public GameObject Cube; // Cube prefab that all tiles share.
        public GameObject Progress; // Progress bar object to hook into GenerateMinefield()
        public Camera camera;

        public float MineCount = 10; // How many mines should be genereated in the field.
        public float TilePadding = 4f; // How much spacing should be added between tiles.
        public float TileSize = 1f; // Size of tiles.
        public Tile[] tilePrefabs; // List of all the possible tile options.

        // X,Y,Z Dimensions of the Minefield
        private int MINEFIELD_WIDTH = 8;
        private int MINEFIELD_HEIGHT = 8;
        private int MINEFIELD_DEPTH = 8;

        private Tile[,,] tiles; // TileData Array
        private Coroutine routine; // Coroutine variable for calling StopCoroutine()
        private GameObject[,,] tileObjects; // Physical Tile Objects

        void Awake() {
            transform.position = Vector3.zero;
        }

        /// <summary>
        /// Iterate through children of the _Grid object and delete them.
        /// </summary>
        public void DestroyMinefield() {
            if (routine != null)
                StopCoroutine(routine);

            for (int i = 0; i < _Grid.transform.childCount; i++) {
                Destroy(_Grid.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Calls the required methods to create a new minefield.
        /// </summary>
        public void CreateMinefield() {
            if (Application.isPlaying)
                routine = StartCoroutine(GenerateMinefield());
            else
                GenerateEditorMinefield();
        }

        /// <summary>
        /// Generates the Minefield.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GenerateMinefield() {
            DestroyMinefield();

            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            // Backend initalization

            Progress.SetActive(true);
            Slider slider = Progress.GetComponentInChildren<Slider>();
            slider.value = 0;
            slider.maxValue = tiles.Length;

            BoxCollider collider = _Grid.GetComponent<BoxCollider>();
            collider.size = new Vector3(TotalSize.x + 30, TotalSize.y + 30, TotalSize.z + 2);

            // Update the camera location to reflect new grid
            camera.GetComponent<Control.CameraControls>().CenterCamera();

            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            for (int y = 0; y < MINEFIELD_HEIGHT; y++) {
                layers[y] = new GameObject("Layer " + (y + 1)); // Instantiate parent object of following cubes.
                layers[y].transform.SetParent(_Grid.transform);
                for (int x = 0; x < MINEFIELD_WIDTH; x++) {
                    for (int z = 0; z < MINEFIELD_DEPTH; z++) {
                        // Create the Tile data.
                        Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                        tile.TileIndex = new Vector3(x, y, z);

                        // Instantiate the Tile's GameObject.
                        GameObject tileObj = Instantiate(Cube, new Vector3((x * TilePadding * TileSize), 0 + (y * TilePadding * TileSize), (z * TilePadding * TileSize)), Quaternion.identity, layers[y].transform);
                        Vector3 size = new Vector3(TileSize, TileSize, TileSize);
                        tileObj.transform.Translate(size); // Offset all of the cubes so that 0,0,0 is the corner.
                        tileObj.GetComponent<Renderer>().sharedMaterial = tile.GetMaterial();

                        // Set key variables and increment slider
                        tileObjects[y, x, z] = tileObj;
                        tiles[y, x, z] = tile;
                        slider.value += 1;

                        yield return null;
                    }
                }
            }

            // Deactivate slider once finished
            Progress.SetActive(false);
        }

        /// <summary>
        /// Non-coroutine version of GenerateMinefield() for attmepted use in the Editor when not playing.
        /// </summary>
        public void GenerateEditorMinefield() {
            DestroyMinefield();

            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            // Update the camera location to reflect new grid
            camera.GetComponent<Control.CameraControls>().CenterCamera();

            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            for (int y = 0; y < MINEFIELD_HEIGHT; y++) {
                layers[y] = new GameObject("Layer " + (y + 1)); // Instantiate parent object of following cubes.
                layers[y].transform.SetParent(_Grid.transform);
                for (int x = 0; x < MINEFIELD_WIDTH; x++) {
                    for (int z = 0; z < MINEFIELD_DEPTH; z++) {
                        // Create the Tile data.
                        Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                        tile.TileIndex = new Vector3(x, y, z);

                        // Instantiate the Tile's GameObject.
                        GameObject tileObj = Instantiate(Cube, new Vector3((x * TilePadding * TileSize), 0 + (y * TilePadding * TileSize), (z * TilePadding * TileSize)), Quaternion.identity, layers[y].transform);
                        Vector3 size = new Vector3(TotalSize.x / 2, TotalSize.y / 2, TotalSize.z / 2);
                        tileObj.transform.Translate(-size); // Offset all of the cubes so that 0,0,0 is the center.
                        tileObj.GetComponent<Renderer>().sharedMaterial = tile.GetMaterial();

                        // Set key variables and increment slider
                        tileObjects[y, x, z] = tileObj;
                        tiles[y, x, z] = tile;
                    }
                }
            }
        }

        public int MinefieldWidth {
            get {
                return MINEFIELD_WIDTH;
            }

            set {
                MINEFIELD_WIDTH = value;
            }
        }

        public float TotalWidth {
            get {
                return (MINEFIELD_WIDTH * TileSize * TilePadding);
            }
        }

        public int MinefieldHeight {
            get {
                return MINEFIELD_HEIGHT;
            }

            set {
                MINEFIELD_HEIGHT = value;
            }
        }

        public float TotalHeight {
            get {
                return (MINEFIELD_HEIGHT * TileSize * TilePadding);
            }
        }

        public int MinefieldDepth {
            get {
                return MINEFIELD_DEPTH;
            }

            set {
                MINEFIELD_DEPTH = value;
            }
        }

        public float TotalDepth {
            get {
                return (MINEFIELD_DEPTH * TileSize * TilePadding);
            }
        }


        public Vector3 TotalSize {
            get {
                return new Vector3(TotalWidth, TotalHeight, TotalDepth);
            }
        }

        public Vector3 Middle {
            get {
                return new Vector3(TotalWidth / 2, TotalHeight / 2, TotalDepth / 2);
            }
        }
    }

    [CreateAssetMenu(fileName = "New Tile", menuName = "Minesweeper/Tile", order = 1)]
    public class Tile : ScriptableObject {
        public Color color;
        private Vector3 tileIndex;

        private Material mat;

        public Material GetMaterial() {
            if (mat == null) {
                mat = new Material(Shader.Find("VertexLit"));
                mat.color = color;
            }
            return mat;
        }

        public Vector3 TileIndex {
            get {
                return tileIndex;
            }

            set {
                tileIndex = value;
            }
        }

    }
}