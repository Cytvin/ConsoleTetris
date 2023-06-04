namespace Tetris.Figures
{
    public class Figure : IFigure
    {
        protected List<Block> _blocks;
        protected Block _center;
        public IEnumerable<Block> Blocks => _blocks;

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

        public void Rotate()
        {
            if (_center == null)
            {
                return;
            }

            foreach (var block in _blocks)
            {
                if (block != _center)
                {
                    int[] newCoordinate = RotationSystem.Rotate(block.X, block.Y, _center.X, _center.Y);

                    int newX = newCoordinate[0];
                    int newY = newCoordinate[1];

                    block.SetCoordinate(newX, newY);
                }
            }
        }
    }
}
