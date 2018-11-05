using Minesweeper;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Minesweeper {
    public class Tile : MonoBehaviour {
        [SerializeField] private Color color;
        [SerializeField] private bool isMine;
        [SerializeField] private Minefield field;
        [SerializeField] private Material material;
        [SerializeField] private Vector3 tileIndex;        

        public Tile(Minefield field, Color color, Vector3 tileIndex) {
            this.field = field;
            _Color = color;
            _TileIndex = tileIndex;
            CreateMaterial();
        }

        public Tile(Minefield field, Color color, Vector3 tileIndex, Material material) {
            this.field = field;
            _Color = color;
            _TileIndex = tileIndex;
            this.material = material;
        }

        private void CreateMaterial() {
            material = new Material(Shader.Find("VertexLit"));
            material.color = color;
        }

        private void CreateMaterial(Shader shader) {
            material = new Material(shader);
            material.color = color;
        }

        public void CreateTile() {
            CreateMaterial();
        }

        public int GetAdjacentMines() {
            int mines = 0;
            foreach (Tile t in GetAdjacent())
                if (t.isMine)
                    mines++;
            return mines;
        }
        
        public List<Tile> GetAdjacent() {
            List<Tile> adjacent = new List<Tile>();
            Tile tile = null;
            // Current Layer
            if ((tile = field.GetTile(tileIndex + new Vector3(0, -1, 0)))  != null) adjacent.Add(tile); // Left
            if ((tile = field.GetTile(tileIndex + new Vector3(0, 1, 0)))   != null) adjacent.Add(tile); // Right
            if ((tile = field.GetTile(tileIndex + new Vector3(0, 0, 1)))   != null) adjacent.Add(tile); // Top
            if ((tile = field.GetTile(tileIndex + new Vector3(0, 0, -1)))  != null) adjacent.Add(tile); // Bottom
            if ((tile = field.GetTile(tileIndex + new Vector3(0, -1, 1)))  != null) adjacent.Add(tile); // Top-left
            if ((tile = field.GetTile(tileIndex + new Vector3(0, 1, 1)))   != null) adjacent.Add(tile); // Top-right
            if ((tile = field.GetTile(tileIndex + new Vector3(0, -1, -1))) != null) adjacent.Add(tile); // Bottom-left
            if ((tile = field.GetTile(tileIndex + new Vector3(0, 1, -1)))  != null) adjacent.Add(tile); // Bottom-right
            // Above Layer
            if ((tile = field.GetTile(tileIndex + new Vector3(1, -1, 0))) != null) adjacent.Add(tile); // Left
            if ((tile = field.GetTile(tileIndex + new Vector3(1, 1, 0))) != null) adjacent.Add(tile); // Right
            if ((tile = field.GetTile(tileIndex + new Vector3(1, 0, 1))) != null) adjacent.Add(tile); // Top
            if ((tile = field.GetTile(tileIndex + new Vector3(1, 0, -1))) != null) adjacent.Add(tile); // Bottom
            if ((tile = field.GetTile(tileIndex + new Vector3(1, -1, 1))) != null) adjacent.Add(tile); // Top-left
            if ((tile = field.GetTile(tileIndex + new Vector3(1, 1, 1))) != null) adjacent.Add(tile); // Top-right
            if ((tile = field.GetTile(tileIndex + new Vector3(1, -1, -1))) != null) adjacent.Add(tile); // Bottom-left
            if ((tile = field.GetTile(tileIndex + new Vector3(1, 1, -1))) != null) adjacent.Add(tile); // Bottom-right
            // Below Layer
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, -1, 0))) != null) adjacent.Add(tile); // Left
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, 1, 0))) != null) adjacent.Add(tile); // Right
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, 0, 1))) != null) adjacent.Add(tile); // Top
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, 0, -1))) != null) adjacent.Add(tile); // Bottom
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, -1, 1))) != null) adjacent.Add(tile); // Top-left
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, 1, 1))) != null) adjacent.Add(tile); // Top-right
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, -1, -1))) != null) adjacent.Add(tile); // Bottom-left
            if ((tile = field.GetTile(tileIndex + new Vector3(-1, 1, -1))) != null) adjacent.Add(tile); // Bottom-right
            return adjacent;
        }

        public void PlantFlag() {
             _Color = Color.green;
            CreateMaterial();
            GetComponent<Renderer>().material = material;
        }

        public void Clear() { // TODO
            if(GetAdjacentMines() > 0) {
                // TODO Create text
            }
        }

        public bool IsMine {
            get {
                return isMine;
            }
            set {
                isMine = value;
            }
        }

        public Color _Color {
            get {
                if (color == null)
                    color = Color.white;
                return color;
            }
            set {
                color = value;
            }
        }

        public Vector3 _TileIndex {
            get {
                return tileIndex;
            }
            set {
                tileIndex = value;
            }
        }

        public Material _Material {
            get {
                if (material == null)
                    CreateMaterial();
                return material;
            }
        }

        public Minefield Field {
            set {
                field = value;
            }
        }

        public override string ToString() {
            return "[TileIndex: " + tileIndex.ToString() + ", Color: " + color.ToString() + ", IsMine: " + isMine + ", Adjacent: " + GetAdjacent().Count + ", AdjacentMines: " + GetAdjacentMines() + "]";
        }

    }
}