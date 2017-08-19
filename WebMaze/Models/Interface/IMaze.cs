using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Implementation;

namespace WebMaze.Models.Interface
{
    public interface IMaze
    {
        /// <summary>
        /// Generate the maze array
        /// </summary>
        Result Generate();

        Result Solve();

        /// <summary>
        /// Generate a maze with the Depth-First Search approach
        /// </summary>
        /// <param name="mazeArray">the array of cells</param>
        Result DepthFirstSearchMazeGeneration(Cell[,] mazeArray);

        /// <summary>
        /// Solving the maze using the Iterative depth first
        /// </summary>
        /// <param name="start">Maze start cell</param>
        /// <param name="end">Maze end cell</param>
        /// <returns>return true when the solution is found</returns>
        /// unsafe shows that the current method is using pointers
        Result SolveWithIterativeDepthFirst(Cell start, Cell end);
    }
}
