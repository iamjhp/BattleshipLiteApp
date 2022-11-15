using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary.Models {
    public class GridModel {
        public string CellLetter { get; set; }
        public int CellNumber { get; set; }
        public GridCellStatus Status { get; set; } = GridCellStatus.Empty;
    }
}
