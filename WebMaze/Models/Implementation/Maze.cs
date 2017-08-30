using System;
using System.Collections.Generic;
using System.Linq;
using WebMaze.Models.Interface;

namespace WebMaze.Models.Implementation
{
    /// <summary>
    /// The maze class
    /// Generate, Solve maze
    /// </summary>
    public class Maze : IMaze
    {
        private readonly MazeSolver _mazeSolver;

        // Properties 
        /// <summary>
        /// The width specefied by the user
        /// </summary>
        private int _width;

        /// <summary>
        /// The Height specifued by the user
        /// </summary>
        private int _height;

        /// <summary>
        /// The two dimension array the represent the maze
        /// </summary>
        private readonly Cell[,] _mazeArray;

        /// <summary>
        /// The maze start point
        /// </summary>
        private Cell _startPoint;

        /// <summary>
        /// The maze end point
        /// </summary>
        private Cell _endPoint;

        /// <summary>
        /// Define if the process of generating is on or not
        /// </summary>
        public bool IsRunning;

        /// <summary>
        /// solution path
        /// </summary>
        private List<Cell> _pathSolution = new List<Cell>();

        /// <summary>
        /// Define if the process of solving the maze is on or not
        /// </summary>
        public bool IsSolving;

        /// <summary>
        /// Used to generate maze
        /// </summary>
        public Random Random;

        /// <summary>
        /// Get maze width
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return _width;
        }

        /// <summary>
        /// Get maze height
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return _height;
        }

        /// <summary>
        /// Get start point of maze
        /// </summary>
        public Cell GetStartPoint()
        {
            return _startPoint;
        }
        /// <summary>
        /// Get end point of maze
        /// </summary>
        public Cell GetEndPoint()
        {
            return _endPoint;
        }

        public Cell[,] MazeArray => _mazeArray;

        /// <summary>
        /// Get maze solution path
        /// </summary>
        public List<Cell> GetMazeSolution()
        {
            return _pathSolution;
        }
        // Constructor 
        /// <summary>
        /// Initializes a maze with a maximum size
        /// </summary>
        /// <param name="totalWidth">The maximum width of the maze</param>
        /// <param name="totalHeight">The maximum height of the maze</param>
        /// <param name="mazeSolver">The maze solver is used to solve the maze</param>
        public Maze(int totalWidth, int totalHeight, MazeSolver mazeSolver)
        {
            _mazeSolver = mazeSolver;
            _mazeArray = new Cell[totalHeight, totalWidth];
            Random = new Random();
            Initailze(_mazeArray, totalWidth, totalHeight);
        }



        /// <summary>
        /// Generate the maze array
        /// </summary>
        public Result Generate()
        {
            IsRunning = true;

            var result = DepthFirstSearchMazeGeneration(_mazeArray);

            IsRunning = false;

            return result;
        }

        /// <summary>
        /// Initilize the maze array
        /// </summary>
        /// <param name="mazeArray">The maze array</param>
        /// <param name="width">Maze Width</param>
        /// <param name="height">Maze Height</param>
        private void Initailze(Cell[,] mazeArray, int width, int height)
        {
            _width = width;
            _height = height;

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    mazeArray[i, j] = new Cell(i, j);
                }
            }
        }

        /// <summary>
        /// Generate a maze with the Depth-First Search approach
        /// </summary>
        /// <param name="mazeArray">the array of cells</param>
        public Result DepthFirstSearchMazeGeneration(Cell[,] mazeArray)
        {
            Stack<Cell> stack = new Stack<Cell>();
            Random neighbourRandom = new Random();

            Cell location = mazeArray[Random.Next(_height), Random.Next(_width)];
            stack.Push(location);

            while (stack.Count > 0)
            {
                List<Tuple<int, int>> neighbours = FetchNeighborCells(mazeArray, location);
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

            MakeMazeBeginEnd(_mazeArray);

            if (this._startPoint.LeftWall || this._endPoint.RightWall)
            {
                return new Result() { ErrorMessage = "Error occured : maze start or end point has not been created" };

            }

            return new Result() { IsSuccessfull = true };

        }

        public string[,] MapMazeToArray(bool withSolution = false)
        {
            var mazeArray = new string[_height * 2 + 1, _width * 2 + 1];

            int row = 0;
            int col = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int b = 0; b < _height; b++)
                {
                    var x = _mazeArray[b, i].DisplayCell();

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
                            _pathSolution.FirstOrDefault(currentMaze => currentMaze.ColIndex == _mazeArray[b, i].ColIndex &&
                                                                             currentMaze.RowIndex == _mazeArray[b, i].RowIndex);
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

        /// <summary>
        /// Used to create a begin and end for a maze
        /// </summary>
        /// <param name="mazeArray">The array of the maze</param>
        private void MakeMazeBeginEnd(Cell[,] mazeArray)
        {
            int tempRow = Random.Next(_height);
            int tempCol = 0;
            mazeArray[tempRow, tempCol].LeftWall = false;
            _startPoint = mazeArray[tempRow, tempCol];

            tempRow = Random.Next(_height);
            tempCol = _width - 1;
            mazeArray[tempRow, tempCol].RightWall = false;
            _endPoint = mazeArray[tempRow, tempCol];
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
        /// Fetch all the neighbors who exists and have not been visited already for the Cell
        /// </summary>
        /// <param name="mazeArray">The maze array</param>
        /// <param name="cell">cell to get neighbors</param>
        /// <param name="width">The width of the maze</param>
        /// <param name="height">The height of the maze</param>
        /// <returns></returns>
        private List<Tuple<int, int>> FetchNeighborCells(Cell[,] mazeArray, Cell cell)
        {
            int tempRow = cell.RowIndex;
            int tempCol = cell.ColIndex;
            List<Tuple<int, int>> availablePlaces = new List<Tuple<int, int>>();

            // Left
            tempCol = cell.ColIndex - 1;
            if (tempCol >= 0 && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            // Right
            tempCol = cell.ColIndex + 1;
            if (tempCol < _width && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }

            // UpcolIndex;
            tempCol = cell.ColIndex;
            tempRow = cell.RowIndex - 1;
            if (tempRow >= 0 && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            // Down
            tempRow = cell.RowIndex + 1;
            if (tempRow < _height && CheckAllWallsIntact(mazeArray, mazeArray[tempRow, tempCol]))
            {
                availablePlaces.Add(new Tuple<int, int>(tempRow, tempCol));
            }
            return availablePlaces;
        }

        /// <summary>
        /// Set all passed array cells (Maze array)  to unvisited and clear the path,solution
        /// </summary>
        /// <param name="mazeArray">The maze array to reset elements</param>
        private void UnVisitAllCells(Cell[,] mazeArray)
        {
            for (int i = 0; i < _mazeArray.GetLength(0); i++)
            {
                for (int j = 0; j < _mazeArray.GetLength(1); j++)
                {
                    mazeArray[i, j].Visited = false;
                    mazeArray[i, j].Path = Cell.Paths.None;
                    mazeArray[i, j].IsSolution = false;
                }
            }
        }

        /// <summary>
        /// Solve the maze
        /// </summary>
        public Result Solve()
        {
            // initialize
            IsSolving = true;
            _pathSolution.Clear();
            UnVisitAllCells(_mazeArray);

            var result = _mazeSolver.SolveWithIterativeDepthFirst(this,_startPoint, _endPoint);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            _pathSolution = result.Data;
            IsSolving = false;

            return result;

        }
 
    }
}
