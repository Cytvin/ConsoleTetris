namespace Tetris.Figures
{
    public class SFigure : Figure
    {
        public SFigure()
        {
            Initilize();
        }

        private void Initilize()
        {
            _blocks.Add(new Block(6, 0));
            _blocks.Add(new Block(5, 0));
            _blocks.Add(new Block(5, 1));
            _blocks.Add(new Block(4, 1));

            _center = _blocks[1];
        }
    }
}
