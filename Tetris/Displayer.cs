namespace Tetris
{
    public class Displayer
    {
        public Displayer() { }

        public void Update(char[,] field, int score, int level)
        {
            Console.Clear();

            DisplayScore(score);
            DisplayLevel(level);
            DisplayField(field);
        }

        private void DisplayField(char[,] field)
        {
            int lineNumber = 0;
            int columnNumber = 10;

            string[] fieldFrame = CreateFrame(field).Split('\n');

            foreach (var frameString in fieldFrame)
            {
                Console.SetCursorPosition(columnNumber, lineNumber);
                Console.WriteLine(frameString);
                lineNumber++;
            }
        }

        private void DisplayScore(int score)
        {
            int lineNumber = 4;
            int columnNumber = 2;

            Console.SetCursorPosition(columnNumber, lineNumber);

            Console.Write("Score: ");
            Console.SetCursorPosition(columnNumber, ++lineNumber);
            Console.Write(score.ToString());
        }

        private void DisplayLevel(int level)
        {
            int lineNumber = 8;
            int columnNumber = 2;

            Console.SetCursorPosition(columnNumber, lineNumber);

            Console.Write("Level: ");
            Console.SetCursorPosition(columnNumber, ++lineNumber);
            Console.Write(level.ToString());
        }

        private string CreateFrame(char[,] field)
        {
            string stringField = "";

            for (int i = 4; i < 26; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (i == 4 || i == 25)
                    {
                        stringField += "-";
                    }

                    if ((j == 0 || j == 11) && (i != 4 && i != 25))
                    {
                        stringField += "|";
                    }

                    if ((j > 0 && j < 11) && (i > 4 && i < 25))
                    {
                        stringField += field[i - 1, j - 1];
                    }
                }

                stringField += "\n";
            }

            return stringField;
        }
    }
}
