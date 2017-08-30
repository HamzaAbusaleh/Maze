using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Implementation;

namespace WebMaze.Models.Interface
{
    public interface IMazeActions
    {
        /// <summary>
        /// Map the maze into a string array so that it's used for drawing it on the view
        /// </summary>
        /// <param name="maze">The maze generated</param>
        /// <param name="withSolution">A flag to allow or prevent drawing the solution</param>
        /// <returns>return a two dimensional array string which represents the maze </returns>
        string[,] MapMazeToArray(Maze maze,bool withSolution = false);

        /// <summary>
        /// Fetch all the neighbors who exists and have not been visited already for the Cell
        /// </summary>
        /// <param name="maze">The maze generated</param>
        /// <param name="currentLocation">cell to get neighbors</param>
        /// <returns></returns>
        List<Tuple<int, int>> FetchNeighborCells(Maze maze, Cell currentLocation);
    }
}
