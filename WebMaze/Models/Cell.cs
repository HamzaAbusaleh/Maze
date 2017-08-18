using System;

namespace WebMaze.Models
{
    /// <summary>
    /// Represents a maze cell
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// Initializes a new instance of maze cell with locations
        /// </summary>
        public unsafe Cell(int row, int col)
        {
            this.RowIndex = row;
            this.ColIndex = col;
            // initially, all walls are intact
            this.LeftWall = true;
            this.RightWall = true;
            this.UpWall = true;
            this.DownWall = true;
            this.Path = Paths.None;

            // must be initialized, since it is a member of a struct
            this.Visited = false;
            this.Previous = null;
            this.IsSolution = false;
        }

        /// <summary>
        /// Gets or sets a value whether the cell has an intact left wall
        /// </summary>
        public bool LeftWall;

        /// <summary>
        /// /// Gets or sets a value whether the cell has an intact right wall
        /// </summary>
        public bool RightWall;

        /// <summary>
        /// Gets or sets a value whether the cell has an intact up wall
        /// </summary>
        public bool UpWall;

        /// <summary>
        /// Gets or sets a value whether the cell has an intact down wall
        /// </summary>
        public bool DownWall;

        /// <summary>
        /// Gets or sets a value whether the cell has been visited already
        /// </summary>
        public bool Visited;


        public enum Paths
        {
            Up, Down, Right, Left, None
        }

        public Paths Path;

        /// <summary>
        /// Gets or sets a pointer to the previous Cell in the found path chain
        /// </summary>
        public unsafe Cell* Previous;

        public bool IsSolution { get; set; }
        /// <summary>
        /// Provides indexing to the boolean fields in the cell
        /// </summary>
        /// <param name="index">0 leftW, 1 rightW, 2 UpW, 3 downW, 4 visited</param>
        /// <returns></returns>
        public bool this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.LeftWall;
                    case 1:
                        return this.RightWall;
                    case 2:
                        return this.UpWall;
                    case 3:
                        return this.DownWall;
                    case 4:
                        return this.Visited;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.LeftWall = value;
                        break;
                    case 1:
                        this.RightWall = value;
                        break;
                    case 2:
                        this.UpWall = value;
                        break;
                    case 3:
                        this.DownWall = value;
                        break;
                    case 4:
                        this.Visited = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string[,] DisplayCell()
        {

            var x = new string[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int k = 0; k < 3; k++)
                {
                    x[r, k] = " ";

                }

            }

            x[1, 1] = "  ";

            if (this.UpWall) x[0, 1] = "---";
            if (this.LeftWall) x[1, 0] = "|";
            if (this.RightWall) x[1, 2] = "|";
            if (this.DownWall) x[2, 1] = "---";

            return x;

        }
        /// <summary>
        /// Reset a cell so that all walls are intact and not visited
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < 4; i++)
            {
                this[i] = true;
            }
            this.Visited = false;
        }

        public int ColIndex;

        public int RowIndex;

    }
}
