namespace tictactoe
{
    internal class Program
    {
        static char playerChar = 'X';
        static char minimaxChar = 'O';

        static void Main(string[] args)
        {
            char[] board = new char[9];

            // True - player start
            // False - computer start
            bool turn = true;

            for (int i = 0; i < 9; i++)
                board[i] = ' ';

            while (true)
            {
                DrawBoards(board);

                var symbolChar = turn ? playerChar : minimaxChar;

                if (turn)
                {
                    Console.Write($"Pick slot to put {symbolChar} in range 1 to 9: ");
                    var isValidNumber = int.TryParse(Console.ReadLine(), out int slot);
                    while (!isValidNumber || slot < 1 || slot > 9 || board[slot - 1] != ' ')
                    {
                        Console.Write($"Pick slot properly this time: ");
                        isValidNumber = int.TryParse(Console.ReadLine(), out slot);
                    }

                    slot--;
                    board[slot] = turn ? playerChar : minimaxChar;
                    turn = !turn;
                }
                else
                {
                    int slot = BestMove(board);
                    board[slot] = turn ? playerChar : minimaxChar;
                    turn = !turn;
                }


                if (HasWon(board, symbolChar))
                {
                    DrawBoards(board);
                    Console.WriteLine($"{symbolChar} has won!");
                    return;
                }

                if (DrawCheck(board))
                {
                    DrawBoards(board);
                    Console.WriteLine("No winner this game!");
                    return;
                }

            }
        }

        private static void DrawBoards(char[] board)
        {
            Console.Clear();
            Console.WriteLine("Board:");
            Console.Write("---+---+---\n");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($" {board[i]} |");
                if (i == 2 || i == 5 || i == 8)
                    Console.Write("\n---+---+---\n");
            }

            Console.WriteLine("\nSlots:");
            Console.Write("---+---+---\n");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($" {i + 1} |");
                if (i == 2 || i == 5 || i == 8)
                    Console.Write("\n---+---+---\n");
            }
        }

        private static bool HasWon(char[] board, char symbol)
        {
            var t = symbol;
            if(// Rows
               (board[0] == t && board[1] == t && board[2] == t) ||
               (board[3] == t && board[4] == t && board[5] == t) ||
               (board[6] == t && board[7] == t && board[8] == t) ||
               // Collumns
               (board[0] == t && board[3] == t && board[6] == t) ||
               (board[1] == t && board[4] == t && board[7] == t) ||
               (board[2] == t && board[5] == t && board[8] == t) ||
               // Cross
               (board[0] == t && board[4] == t && board[8] == t) ||
               (board[2] == t && board[4] == t && board[6] == t))
            {
                return true;
            }

            return false;
        }

        private static bool DrawCheck(char[] board)
        {
            for (int i = 0; i < 9; i++)
                if (board[i] == ' ')
                    return false;

            return true;
        }

        private static int MiniMax(char[] _board, bool maximazing, int depth)
        {
            char[] board = (char[])_board.Clone();

            if (HasWon(board, minimaxChar))
                return 10;
            else if (HasWon(board, playerChar))
                return -10;
            else if (DrawCheck(board))
                return 0;

            if (maximazing)
            {
                int max = int.MinValue;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == ' ')
                    {
                        int returner = MiniMax(MakeMoveAtSlot(board, false, i), false, depth + 1);
                        max = Math.Max(max, returner - depth);
                    }
                }
                return max;
            }
            else
            {
                int min = int.MaxValue;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == ' ')
                    {
                        int returner = MiniMax(MakeMoveAtSlot(board, true, i), true, depth + 1);
                        min = Math.Min(min, returner);
                    }
                }
                return min;
            }
        }

        static char[] MakeMoveAtSlot(char[] _board, bool whosMoving, int slot)
        {
            char[] board = (char[])_board.Clone();
            board[slot] = whosMoving ? playerChar : minimaxChar;
            return board;
        }

        static int BestMove(char[] _board)
        {
            int max = int.MinValue;
            int slot = int.MinValue;
            char[] board = (char[])_board.Clone();
            for (int i = 0; i < board.Length; i++)
                if (board[i] == ' ')
                {
                    int returner = MiniMax(MakeMoveAtSlot(board, false, i), false, 0);
                    if (returner > max)
                    {
                        max = returner;
                        slot = i;
                    }
                }

            return slot;
        }
    }
}