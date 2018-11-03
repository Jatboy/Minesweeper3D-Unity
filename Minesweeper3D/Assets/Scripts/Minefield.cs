using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper;

namespace Minesweeper {

    [RequireComponent(typeof(BoxCollider))]
    public class Minefield : MonoBehaviour {
        public GameObject Grid;
        public GameObject Prefab; 
        public GameObject Progress;

        public float MineCount = 10;
        public float TilePadding = 4f; 
        public float TileSize = 1f;

        private int MINEFIELD_WIDTH = 8;
        private int MINEFIELD_HEIGHT = 8;
        private int MINEFIELD_DEPTH = 8;

        private Slider slider;
        private Tile[,,] tiles;
        private Coroutine routine;
        private GameObject[,,] tileObjects;

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

            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            for (int y = 0; y < MINEFIELD_HEIGHT; y++) {
                layers[y] = new GameObject("Layer " + (y + 1));
                layers[y].transform.SetParent(Grid.transform);
                for (int x = 0; x < MINEFIELD_WIDTH; x++) {
                    for (int z = 0; z < MINEFIELD_DEPTH; z++) {
                        Vector3 pos = new Vector3((x * TilePadding * TileSize), 0 + (y * TilePadding * TileSize), (z * TilePadding * TileSize));
                        GameObject tileObj = Instantiate(Prefab, pos, Quaternion.identity, layers[y].transform);
                        Tile tile = tileObj.AddComponent<Tile>();
                        tile._Color = Random.ColorHSV();
                        tile._TileIndex = pos;
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
            Camera.main.GetComponent<CameraControls>().CenterCamera();
            InitUI(false);
        }

        private void InitUI(bool isStart) {
            if(isStart) {
                if (slider == null)
                    slider = Progress.GetComponentInChildren<Slider>();
                Progress.SetActive(true);
                slider.value = 0;
                slider.maxValue = tiles.Length;
            } else {
                Progress.SetActive(false);
            }
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