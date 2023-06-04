namespace Tetris.Figures
{
    public class LFigure : Figure
    {
        public LFigure() 
        {
            Initialize();
        }

        private void Initialize()
        {
            _blocks.Add(new Block(6, 0));
            _blocks.Add(new Block(4, 1));
            _blocks.Add(new Block(5, 1));
            _blocks.Add(new Block(6, 1));

            _center = _blocks[2];
        }
    }
}
