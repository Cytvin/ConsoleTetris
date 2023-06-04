namespace Tetris
{
    public class Displayer
    {
        public Displayer() { }

        public void DisplayField(char[,] field)
        {
            Console.Clear();

            string frame = CreateFrame(field);

            Console.WriteLine(frame);
        }

        private string CreateFrame(char[,] field)
        {
            string stringField = "";

            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (i == 0 || i == 21)
                    {
                        stringField += "-";
                    }

                    if ((j == 0 || j == 11) && (i != 0 && i != 21))
                    {
                        stringField += "|";
                    }

                    if ((i > 0 && i < 21) && (j > 0 && j < 11))
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
