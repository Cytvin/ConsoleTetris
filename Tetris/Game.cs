using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Game
    {
        private char[,] _field;
        private List<Figure> _figures;
        private Figure? _lastFigure;
        private Input _input;

        private int _height;
        private int _width;

        public event Action<char[,]>? FieldChanged;
        public bool LastFigureIsNull => _lastFigure == null;
        public bool IsGameOver => GameOver();

        public Game(int height, int width) 
        {
            _height = height;
            _width = width;
            _field = new char[height, width];
            _figures = new List<Figure>();
            _input = new Input();
            _input.KeyPressed += MoveLastFigureByInput;

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
                    AddFigure(new Figure(FigureTypes.O));
                    FieldChanged?.Invoke(Get());
                }

                playing = !IsGameOver;

                if (playing)
                {
                    MoveLastFigure();
                }

                Thread.Sleep(100);
            }
        }

        public char[,] Get()
        {
            Clear();

            foreach (var figure in _figures) 
            {
                foreach (var block in figure.Blocks)
                {
                    _field[block.Y, block.X] = block.Symbol;
                }
            }

            return _field;
        }

        private void AddFigure(Figure figure)
        {
            _figures.Add(figure);
            _lastFigure = figure;
            FieldChanged?.Invoke(Get());
        }

        private void MoveLastFigureByInput(ConsoleKey key)
        {
            if (key == ConsoleKey.RightArrow)
            {
                MoveLastFigureRight();
                return;
            }
            if (key == ConsoleKey.LeftArrow) 
            {
                MoveLastFigureLeft();
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

        private bool TryMove(Figure figure)
        {
            foreach (var block in figure.Blocks) 
            {
                if (block.Y == _height - 1)
                {
                    return false;
                }

                foreach (var lyingFigure in _figures)
                {
                    if (lyingFigure == figure)
                    {
                        continue;
                    }

                    foreach (var lyingBlock in lyingFigure.Blocks)
                    {
                        if (lyingBlock.X == block.X && lyingBlock.Y == block.Y + 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool TryMoveRight(Figure figure)
        {
            foreach (var block in figure.Blocks)
            {
                if (block.X == _width - 1)
                {
                    return false;
                }

                foreach (var lyingFigure in _figures)
                {
                    if (lyingFigure == figure)
                    {
                        continue;
                    }

                    foreach (var lyingBlock in lyingFigure.Blocks)
                    {
                        if (lyingBlock.X == block.X + 1 && lyingBlock.Y == block.Y + 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool TryMoveLeft(Figure figure)
        {
            foreach (var block in figure.Blocks)
            {
                if (block.X == 0)
                {
                    return false;
                }

                foreach (var lyingFigure in _figures)
                {
                    if (lyingFigure == figure)
                    {
                        continue;
                    }

                    foreach (var lyingBlock in lyingFigure.Blocks)
                    {
                        if (lyingBlock.X == block.X - 1 && lyingBlock.Y == block.Y + 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool GameOver()
        {
            foreach (var figure in _figures) 
            {
                foreach (var block in figure.Blocks)
                {
                    if (block.Y == 0 && (TryMove(_lastFigure) == false))
                    {
                        return true;
                    }
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
                    _field[i, j] = '.';
                }
            }
        }
    }
}
