using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minesweeper;

namespace Minesweeper {
    public class Minefield : MonoBehaviour {
        public GameObject _Grid;
        public float MineCount = 10;
        public float TilePadding = 2.5f;
        public float TileSize = 1f;
        public Vector3 MinefieldSize = new Vector3(8, 8, 8);
        public Tile[] tilePrefabs;

        private Tile[] tiles;
        private GameObject[] tileObjects;

        void Start() {
            tiles = new Tile[(int)MinefieldSize.x * (int)MinefieldSize.y];
            tileObjects = new GameObject[(int) MinefieldSize.x * (int) MinefieldSize.y];
        }

        public void DestroyMinefield() {
            for (int i = 0; i < _Grid.transform.childCount; i++) {
                Destroy(_Grid.transform.GetChild(i).gameObject);
            }
        }

        public void CreateMinefield() {
            StartCoroutine(GenerateMinefield());
        }

        public IEnumerator GenerateMinefield() {
            DestroyMinefield();

            int y = 0, x = 0, z = 0, tileIndex = 0;
            GameObject[] layers = new GameObject[(int) MinefieldSize.y];
            while (true) {
                if (layers[y] == null) {
                    layers[y] = new GameObject("Layer " + (y + 1));
                    layers[y].transform.SetParent(_Grid.transform);
                }

                Tile tile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tileObj.transform.position = new Vector3(x * TilePadding * TileSize, y * TilePadding * TileSize, z * TilePadding * TileSize);
                tileObj.transform.SetParent(layers[y].transform);
                tileObj.GetComponent<Renderer>().material.color = tile.color;
                tileObjects[tileIndex] = tileObj;

                tiles[tileIndex] = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                tiles[tileIndex].SetTileIndex(tileIndex);

                x++;
                if (x == MinefieldSize.x) {
                    x = 0;
                    y++;
                }
                if (y == MinefieldSize.y) {
                    y = 0;
                    z++;
                }
                if (z == MinefieldSize.z)
                    break;
                tileIndex = (int) (MinefieldSize.x * y + x);
                //Debug.Log("Layer " + y + " | Column " + x + " | Row " + z);

                yield return null;
            }
        }

    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "New Tile", menuName = "Minesweeper/Tile", order = 1)]
    public class Tile : ScriptableObject {
        public Color color;
        private int tileIndex;

        public void SetTileIndex(int _tileIndex) {
            tileIndex = _tileIndex;
        }

        public int GetTileIndex() {
            return tileIndex;
        }

    }
}