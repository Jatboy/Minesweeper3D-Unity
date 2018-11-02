using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper;

namespace Minesweeper {
    public class Minefield : MonoBehaviour {
        public GameObject _Grid;
        public GameObject Cube;
        public GameObject Progress;

        public float MineCount = 10;
        public float TilePadding = 2.5f;
        public float TileSize = 1f;
        public Tile[] tilePrefabs;

        private int MINEFIELD_WIDTH = 8;
        private int MINEFIELD_HEIGHT = 8;
        private int MINEFIELD_DEPTH = 8;

        private Tile[,,] tiles;
        private Coroutine routine;
        private GameObject[,,] tileObjects;
        
        void Start() {
            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
        }

        public void DestroyMinefield() {
            if(routine != null)
                StopCoroutine(routine);
            
            for (int i = 0; i < _Grid.transform.childCount; i++) {
                Destroy(_Grid.transform.GetChild(i).gameObject);
            }
        }

        public void CreateMinefield() {
            if (Application.isPlaying)
                routine = StartCoroutine(GenerateMinefield());
            else
                GenerateEditorMinefield();
        }

        public IEnumerator GenerateMinefield() {
            DestroyMinefield();

            tiles = new Tile[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];
            tileObjects = new GameObject[MINEFIELD_HEIGHT, MINEFIELD_WIDTH, MINEFIELD_DEPTH];

            Progress.SetActive(true);
            Slider slider = Progress.GetComponentInChildren<Slider>();
            slider.value = 0;
            slider.maxValue = tiles.Length;

            float equationX = MINEFIELD_WIDTH * TilePadding * TileSize;
            float equationY = MINEFIELD_HEIGHT * TilePadding * TileSize;
            float equationZ = MINEFIELD_DEPTH * TilePadding * TileSize;
            _Grid.transform.position = new Vector3(equationX / 2, equationY / 2, equationZ / 2);

            int y = 0, x = 0, z = 0;
            GameObject[] layers = new GameObject[MINEFIELD_HEIGHT];
            while (true) {
                if (layers[y] == null) {
                    layers[y] = new GameObject("Layer " + (y + 1));
                    layers[y].transform.SetParent(_Grid.transform);
                }

                Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                tile.SetTileIndex(new Vector3(x, y, z));

                GameObject tileObj = Instantiate(Cube, new Vector3(x * TilePadding * TileSize, (y + 1) * TilePadding * TileSize, z * TilePadding * TileSize), Quaternion.identity, layers[y].transform);
                tileObj.GetComponent<Renderer>().material = tile.GetMaterial();

                tileObjects[y,x,z] = tileObj;
                tiles[y,x,z] = tile;

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

                slider.value += 1;
                yield return null;
            }

            Progress.SetActive(false);
        }

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

        public void SetMinefieldWidth(int width) {
            MINEFIELD_WIDTH = width;
        }

        public int GetMinefieldWidth() {
            return MINEFIELD_WIDTH;
        }

        public void SetMinefieldHeight(int height) {
            MINEFIELD_HEIGHT = height;
        }

        public int GetMinefieldHeight() {
            return MINEFIELD_HEIGHT;
        }

        public void SetMinefieldDepth(int depth) {
            MINEFIELD_DEPTH = depth;
        }

        public int GetMinefieldDepth() {
            return MINEFIELD_DEPTH;
        }

    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "New Tile", menuName = "Minesweeper/Tile", order = 1)]
    public class Tile : ScriptableObject {
        public Color color;
        private Vector3 tileIndex;

        public Material GetMaterial() {
            Material mat = new Material(Shader.Find("VertexLit"));
            mat.color = color;
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