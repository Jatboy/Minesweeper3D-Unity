﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper;

namespace Minesweeper {
    public class Minefield : MonoBehaviour {
        public GameObject _Grid; // Parent object of all Generation code.
        public GameObject Cube; // Cube prefab that all tiles share.
        public GameObject Progress; // Progress bar object to hook into GenerateMinefield()
        public Camera camera;

        public float MineCount = 10; // How many mines should be genereated in the field.
        public float TilePadding = 2.5f; // How much spacing should be added between tiles.
        public float TileSize = 1f; // Size of tiles.
        public Tile[] tilePrefabs; // List of all the possible tile options.

        // X,Y,Z Dimensions of the Minefield
        private int MINEFIELD_WIDTH = 8;
        private int MINEFIELD_HEIGHT = 8;
        private int MINEFIELD_DEPTH = 8;

        private Tile[,,] tiles; // TileData Array
        private Coroutine routine; // Coroutine variable for calling StopCoroutine()
        private GameObject[,,] tileObjects; // Physical Tile Objects

        void Awake()
        {
            transform.position = Vector3.zero;
        }

        /// <summary>
        /// Iterate through children of the _Grid object and delete them.
        /// </summary>
        public void DestroyMinefield() {
            if(routine != null)
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

            // Initalize the arrays.
            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            // Display the progress bar and update its values.
            Progress.SetActive(true);
            Slider slider = Progress.GetComponentInChildren<Slider>();
            slider.value = 0;
            slider.maxValue = tiles.Length;

            // Calculate the end size of the grid and reposition origin point
            _Grid.transform.position = new Vector3(TotalWidth / 2, TotalHeight / 2, TotalDepth / 2);
            camera.GetComponent<Control.CameraControls>().CenterCamera();

            int y = 0, x = 0, z = 0;
            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT]; // Layers to nest objects inside of.
            while (true) {
                if (layers[y] == null) { // Initalize the current layer if it doesn't exist.
                    layers[y] = new GameObject("Layer " + (y + 1));
                    layers[y].transform.SetParent(_Grid.transform);
                }

                // Create tile and its data
                Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                tile.SetTileIndex(new Vector3(x, y, z));

                GameObject tileObj = Instantiate(Cube, new Vector3(x * TilePadding * TileSize, (y + 1) * TilePadding * TileSize, z * TilePadding * TileSize), Quaternion.identity, layers[y].transform);
                tileObj.GetComponent<Renderer>().material = tile.GetMaterial();

                tileObjects[y,x,z] = tileObj;
                tiles[y,x,z] = tile;

                // Increment current position in the array
                x++;
                if (x == MINEFIELD_WIDTH) {
                    x = 0;
                    z++;
                }
                if (z == MINEFIELD_DEPTH) {
                    z = 0;
                    y++;
                }
                if (y == MINEFIELD_HEIGHT)
                    break;

                // Increase slider progress
                slider.value += 1;
                yield return null;
            }

            _Grid.transform.position = Vector3.zero;
            camera.GetComponent<Control.CameraControls>().CenterCamera();
            // Deactivate on completion
            Progress.SetActive(false);
        }

        /// <summary>
        /// Non-coroutine version of GenerateMinefield() for attmepted use in the Editor when not playing.
        /// </summary>
        public void GenerateEditorMinefield() {
            DestroyMinefield();

            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            int y = 0, x = 0, z = 0;
            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            while (true) {
                if (layers[y] == null) {
                    layers[y] = new GameObject("Layer " + (y + 1));
                    layers[y].transform.SetParent(_Grid.transform);
                }

                Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                tile.SetTileIndex(new Vector3(x, y, z));

                GameObject tileObj = Instantiate(Cube, new Vector3(x * TilePadding * TileSize, y * TilePadding * TileSize, z * TilePadding * TileSize), Quaternion.identity, layers[y].transform);
                tileObj.GetComponent<Renderer>().sharedMaterial = tile.GetMaterial();

                tileObjects[y, x, z] = tileObj;
                tiles[y, x, z] = tile;

                x++;
                if (x == MINEFIELD_WIDTH) {
                    x = 0;
                    z++;
                }
                if (z == MINEFIELD_DEPTH) {
                    z = 0;
                    y++;
                }
                if (y == MINEFIELD_HEIGHT)
                    break;
            }
        }

        public int MinefieldWidth
        {
            get
            {
                return MINEFIELD_WIDTH;
            }

            set
            {
                MINEFIELD_WIDTH = value;
            }
        }

        public float TotalWidth
        {
            get
            {
                return (MINEFIELD_WIDTH * TileSize * TilePadding);
            }
        }

        public int MinefieldHeight
        {
            get
            {
                return MINEFIELD_HEIGHT;
            }

            set
            {
                MINEFIELD_HEIGHT = value;
            }
        }

        public float TotalHeight
        {
            get
            {
                return (MINEFIELD_HEIGHT * TileSize * TilePadding);
            }
        }

        public int MinefieldDepth
        {
            get
            {
                return MINEFIELD_DEPTH;
            }

            set
            {
                MINEFIELD_DEPTH = value;
            }
        }

        public float TotalDepth
        {
            get
            {
                return (MINEFIELD_DEPTH * TileSize * TilePadding);
            }
        }
    }

    [System.Serializable]
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

        public void SetTileIndex(Vector3 _tileIndex) {
            tileIndex = _tileIndex;
        }

        public Vector3 GetTileIndex() {
            return tileIndex;
        }

    }
}