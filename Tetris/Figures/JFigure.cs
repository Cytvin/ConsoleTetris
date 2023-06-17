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
            _blocks.Add(new Block(4, 2));
            _blocks.Add(new Block(4, 3));
            _blocks.Add(new Block(5, 3));
            _blocks.Add(new Block(6, 3));

            _center = _blocks[2];
        }
    }
}
