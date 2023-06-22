using Tetris;

Displayer displayer = new Displayer();
Game game = new Game(24, 10);

game.GameChanged += displayer.Update;
game.GameOver += displayer.DisplayMessage;

game.Play();