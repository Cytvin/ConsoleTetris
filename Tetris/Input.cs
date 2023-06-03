
namespace Tetris
{
    public class Input
    {
        public event Action<ConsoleKey>? KeyPressed;

        public Input() { }

        public void ListenInput()
        {
            while (true) 
            {
                var key = Console.ReadKey(false).Key;
                KeyPressed?.Invoke(key);
            }
        }
    }
}
