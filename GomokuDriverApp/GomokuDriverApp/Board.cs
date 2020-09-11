using System;
using System.Collections.Generic;
using System.Text;

namespace GomokuDriverApp
{
    public class Board
    {

        
        private int[,] boardMatrix; // 0: Empty 1: White 2: Black


        public Board(int boardSize)
        {
            
            boardMatrix = new int[boardSize,boardSize];

        }
        // Fake copy constructor (only copies the boardMatrix)
        public Board(Board board)
        {
            int[,] matrixToCopy = board.getBoardMatrix();
            boardMatrix = new int[matrixToCopy.GetLength(0), matrixToCopy.GetLength(0)];
            for (int i = 0; i < matrixToCopy.GetLength(0); i++)
            {
                for (int j = 0; j < matrixToCopy.GetLength(0); j++)
                {
                    boardMatrix[i,j] = matrixToCopy[i,j];
                }
            }
        }
        public int getBoardSize()
        {
            return boardMatrix.GetLength(0);
        }
        public void addStoneNoGUI(int posX, int posY, bool black)
        {
            boardMatrix[posY, posX] = black ? 2 : 1;
        }
        public bool addStone(int posX, int posY, bool black)
        {

            // Check whether the cell is empty or not
            if (boardMatrix[posY, posX] != 0) return false;

            
            boardMatrix[posY, posX] = black ? 2 : 1;
            return true;

        }
        public List<int[]> generateMoves()
        {
            List<int[]> moveList = new List<int[]>();

            int boardSize = boardMatrix.GetLength(0);

            // Look for cells that has at least one stone in an adjacent cell.
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {

                    if (boardMatrix[i,j] > 0) continue;

                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            if (boardMatrix[i - 1,j - 1] > 0 ||
                               boardMatrix[i,j - 1] > 0)
                            {
                                int[] move = { i, j };
                                moveList.Add(move);
                                continue;
                            }
                        }
                        if (j < boardSize - 1)
                        {
                            if (boardMatrix[i - 1,j + 1] > 0 ||
                               boardMatrix[i,j + 1] > 0)
                            {
                                int[] move = { i, j };
                                moveList.Add(move);
                                continue;
                            }
                        }
                        if (boardMatrix[i - 1, j] > 0)
                        {
                            int[] move = { i, j };
                            moveList.Add(move);
                            continue;
                        }
                    }
                    if (i < boardSize - 1)
                    {
                        if (j > 0)
                        {
                            if (boardMatrix[i + 1, j - 1] > 0 ||
                               boardMatrix[i, j - 1] > 0)
                            {
                                int[] move = { i, j };
                                moveList.Add(move);
                                continue;
                            }
                        }
                        if (j < boardSize - 1)
                        {
                            if (boardMatrix[i + 1,j + 1] > 0 ||
                               boardMatrix[i,j + 1] > 0)
                            {
                                int[] move = { i, j };
                                moveList.Add(move);
                                continue;
                            }
                        }
                        if (boardMatrix[i + 1, j] > 0)
                        {
                            int[] move = { i, j };
                            moveList.Add(move);
                            continue;
                        }
                    }

                }
            }

            return moveList;

        }
        public int[,] getBoardMatrix()
        {
            return boardMatrix;
        }
        //Method that draws an updated board
        public void BoardDraw()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine(string.Concat(System.Linq.Enumerable.Repeat("_", (boardMatrix.GetLength(0) * 2) + 1)));
            for (int i = 0; i < boardMatrix.GetLength(0); i++)
            {
                if (i > 9)
                    Console.Write(i);
                else
                    Console.Write(i + " ");


                for (int j = 0; j < boardMatrix.GetLength(0); j++)
                {


                    if (boardMatrix[i, j] == 1)
                    {
                        Fill("X", ConsoleColor.White);
                    }
                    else if (boardMatrix[i, j] == 2)
                    {
                        Fill("O", ConsoleColor.Red);
                    }
                    else
                        Console.Write("|_");

                }
                Console.WriteLine("|");
            }


        }
        // method of BoardDraw that fills pieces in color
        private void Fill(string cond, ConsoleColor color)
        {
            Console.Write("|");
            Console.ForegroundColor = color;
            Console.Write(cond);
            Console.ForegroundColor = ConsoleColor.DarkYellow;


        }

    }
}
