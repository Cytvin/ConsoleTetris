using System.Diagnostics.Contracts;
using Tetris.Figures;

namespace Tetris
{
    public class Game
    {
        private char[,] _field;
        private List<IFigure> _plasedFigures;
        private List<Block> _placedBlocks;
        private IFigure? _lastFigure;
        private Input _input;
        private Random _random;

        private int _height;
        private int _width;
        private int _score;

        public event Action<char[,]>? FieldChanged;
        public bool LastFigureIsNull => _lastFigure == null;
        public bool IsGameOver => GameOver();

        public Game(int height, int width) 
        {
            _height = height;
            _width = width;
            _score = 0;
            _field = new char[height, width];
            _plasedFigures = new List<IFigure>();
            _placedBlocks = new List<Block>();
            _input = new Input();
            _random = new Random();
            _input.KeyPressed += MoveLastFigureByInput;
            _lastFigure = Figure.MakeFigure(GetNextFigureType(_random));

            Initializing(height, width);
        }

        public void Play()
        {
            Thread inputTread = new Thread(_input.ListenInput);
            inputTread.Start();

            bool playing = true;

            while (playing)
            {
                if (LastFigureIsNull)
                {
                    FigureTypes newFigureType = GetNextFigureType(_random);

                    AddFigure(Figure.MakeFigure(newFigureType));
                    FieldChanged?.Invoke(Get());
                }

                playing = !IsGameOver;

                if (playing)
                {
                    MoveLastFigure();

                    FindingFullLine();
                }

                Thread.Sleep(500);
            }
        }

        private char[,] Get()
        {
            Clear();

            if (LastFigureIsNull == false)
            {
                foreach (var block in _lastFigure.Blocks)
                {
                    _field[block.Y, block.X] = block.Symbol;
                }
            }

            foreach (var block in _placedBlocks) 
            {
                _field[block.Y, block.X] = block.Symbol;
            }

            return _field;
        }

        private void AddFigure(IFigure figure)
        {
            _lastFigure = figure;
        }

        private void MoveLastFigureByInput(ConsoleKey key)
        {
            if (key == ConsoleKey.RightArrow)
            {
                MoveLastFigureRight();
                return;
            }
            else if (key == ConsoleKey.LeftArrow) 
            {
                MoveLastFigureLeft();
                return;
            }
            else if (key == ConsoleKey.UpArrow)
            {
                RotateLastFigure();
                return;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                MoveLastFigure();
                return;
            }
        }

        private void MoveLastFigure()
        {
            if (_lastFigure == null)
            {
                return;
            }

            if (TryMove(_lastFigure) == false)
            {
                AddFigureToPlaced(_lastFigure);
                _lastFigure = null;
                return;
            }

            _lastFigure.Move();
            FieldChanged?.Invoke(Get());
        }

        private void MoveLastFigureRight()
        {
            if (_lastFigure == null)
            {
                return;
            }

            if (TryMoveRight(_lastFigure) == false)
            {
                return;
            }

            _lastFigure.MoveRight();
            FieldChanged?.Invoke(Get());
        }

        private void MoveLastFigureLeft()
        {
            if (_lastFigure == null)
            {
                return;
            }

            if (TryMoveLeft(_lastFigure) == false)
            {
                return;
            }

            _lastFigure.MoveLeft();
            FieldChanged?.Invoke(Get());
        }

        private bool TryMove(IFigure figure)
        {
            foreach (var block in figure.Blocks) 
            {
                if (block.Y == _height - 1)
                {
                    return false;
                }

                foreach (var lyingBlock in _placedBlocks)
                {
                    if (lyingBlock.X == block.X && lyingBlock.Y == block.Y + 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool TryMoveRight(IFigure figure)
        {
            foreach (var block in figure.Blocks)
            {
                if (block.X == _width - 1)
                {
                    return false;
                }

                foreach (var lyingBlock in _placedBlocks)
                {
                    if (lyingBlock.X == block.X + 1 && lyingBlock.Y == block.Y + 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool TryMoveLeft(IFigure figure)
        {
            foreach (var block in figure.Blocks)
            {
                if (block.X == 0)
                {
                    return false;
                }

                foreach (var lyingBlock in _placedBlocks)
                {
                    if (lyingBlock.X == block.X - 1 && lyingBlock.Y == block.Y + 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void RotateLastFigure()
        {
            if (_lastFigure == null)
            {
                return;
            }

            _lastFigure.Rotate(_placedBlocks, _width, _height);
            
            FieldChanged?.Invoke(Get());
        }

        private bool GameOver()
        {
            foreach (var block in _placedBlocks)
            {
                if (block.Y == 0 && (TryMove(_lastFigure) == false))
                {
                    return true;
                }
            }

            return false;
        }

        private void Clear()
        {
            Initializing(_height, _width);
        }

        private void Initializing(int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _field[i, j] = ' ';
                }
            }
        }

        private void AddFigureToPlaced(IFigure figure)
        {
            _plasedFigures.Add(figure);
            foreach(var block in figure.Blocks)
            {
                _placedBlocks.Add(block);
            }
        }

        private FigureTypes GetNextFigureType(Random random)
        {
            return (FigureTypes)random.Next(Convert.ToInt32(FigureTypes.L));
        }

        private void FindingFullLine()
        {
            int lineNumber = _height - 1;

            while (lineNumber > 4)
            {
                List<Block> removalBlocks = new List<Block>();

                foreach (var block in _placedBlocks)
                {
                    if (block.Y == lineNumber)
                    {
                        removalBlocks.Add(block);
                    }
                }

                if (removalBlocks.Count == _width)
                {
                    DeleteFullLine(removalBlocks, lineNumber);
                }

                lineNumber--;
            }
        }

        private void DeleteFullLine(List<Block> removalBlocks, int lineNumber)
        {
            _placedBlocks = _placedBlocks.Except(removalBlocks).ToList();

            _score += 100;

            foreach(var block in _placedBlocks)
            {
                if (block.Y < lineNumber) 
                {
                    block.Move();
                }
            }

            FieldChanged?.Invoke(Get());
        }
    }
}
