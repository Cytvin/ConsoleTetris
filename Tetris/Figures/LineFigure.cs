namespace Tetris.Figures
{
    public class LineFigure : Figure
    {
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

            _center = _blocks[2];

            char[,] preview =
            {
                { ' ', ' ', ' ', ' '},
                { _blockSymbol, _blockSymbol, _blockSymbol, _blockSymbol}
            };

            Preview = preview;
        }
    }
}
