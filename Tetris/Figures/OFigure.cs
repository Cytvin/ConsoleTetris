namespace Tetris.Figures
{
    public class OFigure : Figure
    {
        public OFigure() 
        {
            Initialize();
        }

        private void Initialize()
        {
            _blocks.Add(new Block(4, 2));
            _blocks.Add(new Block(5, 2));
            _blocks.Add(new Block(4, 3));
            _blocks.Add(new Block(5, 3));

            char[,] preview =
            {
                { ' ', _blockSymbol, _blockSymbol, ' '},
                { ' ', _blockSymbol, _blockSymbol, ' '}
            };

            Preview = preview;
        }
    }
}
