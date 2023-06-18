namespace Tetris.Figures
{
    public interface IFigure
    {
        IEnumerable<Block> Blocks { get; }
        bool IsPlaced { get; }
        char[,] Preview { get; }

        void MoveDown(IEnumerable<Block> placedBlock, int fieldHeight);
        void MoveInDirection(IEnumerable<Block> placedBlocks, int direction, int fieldWidth);
        void Rotate(IEnumerable<Block> placedBlocks, int fieldWidth, int fieldHeight);
    }
}