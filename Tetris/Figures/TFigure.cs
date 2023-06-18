namespace Tetris.Figures
{
    public class TFigure : Figure
    {
        public TFigure() 
        {
            Initialize();
        }

        private void Initialize()
        {
            _blocks.Add(new Block(4, 2));
            _blocks.Add(new Block(5, 2));
            _blocks.Add(new Block(6, 2));
            _blocks.Add(new Block(5, 3));

            _center = _blocks[1];

            char[,] preview =
            {
                { _blockSymbol, _blockSymbol, _blockSymbol, ' '},
                { ' ', _blockSymbol, ' ', ' '}
            };

            Preview = preview;
        }
    }
}
