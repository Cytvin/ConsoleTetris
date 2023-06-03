
namespace Tetris.Figures
{
    public class LineFigure : Figure
    {
        FigurePosition _position;

        public LineFigure()
        {
            Initialize();
        }

        private void Initialize()
        {
            _blocks.Add(new Block(4, 0));
            _blocks.Add(new Block(4, 1));
            _blocks.Add(new Block(4, 2));
            _blocks.Add(new Block(4, 3));

            _position = FigurePosition.Vertical;
        }

        public override void Rotate()
        {
            if (_position == FigurePosition.Vertical)
            {
                SetCoordinateForRotate(1);
                
                _position = FigurePosition.Horizontal;
            }
            else
            {
                SetCoordinateForRotate(-1);

                _blocks.Reverse();

                _position = FigurePosition.Vertical;
            }
        }

        private void SetCoordinateForRotate(int multiplier)
        {
            _blocks[0].AddUpToCoordinate(1 * multiplier, 1);
            _blocks[2].AddUpToCoordinate(-1 * multiplier, -1);
            _blocks[3].AddUpToCoordinate(-2 * multiplier, -2);
        }
    }
}
