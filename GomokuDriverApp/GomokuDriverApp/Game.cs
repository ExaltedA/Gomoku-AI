using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GomokuDriverApp
{


    public class Game
    {

        private Board board;
        private bool isAi1Turn = true;
        private bool gameFinished = false;
        private int minimaxDepth = 4;
        private bool ai1Starts = true; // AI makes the first move
        private XMinimax aiX;
        private OMinimax aiO;
        private int winner; // 0: There is no winner yet, 1: AI Wins, 2: Human Wins


        public Game(Board board)
        {
            this.board = board;
            aiX = new XMinimax(board);
            aiO = new OMinimax(board);
            winner = 0;
        }

        public void start()
        {


            // If the AI 1 is making the first move, place a [0] in the middle of the board.
            if (ai1Starts)
                playMove(board.getBoardSize() / 2, board.getBoardSize() / 2, true);
            // If the AI 2 is making the first move, place a [X] in the middle of the board.
            else
            {
                playMove(board.getBoardSize() / 2, board.getBoardSize() / 2, false);

            }
            // Primary scenario execution
            while (!gameFinished)
            {
                run();
            }
            Console.WriteLine("Simulation Success! Press any key to exit..");
            Console.ReadKey();

        }

        /*
             * 	Sets the depth of the minimax tree. (i.e. how many moves ahead should the AI calculate.)
             * 	and Sets the turn of Ai
             */
        public void setAIDepth(int depth)
        {
            this.minimaxDepth = depth;

        }
        public void setAIStarts(bool aiStarts)
        {
            this.ai1Starts = aiStarts;
        }


        public void run()
        {
            //Clear Console and draw board
            board.BoardDraw();
            // AI 1 LOGIC
            // using Minimax and alpha beta pruning algorithms calculate and return coordinates 
            int[] aiMove = aiX.calculateNextMove(minimaxDepth);
            //Check if it is tie
            if (aiMove == null)
            {
                Console.WriteLine("No possible moves left. Game Over.");

                gameFinished = true;
                return;
            }


            // Place a [0] to the found cell.
            playMove(aiMove[1], aiMove[0], true);

            board.BoardDraw();

            Console.WriteLine("[0]: " + OMinimax.getScore(board, true, true) + " [X]: " + OMinimax.getScore(board, false, true));


            // Check if the last move ends the game.
            winner = checkWinner();

            if (winner == 1)
            {
                Console.WriteLine("AI1 [0] WON!");

                gameFinished = true;
                return;
            }

            // Make the AI instance calculate a move.
            int[] aiMove2 = aiO.calculateNextMove(minimaxDepth);

            if (aiMove2 == null)
            {
                Console.WriteLine("No possible moves left. Game Over.");

                gameFinished = true;
                return;
            }


            // Place a [X] to the found cell.
            playMove(aiMove2[1], aiMove2[0], false);

            board.BoardDraw();

            Console.WriteLine("Black: " + OMinimax.getScore(board, true, true) + " White: " + OMinimax.getScore(board, false, true));

            winner = checkWinner();

            if (winner == 2)
            {
                Console.WriteLine("AI2 [X] WON!");

                gameFinished = true;
                return;
            }

            if (board.generateMoves().Count == 0)
            {
                Console.WriteLine("No possible moves left. Game Over.");

                gameFinished = true;
                return;

            }

            isAi1Turn = true;
        }




        private int checkWinner()
        {
            if (OMinimax.getScore(board, true, false) >= OMinimax.getWinScore()) return 1;
            if (OMinimax.getScore(board, false, true) >= OMinimax.getWinScore()) return 2;
            return 0;
        }
        private bool playMove(int posX, int posY, bool black)
        {
            return board.addStone(posX, posY, black);
        }

    }
}

