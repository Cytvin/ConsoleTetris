namespace Tetris.Figures
{
    public interface IFigure
    {
        IEnumerable<Block> Blocks { get; }

        void MoveDown();
        void MoveInDirection(int direction, int fieldWidth, IEnumerable<Block> placedBlocks);
        void Rotate(IEnumerable<Block> placedBlocks, int fieldWidth, int fieldHeight);
    }
}