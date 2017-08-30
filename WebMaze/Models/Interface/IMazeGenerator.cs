using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Implementation;

namespace WebMaze.Models.Interface
{
    public interface IMazeGenerator
    {
        /// <summary>
        /// Generate a maze with the Depth-First Search approach
        /// </summary>
        /// <param name="maze">The maze</param>
        Result<Cell[,]> DepthFirstSearchMazeGeneration(Maze maze);

        /// <summary>
        /// Generate the maze start point
        /// </summary>
        /// <returns></returns>
        Cell GenerateMazeStartPoint(Maze maze);

        /// <summary>
        /// Generate the maze end point
        /// </summary>
        /// <returns></returns>
        Cell GenerateMazeEndPoint(Maze maze);

    }
}
