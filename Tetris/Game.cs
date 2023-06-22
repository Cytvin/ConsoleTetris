using System.Diagnostics.Contracts;
using Tetris.Figures;

namespace Tetris
{
    public class Game
    {
        private char[,] _field;
        private List<IFigure> _plasedFigures;
        private List<Block> _placedBlocks;
        private IFigure _currentFigure;
        private IFigure _nextFigure;
        private Input _input;
        private Random _random;

        private int _height;
        private int _width;
        private int _score;
        private int _level;
        private int _removedLinesCounter;
        private int _gameSpeed;
        private int _waitTime;

        public event Action<char[,], int, int, char[,]>? GameChanged;
        public event Action<string>? GameOver;

        public bool IsGameOver => !TryPlaceNewFigure();

        public Game(int height, int width)
        {
            _height = height;
            _width = width;
            _score = 0;
            _level = 1;
            _removedLinesCounter = 0;
            _gameSpeed = 500;
            _waitTime = 500;

            _field = new char[height, width];
            _plasedFigures = new List<IFigure>();
            _placedBlocks = new List<Block>();
            _input = new Input();
            _random = new Random();
            _input.KeyPressed += MoveCurrentFigureByInput;
            _currentFigure = Figure.MakeFigure(GetNextFigureType(_random));
            _nextFigure = Figure.MakeFigure(GetNextFigureType(_random));

            InitializingField(height, width);
        }

        public void Play()
        {
            Thread inputTread = new Thread(_input.ListenInput);
            inputTread.Start();

            while (true)
            {
                if (_level > 10)
                {
                    GameOver?.Invoke("!!!!! Game  Done !!!!!");
                    break;
                }

                if (IsGameOver)
                {
                    GameOver?.Invoke("***** Game  Over *****");
                    break;
                }

                if (_currentFigure.IsPlaced)
                {
                    Thread.Sleep(_waitTime);

                    if (_currentFigure.IsPlaced)
                    {
                        AddFigureToPlaced(_currentFigure);
                        FindingFullLine();

                        _currentFigure = _nextFigure;
                        _nextFigure = Figure.MakeFigure(GetNextFigureType(_random));
                        continue;
                    }
                }

                MoveCurrentFigureDown();

                Thread.Sleep(_gameSpeed);
            }

            _input.KeyPressed -= MoveCurrentFigureByInput;
        }

        private char[,] GetField()
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

        private FigureTypes GetNextFigureType(Random random)
        {
            return (FigureTypes)random.Next(7);
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
                MoveCurrentFigureDown();
                return;
            }
        }

        private void MoveCurrentFigureDown()
        {
            _currentFigure.MoveDown(_placedBlocks, _height);
            GameChanged?.Invoke(GetField(), _score, _level, _nextFigure.Preview);
        }

        private void MoveCurrentFigureInDirection(int direction)
        {
            _currentFigure.MoveInDirection(_placedBlocks, direction, _width);
            GameChanged?.Invoke(GetField(), _score, _level, _nextFigure.Preview);
        }

        private void RotateCurrentFigure()
        {
            _currentFigure.Rotate(_placedBlocks, _width, _height);   
            GameChanged?.Invoke(GetField(), _score, _level, _nextFigure.Preview);
        }

        private bool TryPlaceNewFigure()
        {
            foreach (var block in _placedBlocks)
            {
                if (block.Y < 4)
                {
                    return false;
                }
            }

            return true;
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

            GameChanged?.Invoke(GetField(), _score, _level, _nextFigure.Preview);
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
                _waitTime -= 25;
            }
        }
    }
}