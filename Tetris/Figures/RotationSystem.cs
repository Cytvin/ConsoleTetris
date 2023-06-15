namespace Tetris.Figures
{
    public class RotationSystem
    {
        private static int[,] _movementMatrix =
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 }
        };

        private static int[,] _rotationMatrix =
        {
            { 0, 1, 0 },
            { -1, 0, 0 },
            { 0, 0, 1 }
        };

        public static int[] GetNewCoordinate(int blockX, int blockY, int centerX, int centerY)
        {
            _movementMatrix[2, 0] = centerX * -1;
            _movementMatrix[2, 1] = centerY * -1;

            int[,] transformationMatrix = MatrixMultiplication(_movementMatrix, _rotationMatrix);

            _movementMatrix[2, 0] *= -1;
            _movementMatrix[2, 1] *= -1;

            transformationMatrix = MatrixMultiplication(transformationMatrix, _movementMatrix);

            int[,] currentCoordinate = { { blockX, blockY, 1 } };
            int[,] resultCoordinate = MatrixMultiplication(currentCoordinate, transformationMatrix);

            int[] result = new int[2];
            result[0] = resultCoordinate[0, 0];
            result[1] = resultCoordinate[0, 1];

            return result;
        }

        private static int[,] MatrixMultiplication(int[,] matrixA, int[,] matrixB)
        {
            int matrixARowsCount = matrixA.GetUpperBound(0) + 1;
            int matrixAColumnCount = matrixA.GetUpperBound(1) + 1;
            int matrixBColumnCount = matrixB.GetUpperBound(1) + 1;

            int[,] result = new int[matrixARowsCount, matrixBColumnCount];

            for (int i = 0; i < matrixARowsCount; i++)
            {
                for (int j = 0; j < matrixBColumnCount; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < matrixAColumnCount; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return result;
        }
    }
}
