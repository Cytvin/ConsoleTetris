using Microsoft.VisualBasic;

namespace Tetris.Figures
{
    public class Figure : IFigure
    {
        protected List<Block> _blocks;
        protected Block _center;
        public IEnumerable<Block> Blocks => _blocks;
        public bool IsPlaced { get; private set; } = false;

        public Figure()
        {
            _blocks = new List<Block>();
        }

        public static IFigure MakeFigure(FigureTypes type)
        {
            switch (type)
            {
                case FigureTypes.O:

                    return new OFigure();
                case FigureTypes.I:

                    return new LineFigure();
                case FigureTypes.T:

                    return new TFigure();
                case FigureTypes.J:

                    return new JFigure();
                case FigureTypes.L:

                    return new LFigure();
                case FigureTypes.Z:

                    return new ZFigure();
                case FigureTypes.S:

                    return new SFigure();
                default:
                    throw new ArgumentException("Неверный тип фигуры");
            }
        }

        public void MoveDown(IEnumerable<Block> placedBlock, int fieldHeight)
        {
            if(TryMoveDown(placedBlock, fieldHeight) == false)
            {
                IsPlaced = true;
                return;
            }

            foreach (var block in _blocks)
            {
                block.MoveDown();
            }
        }

        private bool TryMoveDown(IEnumerable<Block> placedBlock, int fieldHeight)
        {
            foreach (var block in _blocks)
            {
                if (block.Y == fieldHeight - 1)
                {
                    return false;
                }

                foreach (var lyingBlock in placedBlock)
                {
                    if (lyingBlock.X == block.X && lyingBlock.Y == block.Y + 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void MoveInDirection(IEnumerable<Block> placedBlocks, int direction, int fieldWidth)
        {
            if (TryMoveInDirection(placedBlocks, direction, fieldWidth) == false)
            {
                return;
            }

            foreach (var block in _blocks)
            {
                block.MoveInDirection(direction);
            }

            IsPlaced = false;
        }

        private bool TryMoveInDirection(IEnumerable<Block> placedBlocks, int direction, int fieldWidth)
        {
            foreach (var block in _blocks)
            {
                if (direction > 0)
                {
                    if (block.X == fieldWidth - 1)
                    {
                        return false;
                    }
                }
                else if (direction < 0)
                {
                    if (block.X == 0)
                    {
                        return false;
                    }
                }

                foreach (var lyingBlock in placedBlocks)
                {
                    if (lyingBlock.X == block.X + direction && lyingBlock.Y == block.Y)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Rotate(IEnumerable<Block> placedBlocks, int fieldWidth, int fieldHeight)
        {
            if (TryRotate(placedBlocks, fieldWidth, fieldHeight) == false)
            {
                return;
            }

            foreach (var block in _blocks)
            {
                if (block != _center)
                {
                    int[] newCoordinate = RotationSystem.GetNewCoordinate(block.X, block.Y, _center.X, _center.Y);

                    int newX = newCoordinate[0];
                    int newY = newCoordinate[1];

                    block.SetCoordinate(newX, newY);
                }
            }

            IsPlaced = false;
        }

        private bool TryRotate(IEnumerable<Block> placedBlocks, int fieldWidth, int fieldHeight)
        {
            if (_center == null)
            {
                return false;
            }

            foreach (var block in _blocks)
            {
                if (block != _center)
                {
                    int[] newCoordinate = RotationSystem.GetNewCoordinate(block.X, block.Y, _center.X, _center.Y);

                    int newX = newCoordinate[0];
                    int newY = newCoordinate[1];

                    if (newY > fieldHeight - 1)
                    {
                        return false;
                    }

                    if (newX > fieldWidth - 1 || newX < 0)
                    {
                        return false;
                    }

                    foreach (var placedBlock in placedBlocks)
                    {
                        if (placedBlock.X == newX && placedBlock.Y == newY)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
