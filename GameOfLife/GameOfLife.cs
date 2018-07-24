using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public struct Cell
    {
        public readonly int X, Y;

        public Cell(int x, int y) { X = x; Y = y; }
    }

    public class Life : IEnumerable<Cell>
    {
        private List<Cell> _cells = new List<Cell>();

        private static readonly int[] Liveables = { 2, 3 };

        public bool Next()
        {
            var died = _cells
                .Where(cell => !Liveables.Contains(Count(cell)))    
                .ToArray();

            var born = _cells
                .SelectMany(Ambit)                                  
                .Distinct()                                         
                .Except(_cells)                                     
                .Where(cell => Count(cell) == 3)                    
                .ToArray();

            if (died.Length == 0 && born.Length == 0)
                return false; 

            _cells = _cells
                .Except(died)
                .Concat(born)
                .ToList();

            return _cells.Any(); 
        }

        private int Count(Cell cell)
        {
            return Ambit(cell)
                .Intersect(_cells)
                .Count();
        }

        private static IEnumerable<Cell> Ambit(Cell cell)
        {
            return Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .Where(y => x != 0 || y != 0) 					
                    .Select(y => new Cell(cell.X + x, cell.Y + y)));
        }

        public override string ToString()
        {
            if (_cells.Count == 0)
                return string.Empty;

            var xmin = _cells.Min(cell => cell.X);
            var xmax = _cells.Max(cell => cell.X);
            var ymin = _cells.Min(cell => cell.Y);
            var ymax = _cells.Max(cell => cell.Y);

            var matrix = Enumerable.Range(xmin, xmax - xmin + 1)
                .Select(x => Enumerable.Range(ymin, ymax - ymin + 1)
                    .Select(y => _cells.Contains(new Cell(x, y))));

            return string.Join(Environment.NewLine,
                matrix.Select(row =>
                    string.Join("",
                        row.Select(b => b ? "X" : "."))));
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(int x, int y)
        {
            _cells.Add(new Cell(x, y));
        }
    }
}
