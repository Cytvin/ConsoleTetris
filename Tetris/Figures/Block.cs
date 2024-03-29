﻿namespace Tetris.Figures
{
    public class Block
    {
        private int _x;
        private int _y;

        public char Symbol => '*';
        public int X => _x;
        public int Y => _y;

        public Block(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void MoveDown()
        {
            _y++;
        }

        public void MoveInDirection(int direction)
        {
            _x += direction;
        }

        public void SetCoordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
