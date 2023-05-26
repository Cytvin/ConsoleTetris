using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Figure
    {
        private List<Block> _blocks;

        public IEnumerable<Block> Blocks => _blocks;

        public Figure(FigureTypes type)
        {
            _blocks = new List<Block>();

            SetType(type);
        }

        private void SetType(FigureTypes type)
        {
            switch (type) 
            {
                case FigureTypes.O:
                    //Random random = new Random();
                    //int x = random.Next(0, 9);

                    _blocks.Add(new Block(4, 0));
                    _blocks.Add(new Block(5, 0));
                    _blocks.Add(new Block(4, 1));
                    _blocks.Add(new Block(5, 1));

                    break;
            }
        }

        public void Move()
        {
            foreach (var block in _blocks) 
            {
                block.Move();
            }
        }

        public void MoveRight()
        {
            foreach (var block in _blocks)
            {
                block.MoveRight();
            }
        }

        public void MoveLeft()
        {
            foreach (var block in _blocks)
            {
                block.MoveLeft();
            }
        }

        public void DeleteBlock()
        {

        }
    }
}
