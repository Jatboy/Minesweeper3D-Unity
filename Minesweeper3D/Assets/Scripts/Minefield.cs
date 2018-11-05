using Minesweeper;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Minesweeper {

    [RequireComponent(typeof(BoxCollider))]
    public class Minefield : MonoBehaviour {
        public GameObject Grid;
        public GameObject Prefab; 
        public GameObject Progress;
        public GameObject TextPrefab;

        public float MineCount = 10;
        public float TilePadding = 3f; 
        public float TileSize = 1f;

        private GameObject WorldCanvas;
        private GameObject CanvasObj;

        private int MINEFIELD_WIDTH = 8;
        private int MINEFIELD_HEIGHT = 8;
        private int MINEFIELD_DEPTH = 8;

        private Slider slider;
        private Tile[,,] tiles;
        private Coroutine routine;
        private GameObject[,,] tileObjects;

        private void Start() {
            CanvasObj = GameObject.Find("Canvas (ScreenSpace)");
            WorldCanvas = GameObject.Find("Canvas (WorldSpace)");
        }

        private void InitUI(bool isStart) {
            if (isStart) {
                if (slider == null)
                    slider = Progress.GetComponentInChildren<Slider>();
                Progress.SetActive(true);
                slider.value = 0;
                slider.maxValue = tiles.Length;
            }
            else {
                Progress.SetActive(false);
            }
        }

        public void CreateMinefield() {
            routine = StartCoroutine(GenerateMinefield());
        }

        public void DestroyMinefield() {
            if (routine != null)
                StopCoroutine(routine);

            for (int i = 1; i < Grid.transform.childCount; i++) {
                Destroy(Grid.transform.GetChild(i).gameObject);
            }
        }

        private IEnumerator GenerateMinefield() {
            DestroyMinefield();

            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            InitUI(true);
            Camera.main.GetComponent<CameraControls>().CenterCamera();

            // Calculate mine coordinates
            List<Vector3> mineCoordinates = new List<Vector3>();
            while(mineCoordinates.Count < MineCount) {
                Vector3 coordinate = new Vector3(Random.Range(0, MINEFIELD_WIDTH), Random.Range(0, MINEFIELD_HEIGHT), Random.Range(0, MINEFIELD_DEPTH));
                if(!mineCoordinates.Contains(coordinate))
                    mineCoordinates.Add(coordinate);
            }

            // Create the minefield
            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            for (int y = 0; y < MINEFIELD_HEIGHT; y++) {
                layers[y] = new GameObject("Layer " + (y + 1));
                layers[y].transform.SetParent(Grid.transform);
                for (int x = 0; x < MINEFIELD_WIDTH; x++) {
                    for (int z = 0; z < MINEFIELD_DEPTH; z++) {
                        Vector3 pos = new Vector3((x * TilePadding * TileSize), 0 + (y * TilePadding * TileSize), (z * TilePadding * TileSize));
                        GameObject tileObj = Instantiate(Prefab, pos, Quaternion.identity, layers[y].transform);
                        Tile tile = tileObj.AddComponent<Tile>();
                        tile.Field = this;
                        if (mineCoordinates.Contains(new Vector3(x, y, z))) {
                            tile._Color = Color.red;
                            tile.IsMine = true;
                        }else
                            tile._Color = Color.white;
                        tile._TileIndex = new Vector3(y, x, z);
                        tile.CreateTile();

                        Vector3 size = new Vector3(TileSize, TileSize, TileSize);
                        tileObj.transform.Translate(size);
                        tileObj.GetComponent<Renderer>().sharedMaterial = tile._Material;
                        
                        tileObjects[y, x, z] = tileObj;
                        tiles[y, x, z] = tile;
                        slider.value += 1;

                        yield return null;
                    }
                }
            }

            Grid.transform.GetChild(0).position = Middle;

            CameraControls controls = Camera.main.GetComponent<CameraControls>();
            controls.inputLocked = false;
            controls.CenterCamera();
            InitUI(false);
        }

        public Tile GetTile(Vector3 tileIndex) {
            Tile tile;
            if (InRange(tileIndex))
                tile = tiles[(int)tileIndex.x, (int)tileIndex.y, (int)tileIndex.z];
            else
                tile = null;
            return tile;
        }

        public void DeleteTile(Vector3 tileIndex) {
            Tile tile = tiles[(int)tileIndex.x, (int)tileIndex.y, (int)tileIndex.z];
            //tile.Clear();
            tile = null;
        }

        public bool InRange(Vector3 tileIndex) {
            bool inRange = true;
            if (tileIndex.x < 0 || tileIndex.x > MINEFIELD_HEIGHT) inRange = false;
            if (tileIndex.y < 0 || tileIndex.y > MINEFIELD_WIDTH) inRange = false;
            if (tileIndex.z < 0 || tileIndex.z > MINEFIELD_DEPTH) inRange = false;
            return inRange;
        }

        public void SetValues(int mineCount, int tilePadding, int tileSize, float width, float height, float depth) {
            MineCount = mineCount;
            TilePadding = tilePadding;
            TileSize = tileSize;
            MINEFIELD_WIDTH = (int)width;
            MINEFIELD_DEPTH = (int)height;
            MINEFIELD_HEIGHT = (int)depth;
        }

        private Vector3 _MinefieldSize;
        public Vector3 MinefieldSize {
            get {
                if(_MinefieldSize == null || _MinefieldSize == Vector3.zero) {
                    _MinefieldSize = new Vector3(MINEFIELD_WIDTH, MINEFIELD_HEIGHT, MINEFIELD_DEPTH);
                }
                return _MinefieldSize;
            }

            set {
                _MinefieldSize = value;
            }
        }
        
        public Vector3 TotalSize {
            get {
                return new Vector3(MINEFIELD_WIDTH * TileSize * TilePadding, MINEFIELD_HEIGHT * TileSize * TilePadding, MINEFIELD_DEPTH * TileSize * TilePadding);
            }
        }

        public Vector3 Middle {
            get {
                return new Vector3(TotalSize.x / 2, TotalSize.y / 2, TotalSize.z / 2);
            }
        }
    }
}