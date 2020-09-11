using System;
using System.Collections.Generic;
using System.Text;

namespace GomokuDriverApp
{

    public class XMinimax
    {

        public static int evaluationCount = 0;
        private Board board;
        private const int winScore = 100000000;


        public XMinimax(Board board)
        {
            this.board = board;
        }

        public static int getWinScore()
        {
            return winScore;
        }
        public static double evaluateBoardFor(Board board, bool xTurn)
        {
            evaluationCount++;


            double xScore = getScore(board, false, !xTurn);
            double oScore = getScore(board, true, !xTurn);

            if (oScore == 0) oScore = 1.0;

            return xScore / oScore;

        }
        public static int getScore(Board board, bool forX, bool XTurn)
        {


            int[,] boardMatrix = board.getBoardMatrix();
            return evaluateHorizontal(boardMatrix, !forX, XTurn) +
                    evaluateVertical(boardMatrix, !forX, XTurn) +
                    evaluateDiagonal(boardMatrix, !forX, XTurn);
        }

        public int[] calculateNextMove(int depth)
        {

            int[] move = new int[2];
            DateTime startTime = DateTime.Now;
            // Check if any available move can finish the game
            Object[] bestMove = searchWinningMove(board);
            if (bestMove != null)
            {
                Console.WriteLine("Finisher!");
                move[0] = (int)(bestMove[1]);
                move[1] = (int)(bestMove[2]);

            }
            else
            {
                // If there is no such move, search the minimax tree with suggested depth.
                bestMove = minimaxSearchAB(depth, board, true, -1.0, getWinScore());
                if (bestMove[1] == null)
                {
                    move = null;
                }
                else
                {
                    move[0] = (int)(bestMove[1]);
                    move[1] = (int)(bestMove[2]);
                }
            }
            Console.WriteLine("Cases calculated: " + evaluationCount + " Calculation time: " + (DateTime.Now - startTime) + " ms");


            evaluationCount = 0;

            return move;


        }


        /*
         * alpha : Best AI Move (Max)
         * beta : Best enemy Ai Move (Min)
         * returns: {score, move[0], move[1]}
         * */
        private static Object[] minimaxSearchAB(int depth, Board board, bool max, double alpha, double beta)
        {
            if (depth == 0)
            {

                Object[] x = { evaluateBoardFor(board, !max), null, null };
                return x;
            }

            List<int[]> allPossibleMoves = board.generateMoves();

            if (allPossibleMoves.Count == 0)
            {

                Object[] x = { evaluateBoardFor(board, !max), null, null };
                return x;
            }

            Object[] bestMove = new Object[3];


            if (max)
            {
                bestMove[0] = -1.0;
                // Iterate for all possible moves that can be made.
                foreach (int[] move in allPossibleMoves)
                {
                    // Create a temporary board that is equivalent to the current board
                    Board dummyBoard = new Board(board);
                    // Play the move to that temporary board without drawing anything
                    dummyBoard.addStoneNoGUI(move[1], move[0], true);

                    // Call the minimax function for the next depth, to look for a minimum score.
                    Object[] tempMove = minimaxSearchAB(depth - 1, dummyBoard, !max, alpha, beta);

                    // Updating alpha
                    if ((Double)(tempMove[0]) > alpha)
                    {
                        alpha = (Double)(tempMove[0]);
                    }
                    // Pruning with beta
                    if ((Double)(tempMove[0]) >= beta)
                    {
                        return tempMove;
                    }
                    if ((Double)tempMove[0] > (Double)bestMove[0])
                    {
                        bestMove = tempMove;
                        bestMove[1] = move[0];
                        bestMove[2] = move[1];
                    }
                }

            }
            else
            {
                bestMove[0] = 100000000.0;
                bestMove[1] = allPossibleMoves[0][0];
                bestMove[2] = allPossibleMoves[0][1];
                foreach (int[] move in allPossibleMoves)
                {
                    Board dummyBoard = new Board(board);
                    dummyBoard.addStoneNoGUI(move[1], move[0], false);

                    Object[] tempMove = minimaxSearchAB(depth - 1, dummyBoard, !max, alpha, beta);

                    // Updating beta
                    if (((Double)tempMove[0]) < beta)
                    {
                        beta = (Double)(tempMove[0]);
                    }
                    // Pruning with alpha
                    if ((Double)(tempMove[0]) <= alpha)
                    {
                        return tempMove;
                    }
                    if ((Double)tempMove[0] < (Double)bestMove[0])
                    {
                        bestMove = tempMove;
                        bestMove[1] = move[0];
                        bestMove[2] = move[1];
                    }
                }
            }
            return bestMove;
        }

        private static Object[] searchWinningMove(Board board)
        {
            List<int[]> allPossibleMoves = board.generateMoves();
            Object[] winningMove = new Object[3];

            // Iterate for all possible moves
            foreach (int[] move in allPossibleMoves)
            {
                evaluationCount++;
                // Create a temporary board that is equivalent to the current board
                Board dummyBoard = new Board(board);
                // Play the move to that temporary board without drawing anything
                dummyBoard.addStoneNoGUI(move[1], move[0], false);

                // If the O AI has a winning score in that temporary board, return the move.
                if (getScore(dummyBoard, false, false) >= winScore)
                {
                    winningMove[1] = move[0];
                    winningMove[2] = move[1];
                    return winningMove;
                }
            }
            return null;

        }
        public static int evaluateHorizontal(int[,] boardMatrix, bool forX, bool XTurn)
        {

            int consecutive = 0;
            int blocks = 2;
            int score = 0;

            for (int i = 0; i < boardMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < boardMatrix.GetLength(1); j++)
                {
                    if (boardMatrix[i, j] == (forX ? 2 : 1))
                    {
                        consecutive++;
                    }
                    else if (boardMatrix[i, j] == 0)
                    {
                        if (consecutive > 0)
                        {
                            blocks--;
                            score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                            consecutive = 0;
                            blocks = 1;
                        }
                        else
                        {
                            blocks = 1;
                        }
                    }
                    else if (consecutive > 0)
                    {
                        score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                        consecutive = 0;
                        blocks = 2;
                    }
                    else
                    {
                        blocks = 2;
                    }
                }
                if (consecutive > 0)
                {
                    score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);

                }
                consecutive = 0;
                blocks = 2;

            }
            return score;
        }

        public static int evaluateVertical(int[,] boardMatrix, bool forX, bool XTurn)
        {

            int consecutive = 0;
            int blocks = 2;
            int score = 0;

            for (int j = 0; j < boardMatrix.GetLength(1); j++)
            {
                for (int i = 0; i < boardMatrix.GetLength(0); i++)
                {
                    if (boardMatrix[i, j] == (forX ? 2 : 1))
                    {
                        consecutive++;
                    }
                    else if (boardMatrix[i, j] == 0)
                    {
                        if (consecutive > 0)
                        {
                            blocks--;
                            score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                            consecutive = 0;
                            blocks = 1;
                        }
                        else
                        {
                            blocks = 1;
                        }
                    }
                    else if (consecutive > 0)
                    {
                        score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                        consecutive = 0;
                        blocks = 2;
                    }
                    else
                    {
                        blocks = 2;
                    }
                }
                if (consecutive > 0)
                {
                    score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);

                }
                consecutive = 0;
                blocks = 2;

            }
            return score;
        }
        public static int evaluateDiagonal(int[,] boardMatrix, bool forX, bool XTurn)
        {

            int consecutive = 0;
            int blocks = 2;
            int score = 0;
            // From bottom-left to top-right diagonally
            for (int k = 0; k <= 2 * (boardMatrix.GetLength(0) - 1); k++)
            {
                int iStart = Math.Max(0, k - boardMatrix.GetLength(0) + 1);
                int iEnd = Math.Min(boardMatrix.GetLength(0) - 1, k);
                for (int i = iStart; i <= iEnd; ++i)
                {
                    int j = k - i;

                    if (boardMatrix[i, j] == (forX ? 2 : 1))
                    {
                        consecutive++;
                    }
                    else if (boardMatrix[i, j] == 0)
                    {
                        if (consecutive > 0)
                        {
                            blocks--;
                            score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                            consecutive = 0;
                            blocks = 1;
                        }
                        else
                        {
                            blocks = 1;
                        }
                    }
                    else if (consecutive > 0)
                    {
                        score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                        consecutive = 0;
                        blocks = 2;
                    }
                    else
                    {
                        blocks = 2;
                    }

                }
                if (consecutive > 0)
                {
                    score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);

                }
                consecutive = 0;
                blocks = 2;
            }
            // From top-left to bottom-right diagonally
            for (int k = 1 - boardMatrix.GetLength(0); k < boardMatrix.GetLength(0); k++)
            {
                int iStart = Math.Max(0, k);
                int iEnd = Math.Min(boardMatrix.GetLength(0) + k - 1, boardMatrix.GetLength(0) - 1);
                for (int i = iStart; i <= iEnd; ++i)
                {
                    int j = i - k;

                    if (boardMatrix[i, j] == (forX ? 2 : 1))
                    {
                        consecutive++;
                    }
                    else if (boardMatrix[i, j] == 0)
                    {
                        if (consecutive > 0)
                        {
                            blocks--;
                            score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                            consecutive = 0;
                            blocks = 1;
                        }
                        else
                        {
                            blocks = 1;
                        }
                    }
                    else if (consecutive > 0)
                    {
                        score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);
                        consecutive = 0;
                        blocks = 2;
                    }
                    else
                    {
                        blocks = 2;
                    }

                }
                if (consecutive > 0)
                {
                    score += getConsecutiveSetScore(consecutive, blocks, forX == XTurn);

                }
                consecutive = 0;
                blocks = 2;
            }
            return score;
        }
        public static int getConsecutiveSetScore(int count, int blocks, bool currentTurn)
        {
            const int winGuarantee = 1000000;
            if (blocks == 2 && count < 5) return 0;
            switch (count)
            {
                case 5:
                    {
                        return winScore;
                    }
                case 4:
                    {
                        if (currentTurn) return winGuarantee;
                        else
                        {
                            if (blocks == 0) return winGuarantee / 4;
                            else return 200;
                        }
                    }
                case 3:
                    {
                        if (blocks == 0)
                        {
                            if (currentTurn) return 50000;
                            else return 200;
                        }
                        else
                        {
                            if (currentTurn) return 10;
                            else return 5;
                        }
                    }
                case 2:
                    {
                        if (blocks == 0)
                        {
                            if (currentTurn) return 7;
                            else return 5;
                        }
                        else
                        {
                            return 3;
                        }
                    }
                case 1:
                    {
                        return 1;
                    }
            }
            return winScore * 2;
        }
    }

}


