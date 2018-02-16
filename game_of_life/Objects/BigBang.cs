using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_of_life {
    class BigBang {
        private World world;
        private InitializationMode initMode;

        public struct InitializationParams {
            public int worldSize {
                get; set;
            }
            public double cellSpreadModifier {
                get; set;
            }
        }

        public enum InitializationMode {
            STANDARD,
            CLEAR
        }

        public BigBang() {
            world = new World();
            initMode = InitializationMode.STANDARD;
            CreateCells();
        }

        public BigBang(int size) {
            world = new World(size);
            initMode = InitializationMode.STANDARD;
            CreateCells();
        }

        public BigBang(InitializationMode initMode) {
            world = new World();
            this.initMode = initMode;
            CreateCells();
        }

        public BigBang(int size, InitializationMode initMode) {
            world = new World(size);
            this.initMode = initMode;
            CreateCells();
        }

        public BigBang(InitializationParams initParams, InitializationMode initMode) {
            world = new World(initParams.worldSize);
            this.initMode = initMode;
            CreateCells(initParams.cellSpreadModifier);
        }

        private void CreateCells() {
            CreateCells(0.5);
        }

        private void CreateCells(double modifier) {
            Cell[,] cells = new Cell[world.GetSize(), world.GetSize()];
            Random rnd = new Random();

            for(int i = 0; i < world.GetSize(); i++) {
                for(int j = 0; j < world.GetSize(); j++) {
                    switch(initMode) {
                        default:
                        case InitializationMode.STANDARD:
                            cells[i, j] = new Cell(i, j, rnd.NextDouble() >= modifier ? true : false);
                            break;
                        case InitializationMode.CLEAR:
                            cells[i, j] = new Cell(i, j);
                            break;
                    }
                }
            }

            world.AddCells(cells);
        }

        public World GetNewWorld() {
            return world;
        }
    }
}