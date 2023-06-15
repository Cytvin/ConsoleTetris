namespace Tetris.Figures
{
    public interface IFigure
    {
        IEnumerable<Block> Blocks { get; }

        void DeleteBlock();
        void Move();
        void MoveLeft();
        void MoveRight();
        void Rotate(IEnumerable<Block> placedBlocks, int fieldWidth, int fieldHeight);
    }
}