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

    }
}
