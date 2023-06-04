namespace Tetris.Figures
{
    public class ZFigure : Figure
    {
        public ZFigure() 
        {
            Initialize();
        }

        private void Initialize()
        {
            _blocks.Add(new Block(4, 0));
            _blocks.Add(new Block(5, 0));
            _blocks.Add(new Block(5, 1));
            _blocks.Add(new Block(6, 1));

            _center = _blocks[1];
        }
    }
}
