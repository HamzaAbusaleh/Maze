using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Implementation;

namespace WebMaze.Models.Interface
{
    public interface IMazeSolver
    {
        /// <summary>
        /// Solving the maze using the Iterative depth first
        /// </summary>
        /// <param name="maze">The maze entity</param>
        /// <param name="start">Maze start cell</param>
        /// <param name="end">Maze end cell</param>
        /// <returns>return result that contain the state of the process with the path if exists</returns>
        Result<List<Cell>> SolveWithIterativeDepthFirst(Maze maze,Cell start, Cell end);
    }
}
