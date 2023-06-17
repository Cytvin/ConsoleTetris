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
            _blocks.Add(new Block(4, 2));
            _blocks.Add(new Block(5, 2));
            _blocks.Add(new Block(5, 3));
            _blocks.Add(new Block(6, 3));

            _center = _blocks[1];
        }
    }
}
