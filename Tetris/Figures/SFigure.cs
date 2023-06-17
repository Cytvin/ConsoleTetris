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
            _blocks.Add(new Block(6, 2));
            _blocks.Add(new Block(5, 2));
            _blocks.Add(new Block(5, 3));
            _blocks.Add(new Block(4, 3));

            _center = _blocks[1];
        }
    }
}
