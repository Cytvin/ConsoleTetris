namespace Tetris.Figures
{
    public class JFigure : Figure
    {
        public JFigure() 
        {
            Initialze();
        }

        private void Initialze()
        {
            _blocks.Add(new Block(4, 0));
            _blocks.Add(new Block(4, 1));
            _blocks.Add(new Block(5, 1));
            _blocks.Add(new Block(6, 1));

            _center = _blocks[2];
        }
    }
}
