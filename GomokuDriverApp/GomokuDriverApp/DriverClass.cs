using System;
using System.Collections.Generic;

namespace GomokuDriverApp
{
    class DriverClass
    {
        static void Main(string[] args)
        {


       
            
            // Create a 15x15 game board.
            Board board = new Board(15);

            // Create the Game manager instance.
            Game game = new Game(board);

            // SET UP DEPTH OF ALGORITHM AND FIRST TURN

            int depth = 4;
            bool Ai1Starts = false;

            Console.WriteLine("Depth: " + depth);

            if (Ai1Starts)
                Console.WriteLine(" AI1 [0] Makes the first move");
            else
                Console.WriteLine(" AI2 [X] Makes the first move");
            

            // Apply the settings.
            game.setAIDepth(depth);
            game.setAIStarts(Ai1Starts);
            Console.WriteLine("Any key to start the Gomoku Simulation");
            Console.ReadKey();

            // Start the game.
            game.start();
            Console.ReadKey();







        }
}
}
