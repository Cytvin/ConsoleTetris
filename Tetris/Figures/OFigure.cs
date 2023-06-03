
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
            _blocks.Add(new Block(4, 0));
            _blocks.Add(new Block(5, 0));
            _blocks.Add(new Block(4, 1));
            _blocks.Add(new Block(5, 1));
        }
    }
}
