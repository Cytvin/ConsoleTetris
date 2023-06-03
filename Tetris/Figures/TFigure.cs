
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
            _blocks.Add(new Block(4, 0));
            _blocks.Add(new Block(5, 0));
            _blocks.Add(new Block(6, 0));
            _blocks.Add(new Block(5, 1));
        }
    }
}
