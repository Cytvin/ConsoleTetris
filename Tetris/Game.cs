using System.Diagnostics.Contracts;
using Tetris.Figures;

namespace Tetris
{
    public class Game
    {
        private char[,] _field;
        private List<IFigure> _plasedFigures;
        private List<Block> _placedBlocks;
        private IFigure? _currentFigure;
        private Input _input;
        private Random _random;

        private int _height;
        private int _width;
        private int _score;
        private int _level;
        private int _removedLinesCounter;
        private int _gameSpeed;

        public event Action<char[,], int, int>? GameChanged;
        public bool IsGameOver => GameOver();

        public Game(int height, int width)
        {
            _height = height;
            _width = width;
            _score = 0;
            _level = 1;
            _removedLinesCounter = 0;
            _gameSpeed = 500;

            _field = new char[height, width];
            _plasedFigures = new List<IFigure>();
            _placedBlocks = new List<Block>();
            _input = new Input();
            _random = new Random();
            _input.KeyPressed += MoveCurrentFigureByInput;
            _currentFigure = Figure.MakeFigure(GetNextFigureType(_random));

            InitializingField(height, width);
        }

        public void Play()
        {
            Thread inputTread = new Thread(_input.ListenInput);
            inputTread.Start();

            bool playing = true;

            while (playing)
            {
                if (_currentFigure.IsPlaced)
                {
                    AddFigureToPlaced(_currentFigure);
                    FindingFullLine();

                    SetCurrentFigure(_random);
                    GameChanged?.Invoke(Get(), _score, _level);
                }

                playing = !IsGameOver;

                if (playing)
                {
                    MoveCurrentFigure();

                    Thread.Sleep(_gameSpeed);
                }
            }

            _input.KeyPressed -= MoveCurrentFigureByInput;
        }

        private char[,] Get()
        {
            ClearField();

            foreach (var block in _currentFigure.Blocks)
            {
                _field[block.Y, block.X] = block.Symbol;
            }

            foreach (var block in _placedBlocks) 
            {
                _field[block.Y, block.X] = block.Symbol;
            }

            return _field;
        }

        private void SetCurrentFigure(Random random)
        {
            FigureTypes newFigureType = GetNextFigureType(random);

            _currentFigure = Figure.MakeFigure(newFigureType);
        }

        private void MoveCurrentFigureByInput(ConsoleKey key)
        {
            if (key == ConsoleKey.RightArrow)
            {
                MoveCurrentFigureInDirection(1);
                return;
            }
            else if (key == ConsoleKey.LeftArrow) 
            {
                MoveCurrentFigureInDirection(-1);
                return;
            }
            else if (key == ConsoleKey.UpArrow)
            {
                RotateCurrentFigure();
                return;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                MoveCurrentFigure();
                return;
            }
        }

        private void MoveCurrentFigure()
        {
            if (_currentFigure == null)
            {
                return;
            }

            _currentFigure.MoveDown(_placedBlocks, _height);
            GameChanged?.Invoke(Get(), _score, _level);
        }

        private void MoveCurrentFigureInDirection(int direction)
        {
            if (_currentFigure == null)
            {
                return;
            }

            _currentFigure.MoveInDirection(_placedBlocks, direction, _width);
            GameChanged?.Invoke(Get(), _score, _level);
        }

        private void RotateCurrentFigure()
        {
            if (_currentFigure == null)
            {
                return;
            }

            _currentFigure.Rotate(_placedBlocks, _width, _height);
            
            GameChanged?.Invoke(Get(), _score, _level);
        }

        private bool GameOver()
        {
            foreach (var block in _placedBlocks)
            {
                if (block.Y < 4)
                {
                    return true;
                }
            }

            return false;
        }

        private void ClearField()
        {
            InitializingField(_height, _width);
        }

        private void InitializingField(int height, int width)
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

            while (lineNumber > 3)
            {
                List<Block> removalBlocks = new List<Block>();

                foreach (var block in _placedBlocks.OrderByDescending(b => b.Y))
                {
                    if (block.Y == lineNumber)
                    {
                        removalBlocks.Add(block);
                    }
                }

                if (removalBlocks.Count < _width)
                {
                    lineNumber--;
                    continue;
                }

                DeleteFullLine(removalBlocks, lineNumber);
            }

            GameChanged?.Invoke(Get(), _score, _level);
        }

        private void DeleteFullLine(List<Block> removalBlocks, int lineNumber)
        {
            _placedBlocks = _placedBlocks.Except(removalBlocks).ToList();
            _removedLinesCounter++;

            MoveBlocksAboveRemovedLine(lineNumber);
            IncreasedScore();
            ChangeLevel();
        }

        private void MoveBlocksAboveRemovedLine(int removedLineNumber)
        {
            foreach (var block in _placedBlocks)
            {
                if (block.Y < removedLineNumber)
                {
                    block.MoveDown();
                }
            }
        }

        private void IncreasedScore()
        {
            _score += 100 * _level;
        }

        private void ChangeLevel()
        {
            if (_removedLinesCounter == 10)
            {
                _removedLinesCounter = 0;
                _level++;
                _gameSpeed -= 50;
            }
        }
    }
}