using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

namespace BattleshipLite {
    public class Program {
        static void Main(string[] args) {
            WelcomeMessage();

            PlayerModel activePlayer = CreatePlayer("Player 1");
            PlayerModel opponent = CreatePlayer("Player 2");
            PlayerModel winner = null;

            while (winner == null) {

                // Display grid from the active Player
                DisplayShotGrid(activePlayer);

                // Take player's shot
                RecordPlayerShot(activePlayer, opponent);

                // Check if the game is over
                bool isPlayerAlive = GameLogic.PlayerStillActive(opponent);

                if (isPlayerAlive) {
                    (activePlayer, opponent) = (opponent, activePlayer);
                } else {
                    winner = activePlayer;
                }
            }

            GetWinner(winner);
        }

        private static void GetWinner(PlayerModel winner) {
            Console.WriteLine($"The winner is { winner.PlayerName }");
        }

        private static void RecordPlayerShot(PlayerModel activePlayer, PlayerModel opponent) {
            string row = "";
            int col = 0;
            
            while (true) {
                Console.WriteLine($"{ activePlayer.PlayerName }'s turn");
                string shot = AskForShot();
                (row, col) = GameLogic.SplitShotIntoLetterAndNumber(shot);
                bool isValidShot = GameLogic.ValidateShot(activePlayer, row, col);

                if (isValidShot) {
                    break;
                }

                Console.WriteLine("Invalid shot location. Try again.");
            }

            // Determin shot results
            bool isHit = GameLogic.IdentifyShotResult(opponent, row, col);

            // Record results
            GameLogic.MarkShotResult(activePlayer, row, col, isHit);
        }

        private static string AskForShot() {
            Console.WriteLine("Enter your shot selection:");
            string input = Console.ReadLine();
            return input;
        }

        private static void DisplayShotGrid(PlayerModel activePlayer) {
            foreach (GridModel cell in activePlayer.ShotGrid) {
                if (cell.Status == GridCellStatus.Empty) {
                    Console.Write($" {cell.CellLetter}{cell.CellNumber} ");
                } else if (cell.Status == GridCellStatus.Hit) {
                    Console.Write(" XX ");
                } else if (cell.Status == GridCellStatus.Miss) {
                    Console.Write(" 00 ");
                }

                if (cell.CellNumber == 5) {
                    Console.WriteLine();
                }
            }
        }

        private static void WelcomeMessage() {
            Console.WriteLine("----Battleship Lite----");
            Console.WriteLine("creatd by JHP");
            Console.WriteLine();
        }

        private static PlayerModel CreatePlayer(string player) {
            PlayerModel model = new PlayerModel();

            Console.WriteLine($"Player information for {player}");

            // Ask for player's name
            model.PlayerName = AskForPlayerName();

            // Create a new shot grid for the player
            GameLogic.InitializeGrid(model);

            // Ask the player for his 5 ship placements
            PlaceShips(model);

            Console.Clear();

            return model;
        }

        private static string AskForPlayerName() {
            Console.WriteLine("What's your name?");
            string input = Console.ReadLine();

            return input;
        }

        private static void PlaceShips(PlayerModel model) {
            while (model.ShipLocations.Count < 5) {
                Console.WriteLine($"Where do you want to place ship number {model.ShipLocations.Count + 1} (e.g. E1; between A-E and 1-5): ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if (isValidLocation) {
                } else {
                    Console.WriteLine("Invalid location. Please try again.");
                }
            }
        }
    }



    
}
