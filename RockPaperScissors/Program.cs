using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    public static class MoveList
    {
        public static string[] moveList = new string[]
        {
            "rock",
            "paper",
            "scissors"
        };
    }

    class GameSetup
    {
        public int rounds = 0;
        public string opponent = "none";
        string winner;
        string result;

        public string SetPlayers()
        {
            while (opponent == "none")
            {
                Console.WriteLine("\nWould you like to play against another player or a computer?");
                string tempOpponent = Console.ReadLine().ToLower();
                if (tempOpponent == "player" | tempOpponent == "computer")
                {
                    opponent = tempOpponent;
                }
                else
                {
                    Console.WriteLine("Please type either <person> or <computer>.");
                }
            }
            return opponent;
        }

        public void SetRounds()
        {
            while (rounds == 0)
            {
                Console.WriteLine("\nHow many rounds would you like to play?");
                int roundChoice = Convert.ToInt32(Console.ReadLine());
                if (roundChoice >= 1 && roundChoice <= 20)
                {
                    rounds = roundChoice;
                }
                else
                {
                    Console.WriteLine("Please enter a valid choice between 1 and 20 rounds.");
                }
            }
        }

        public string RoundOutcome(string p1Name, string p1Move, string p2Name, string p2Move)
        {
            Console.WriteLine("\n{0} played {1}. {2} played {3}", p1Name, p1Move, p2Name, p2Move);
            if (p1Move == p2Move)
            {
                result = "draw";
            }
            else if (p1Move == "rock" && p2Move == "scissors")
            {
                result = p1Name;
            }
            else if (p1Move == "paper" && p2Move == "rock")
            {
                result = p1Name;
            }
            else if (p1Move == "scissors" && p2Move == "paper")
            {
                result = p1Name;
            }
            else
            {
                result = p2Name;
            }
            Console.WriteLine("The winner was: {0}!", result);
            return result;
        }

        public string GameOutcome(string p1Name, int p1wins, string p2Name, int p2wins)
        {
            if (p1wins > p2wins)
            {
                winner = $"{p1Name} Wins!";
            }
            else if (p1wins < p2wins)
            {
                winner = $"{p2Name} Wins!";
            }
            else
            {
                winner = "It was a draw!";
            }
            return winner;
        }
    }


    class Player
    {
        /* Default name set to Computer to help direct the Choice() method
        this is overridden if a second player joins and selects a name */
        public string name = "Computer";
        string move;
        public int wins = 0;
        public List<string> moveHistory = new List<string>();

        public void SetName()
        {
            while (name == "Computer")
            {
                Console.WriteLine("\nPlayer, what is your name?");
                string tempName = Console.ReadLine();
                /* Makes sure the player cant name themselves 'Computer' to break the system
                 Also stops names with no characters. */
                if (tempName.ToLower() == "computer" | tempName.Length < 2)
                {
                    Console.WriteLine("\nYou cannot call yourself that, sorry.");
                }
                else
                {
                    name = tempName;
                }
            }
        }

        /* Allows Choice() to be called in the ExecuteGame class without wondering
        if the player is a person or the computer.*/
        public string Choice()
        {
            if (name == "Computer")
            {
                return ComputerChoice();
            }
            else
            {
                return PlayerChoice();
            }
        }

        public string PlayerChoice()
        {
            move = "none";
            while (move == "none")
            {
                Console.WriteLine("{0}, please input your move.", name);
                string usermove = Console.ReadLine().ToLower();
                // Checks if the inputted move matches the allowed set of r/p/s
                if (MoveList.moveList.Any(usermove.Contains))
                {
                    move = usermove;
                    moveHistory.Add(move);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please pick one of Rock, Paper or Scissors.");
                }
            }
            return move;
        }

        public string ComputerChoice()
        {
            Random random = new Random();
            move = MoveList.moveList[random.Next(3)];
            moveHistory.Add(move);
            return move;
        }

        public void AddWin()
        {
            wins++;
        }

        public string MostUsed()
        {
            string most = (
                from i in moveHistory
                group i by i into g
                orderby g.Count() descending
                select g.Key).First();

            return most;
        }
    }


    class ExecuteGame
    {
        static void Main(string[] args)
        {
            string winner;
            int roundNumber = 1;

            GameSetup g = new GameSetup();

            Player p = new Player();
            p.SetName();

            /* Initial Player 2, then designate as a computer or
             player based on the users input */ 
            Player p2 = new Player();
            if (g.SetPlayers() == "player")
            {
                p2.SetName();
            }

            g.SetRounds();

            while (g.rounds > 0)
            {
                Console.WriteLine("\nRound {0}", roundNumber.ToString());
                Console.WriteLine("-----------------------");
                roundNumber++;

                winner = g.RoundOutcome(p.name, p.Choice(), p2.name, p2.Choice());
                if (winner == p.name)
                {
                    p.AddWin();
                }
                else if (winner == p2.name)
                {
                    p2.AddWin();
                }
                Console.WriteLine("\n{0}: {1}  |  {2}: {3}", p.name, p.wins, p2.name, p2.wins);
                g.rounds--;
            }
            Console.WriteLine("\nGame Over!");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Winner: {0}", g.GameOutcome(p.name, p.wins, p2.name, p2.wins));
            Console.WriteLine("{0}'s most played move: {1}", p.name, p.MostUsed());
            Console.WriteLine("{0}'s most played move: {1}", p2.name, p2.MostUsed());

            Console.ReadLine();

        }
    }
}
