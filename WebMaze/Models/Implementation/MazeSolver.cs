using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Interface;

namespace WebMaze.Models.Implementation
{
    public class MazeSolver : IMazeSolver
    {
        /// <summary>
        /// Solving the maze using the Iterative depth first
        /// </summary>
        /// <param name="maze">The maze entity</param>
        /// <param name="start">Maze start cell</param>
        /// <param name="end">Maze end cell</param>
        /// <returns>return result that contain the state of the process with the path if exists</returns>
        public Result<List<Cell>> SolveWithIterativeDepthFirst(Maze maze)
        {

            Stack<Cell> stack = new Stack<Cell>();
            var pathSolution = new List<Cell>();
            var mazeArray = maze.MazeArray;
            stack.Push(maze.StartPoint);
            
            while (stack.Count > 0)
            {
                Cell temp = stack.Pop();
                // mark as visited to prevent infinite loops
                mazeArray[temp.RowIndex, temp.ColIndex].Visited = true;

                // Check every neighbor cell
                // If neighbour exists in the maze
                // and no walls between them
                // then set the neighbour cell previous pointer to the temp cell
                // and push neighbour cell into stack
                // else if no neighbour exists in the maze then complete

                // Left
                if (temp.ColIndex - 1 >= 0
                    && !mazeArray[temp.RowIndex, temp.ColIndex - 1].RightWall
                    && !mazeArray[temp.RowIndex, temp.ColIndex - 1].Visited)
                {
                    // fixed is used to show that the current memory location won't change
                    mazeArray[temp.RowIndex, temp.ColIndex - 1].Previous = mazeArray[temp.RowIndex, temp.ColIndex];

                    mazeArray[temp.RowIndex, temp.ColIndex - 1].Path = Cell.Paths.Left;
                    mazeArray[temp.RowIndex, temp.ColIndex - 1].IsSolution = true;
                    stack.Push(mazeArray[temp.RowIndex, temp.ColIndex - 1]);
                }

                // Right
                if (temp.ColIndex + 1 < maze.Width
                    && !mazeArray[temp.RowIndex, temp.ColIndex + 1].LeftWall
                    && !mazeArray[temp.RowIndex, temp.ColIndex + 1].Visited)
                {
                    mazeArray[temp.RowIndex, temp.ColIndex + 1].Previous = mazeArray[temp.RowIndex, temp.ColIndex];
                    mazeArray[temp.RowIndex, temp.ColIndex + 1].Path = Cell.Paths.Right;
                    mazeArray[temp.RowIndex, temp.ColIndex + 1].IsSolution = true;

                    stack.Push(mazeArray[temp.RowIndex, temp.ColIndex + 1]);
                }

                // Up
                if (temp.RowIndex - 1 >= 0
                    && !mazeArray[temp.RowIndex - 1, temp.ColIndex].DownWall
                    && !mazeArray[temp.RowIndex - 1, temp.ColIndex].Visited)
                {
                    mazeArray[temp.RowIndex - 1, temp.ColIndex].Previous = mazeArray[temp.RowIndex, temp.ColIndex];
                    mazeArray[temp.RowIndex - 1, temp.ColIndex].Path = Cell.Paths.Up;
                    mazeArray[temp.RowIndex - 1, temp.ColIndex].IsSolution = true;
                    stack.Push(mazeArray[temp.RowIndex - 1, temp.ColIndex]);
                }

                // Down
                if (temp.RowIndex + 1 < maze.Height
                    && !mazeArray[temp.RowIndex + 1, temp.ColIndex].UpWall
                    && !mazeArray[temp.RowIndex + 1, temp.ColIndex].Visited)
                {
                    mazeArray[temp.RowIndex + 1, temp.ColIndex].Previous = mazeArray[temp.RowIndex, temp.ColIndex];
                    mazeArray[temp.RowIndex + 1, temp.ColIndex].Path = Cell.Paths.Down;
                    mazeArray[temp.RowIndex + 1, temp.ColIndex].IsSolution = true;
                    stack.Push(mazeArray[temp.RowIndex + 1, temp.ColIndex]);
                }

                // Adding the end and start point as part of the solution
                if (temp.ColIndex == maze.EndPoint.ColIndex && temp.RowIndex == maze.EndPoint.RowIndex)
                {
                    // add end point to foundPath
                    temp.IsSolution = true;
                    pathSolution.Add(temp);
                    // check all until you reach start point
                    while (temp.Previous != null)
                    {
                        pathSolution.Add(temp);
                        temp = temp.Previous;
                    }
                    // add begin point to foundPath

                    temp.IsSolution = true;
                    pathSolution.Add(temp);

                    mazeArray[temp.RowIndex, temp.ColIndex].Visited = true;
                    mazeArray[temp.RowIndex, temp.ColIndex].IsSolution = true;
                    return new Result<List<Cell>>() { Data = pathSolution, IsSuccessfull = true };
                }

            }

            // no solution found
            return new Result<List<Cell>>() { ErrorMessage = "No solution was found" };
        }
    }
}
