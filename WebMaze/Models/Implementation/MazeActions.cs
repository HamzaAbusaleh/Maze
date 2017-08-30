using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Interface;

namespace WebMaze.Models.Implementation
{
    public class MazeActions : IMazeActions
    {
        /// <summary>
        /// Fetch all the neighbors who exists and have not been visited already for the Cell
        /// </summary>
        /// <param name="maze">The maze generated</param>
        /// <param name="currentLocation">cell to get neighbors</param>
        /// <returns></returns>
        public List<Tuple<int, int>> FetchNeighborCells(Maze maze, Cell currentLocation)
        {
            int tempRow = currentLocation.RowIndex;
            int width = maze.Width;
            int height = maze.Height;
            var mazeArray = maze.MazeArray;
            List<Tuple<int, int>> availablePlaces = new List<Tuple<int, int>>();

            // Left
            int tempCol = currentLocation.ColIndex - 1;
            if (tempCol >= 0 && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            // Right
            tempCol = currentLocation.ColIndex + 1;
            if (tempCol < width && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }

            // UpcolIndex;
            tempCol = currentLocation.ColIndex;
            tempRow = currentLocation.RowIndex - 1;
            if (tempRow >= 0 && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            // Down
            tempRow = currentLocation.RowIndex + 1;
            if (tempRow < height && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            return availablePlaces;
        }

        /// <summary>
        /// Check if the cell wall status are intact or not 
        /// </summary>
        /// <param name="mazeArray">the maze array</param>
        /// <param name="cell">The cell to check</param>
        /// <returns></returns>
        private bool CheckAllWallsIntact(Cell[,] mazeArray, Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!mazeArray[cell.RowIndex, cell.ColIndex][i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Map the maze into a string array so that it's used for drawing it on the view
        /// </summary>
        /// <param name="maze">The maze generated</param>
        /// <param name="withSolution">A flag to allow or prevent drawing the solution</param>
        /// <returns>return a two dimensional array string which represents the maze </returns>
        public string[,] MapMazeToArray(Maze maze, bool withSolution = false)
        {
            var width = maze.Width;
            var height = maze.Height;
            var generatedMazeArray = maze.MazeArray;
            var pathSolution = maze.PathSolution;
            var mazeArray = new string[height * 2 + 1, width * 2 + 1];

            int row = 0;
            int col = 0;
            for (int i = 0; i < width; i++)
            {
                for (int b = 0; b < height; b++)
                {
                    var x = DisplayCell(generatedMazeArray[b, i]);

                    for (int r = 0; r < 3; r++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            mazeArray[row + k, col + r] = x[k, r];
                        }
                    }

                    if (withSolution)
                    {
                        var solutionPathResult =
                            pathSolution.FirstOrDefault(currentMaze => currentMaze.ColIndex == generatedMazeArray[b, i].ColIndex &&
                                                                        currentMaze.RowIndex == generatedMazeArray[b, i].RowIndex);
                        if (solutionPathResult != null && solutionPathResult.IsSolution)
                        {
                            mazeArray[row + 1, col + 1] = solutionPathResult.IsSolution ? "X" : "";
                        }
                    }


                    row = row + 2;
                }

                col = col + 2;
                row = 0;
            }

            return mazeArray;

        }

        private string[,] DisplayCell(Cell cell)
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
            if (cell.UpWall) x[0, 1] = "---";
            if (cell.LeftWall) x[1, 0] = "|";
            if (cell.RightWall) x[1, 2] = "|";
            if (cell.DownWall) x[2, 1] = "---";

            return x;
        }
    }
}
