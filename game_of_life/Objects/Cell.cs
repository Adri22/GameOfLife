using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace game_of_life {
    class Cell {
        private int posX, posY;
        private bool dead;
        private List<Cell> siblings;

        public Cell(int posX, int posY, bool dead) {
            this.posX = posX;
            this.posY = posY;
            this.dead = dead;
            siblings = new List<Cell>();
        }

        public Cell(int posX, int posY) {
            this.posX = posX;
            this.posY = posY;
            dead = true;
            siblings = new List<Cell>();
        }

        public Point GetPosition() {
            return new Point(posX, posY);
        }

        public int GetNumberOfSiblings() {
            return siblings.Count;
        }

        public void AddSibling(Cell sibling) {
            siblings.Add(sibling);
        }

        public void RemoveSibling(Cell sibling) {
            siblings.Remove(sibling);
        }

        public bool ContainsSibling(Cell sibling) {
            return siblings.Contains(sibling);
        }

        public void Revive() {
            dead = false;
        }

        public void Kill() {
            dead = true;
        }

        public bool IsDead() {
            return dead;
        }
    }
}