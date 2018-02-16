using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_of_life {
    class CellGenerationCache {
        private int size;
        private Cell[,,] latestCellGens;

        public CellGenerationCache(int size, int cacheSize) {
            this.size = size;
            latestCellGens = new Cell[size, size, cacheSize];
        }

        public void Add() { // placeholder
        }

        private void Update() { // placeholder
        }





    }
}