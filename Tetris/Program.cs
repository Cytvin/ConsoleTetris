using Tetris;

Displayer displayer = new Displayer();
Game game = new Game(24, 10);

game.GameChanged += displayer.Update;

game.Play();

Thread.Sleep(1000);
Console.Clear();
Console.WriteLine("***** ИГРА ОКОНЧЕНА *****");