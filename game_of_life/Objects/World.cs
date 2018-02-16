using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace game_of_life {
    class World {
        private int size;
        private Shape[,] fields;
        private CellGeneration cellGen;

        public World() {
            size = 100; // TODO: make it variable at runtime
            InitWorld();
        }

        public World(int size) {
            this.size = size;
            InitWorld();
        }

        private void InitWorld() {
            fields = new Shape[size, size];

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    fields[i, j] = new Rectangle(); // TODO: ellipse / rectangle?
                }
            }
        }

        public int GetSize() {
            return size;
        }

        public Shape GetField(int x, int y) {
            return fields[x, y];
        }

        public void AddCells(Cell[,] cells) {
            cellGen = new CellGeneration(cells);
        }

        public CellGeneration GetCells() {
            return cellGen;
        }
    }
}