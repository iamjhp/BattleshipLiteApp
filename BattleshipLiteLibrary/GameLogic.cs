using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary.Models;

namespace BattleshipLiteLibrary {
    public class GameLogic {

        public static void InitializeGrid(PlayerModel model) {
            List<string> letters = new List<string> {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int> {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters) {
                foreach (int number in numbers) {
                    AddGridCell(model, letter, number);
                }
            }
        }

        private static void AddGridCell(PlayerModel model, string letter, int number) {
            GridModel cell = new GridModel();
            cell.CellLetter = letter;
            cell.CellNumber = number;

            model.ShotGrid.Add(cell);
        }

        public static bool PlaceShip(PlayerModel model, string location) {
            bool output = false;
            (string letter, int number) = SplitShotIntoLetterAndNumber(location);

            bool isValidLocation = ValidateGridLocation(model, letter, number);
            bool isCellFree = ValidateShipLocation(model, letter, number);

            if (isValidLocation && isCellFree) {
                model.ShipLocations.Add(new GridModel {
                    CellLetter = letter.ToUpper(),
                    CellNumber = number,
                    Status = GridCellStatus.Ship
                });

                output = true;
            }

            return output;
        }

        private static bool ValidateShipLocation(PlayerModel model, string letter, int number) {
            bool isValidLocation = true;

            foreach (GridModel ship in model.ShipLocations) {
                if (ship.CellLetter == letter.ToUpper() && ship.CellNumber == number) {
                    isValidLocation = false;
                    break;
                }
            }

            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerModel model, string letter, int number) {
            bool isValidLocation = true;

            foreach (GridModel ship in model.ShotGrid) {
                if (ship.CellLetter == letter.ToUpper() && ship.CellNumber == number) {
                    isValidLocation = true;
                    break;
                }
            }

            return isValidLocation;
        }

        public static bool PlayerStillActive(PlayerModel player) {
            bool isActive = false;

            foreach (GridModel ship in player.ShipLocations) {
                if (ship.Status != GridCellStatus.Sunk) {
                    isActive = true;
                    break;
                }
            }

            return isActive;
        }

        public static (string letter, int number) SplitShotIntoLetterAndNumber(string shot) {
            if (shot.Length != 2) {
                throw new ArgumentException("Wrong shot type!");
            }

            char[] shotArr = shot.ToArray();
            string letter = shotArr[0].ToString();
            int number = int.Parse(shotArr[1].ToString());

            return (letter, number);
        }

        public static bool ValidateShot(PlayerModel player, string letter, int number) {
            bool isValidShot = false;

            foreach (GridModel cell in player.ShotGrid) {
                if (cell.CellLetter == letter.ToUpper() && cell.CellNumber == number) {
                    if (cell.Status == GridCellStatus.Empty) {
                        isValidShot = true;
                    }
                }
            }

            return isValidShot;
        }

        public static bool IdentifyShotResult(PlayerModel opponent, string letter, int number) {
            bool isHit = false;

            foreach (GridModel ship in opponent.ShipLocations) {
                if (ship.CellLetter == letter.ToUpper() && ship.CellNumber == number) {
                    ship.Status = GridCellStatus.Sunk;
                    isHit = true;
                    break;
                }
            }

            return isHit;
        }

        public static void MarkShotResult(PlayerModel player, string letter, int number, bool isHit) {
            foreach (GridModel cell in player.ShotGrid) {
                if (cell.CellLetter == letter.ToUpper() && cell.CellNumber == number) {
                    if (isHit) {
                        cell.Status = GridCellStatus.Hit;
                    } else {
                        cell.Status = GridCellStatus.Miss;
                    }
                }
            }
        }
    }
}
