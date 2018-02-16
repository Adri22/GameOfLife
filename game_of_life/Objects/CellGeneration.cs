using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace game_of_life {
    class CellGeneration {
        private Cell[,] cells;

        public CellGeneration(Cell[,] cells) {
            this.cells = cells;
        }

        public Cell GetCell(int x, int y) {
            return cells[x, y];
        }

        private void CheckSiblings() {
            for(int i = 0; i < cells.GetLength(0); i++) {
                for(int j = 0; j < cells.GetLength(1); j++) {
                    for(int x = -1; x < 2; x++) {
                        for(int y = -1; y < 2; y++) {
                            Cell sibling = cells[
                                i + x == -1 ? cells.GetLength(0) - 1 : (i + x) % cells.GetLength(0),
                                j + y == -1 ? cells.GetLength(1) - 1 : (j + y) % cells.GetLength(1)
                            ];
                            if(cells[i, j] != sibling) {
                                ManageSiblingForCell(ref cells[i, j], ref sibling);
                            }
                        }
                    }
                }
            }
        }

        private void ManageSiblingForCell(ref Cell cell, ref Cell sibling) {
            if(cell.ContainsSibling(sibling) && sibling.IsDead()) {
                cell.RemoveSibling(sibling);
            } else if(!cell.ContainsSibling(sibling) && !sibling.IsDead()) {
                cell.AddSibling(sibling);
            }
        }

        public void Evolve() {
            CheckSiblings();

            for(int i = 0; i < cells.GetLength(0); i++) {
                for(int j = 0; j < cells.GetLength(1); j++) {
                    if(cells[i, j].IsDead()) {
                        if(cells[i, j].GetNumberOfSiblings() == 3) {
                            cells[i, j].Revive();
                        }
                    } else {
                        if(cells[i, j].GetNumberOfSiblings() < 2) {
                            cells[i, j].Kill();
                        } else if(cells[i, j].GetNumberOfSiblings() > 3) {
                            cells[i, j].Kill();
                        }
                    }
                }
            }
        }

        public void ReviveCellsAtCoords(List<Point> coords) {
            for(int i = 0; i < coords.Count; i++) {
                try {
                    cells[(int)coords.ElementAt(i).X, (int)coords.ElementAt(i).Y].Revive();
                } catch(IndexOutOfRangeException) {
                    continue;
                }
            }
        }
    }
}