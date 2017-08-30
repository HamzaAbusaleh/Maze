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

        Result<Maze> Solve(Maze maze);

    }
}
