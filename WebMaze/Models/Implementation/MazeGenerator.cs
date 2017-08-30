using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.Interface;

namespace WebMaze.Models.Implementation
{
    public class MazeGenerator : IMazeGenerator
    {
        private readonly IMazeActions _mazeActions;
        private readonly Random _cellRandom;

        public MazeGenerator(IMazeActions mazeActions)
        {
            _mazeActions = mazeActions;
            _cellRandom = new Random();
        }

        /// <summary>
        /// Generate a maze with the Depth-First Search approach
        /// </summary>
        /// <param name="maze">The maze</param>
        public Result<Cell[,]> DepthFirstSearchMazeGeneration(Maze maze)
        {
            Stack<Cell> stack = new Stack<Cell>();
            Random neighbourRandom = new Random();
            var mazeArray = maze.MazeArray;

            try
            {
                Cell location = mazeArray[_cellRandom.Next(maze.Height), _cellRandom.Next(maze.Width)];
                stack.Push(location);

                while (stack.Count > 0)
                {
                    List<Tuple<int, int>> neighbours = _mazeActions.FetchNeighborCells(maze, location);
                    if (neighbours.Count > 0)
                    {
                        var neighbourIndex = neighbourRandom.Next(neighbours.Count);
                        int tempRow = neighbours[neighbourIndex].Item1;
                        int tempCol = neighbours[neighbourIndex].Item2;

                        RemoveWall(mazeArray, ref location, ref mazeArray[tempRow, tempCol]);

                        stack.Push(location);
                        location = mazeArray[tempRow, tempCol];
                    }
                    else
                    {
                        location = stack.Pop();
                    }

                }
            }
            catch (Exception ex)
            {
                return new Result<Cell[,]>() { ErrorMessage =  "Error : Failed to Generate Maze" };

            }

            return new Result<Cell[,]>(){IsSuccessfull = true, Data = mazeArray};

        }

        /// <summary>
        /// Used to create a begin and end for a maze
        /// </summary>
        /// <param name="maze">The maze</param>
        public Cell GenerateMazeStartPoint(Maze maze)
        {
            int tempRow = _cellRandom.Next(maze.Height);
            int tempCol = 0;
            maze.MazeArray[tempRow, tempCol].LeftWall = false;
            return maze.MazeArray[tempRow, tempCol];

        }

        public Cell GenerateMazeEndPoint(Maze maze)
        {
            int tempRow = _cellRandom.Next(maze.Height);
            int tempCol = maze.Width - 1;
            maze.MazeArray[tempRow, tempCol].RightWall = false;
            return maze.MazeArray[tempRow, tempCol];
        }
        /// <summary>
        /// Remove the wall between two neighbor cells
        /// </summary>
        /// <param name="mazeArray">The maze array</param>
        /// <param name="current">the current cell</param>
        /// <param name="next">the next neighbor cell</param>
        private void RemoveWall(Cell[,] mazeArray, ref Cell current, ref Cell next)
        {
            // The next is down
            if (current.ColIndex == next.ColIndex && current.RowIndex > next.RowIndex)
            {
                mazeArray[current.RowIndex, current.ColIndex].UpWall = false;
                mazeArray[next.RowIndex, next.ColIndex].DownWall = false;
            }
            // the next is up
            else if (current.ColIndex == next.ColIndex)
            {
                mazeArray[current.RowIndex, current.ColIndex].DownWall = false;
                mazeArray[next.RowIndex, next.ColIndex].UpWall = false;
            }
            // the next is right
            else if (current.ColIndex > next.ColIndex)
            {
                mazeArray[current.RowIndex, current.ColIndex].LeftWall = false;
                mazeArray[next.RowIndex, next.ColIndex].RightWall = false;
            }
            // the next is left
            else
            {
                mazeArray[current.RowIndex, current.ColIndex].RightWall = false;
                mazeArray[next.RowIndex, next.ColIndex].LeftWall = false;
            }
        }
    }
}
