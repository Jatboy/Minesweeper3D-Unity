using Minesweeper;
using UnityEngine;

namespace Minesweeper {
    public class Tile : MonoBehaviour {
        [SerializeField] private Color color;
        [SerializeField] private bool isMine;
        [SerializeField] private Material material;
        [SerializeField] private Vector3 tileIndex;

        public Tile(Color color, Vector3 tileIndex) {
            _Color = color;
            _TileIndex = tileIndex;
            CreateMaterial();
        }

        public Tile(Color color, Vector3 tileIndex, Material material) {
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

        public int CheckAdjacent(Minefield field) {
            int adjacent = 0;
            // Iterate recursively
            return adjacent;
        }

        public void UpdateTile(int action) {
            switch (action) {
                case 0: // LMB
                    _Color = Color.red;
                    CreateMaterial();
                    GetComponent<Renderer>().material = material;
                    break;
                case 1: // RMB
                    _Color = Color.black;
                    CreateMaterial();
                    GetComponent<Renderer>().material = material;
                    break;
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

    }
}