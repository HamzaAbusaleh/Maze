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
        Result<Maze> Generate(int width,int height);

        /// <summary>
        /// Solve the maze
        /// </summary>
        /// <param name="maze">The maze</param>
        /// <returns>Return the result with the Maze object</returns>
        Result<Maze> Solve(Maze maze);

    }
}
