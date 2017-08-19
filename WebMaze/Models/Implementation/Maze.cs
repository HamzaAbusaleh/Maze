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
        private Cell[,] maze;

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
        /// <param name="random"></param>
        public Maze(int totalWidth, int totalHeight)
        {
            maze = new Cell[totalHeight, totalWidth];
            Random = new Random();
            Initailze(maze, totalWidth, totalHeight);
        }



        /// <summary>
        /// Generate the maze array
        /// </summary>
        public Result Generate()
        {
            IsRunning = true;

            var result = DepthFirstSearchMazeGeneration(maze);

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
                    mazeArray[i, j] = new Cell(j, i);
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

            Cell location = mazeArray[Random.Next(_width), Random.Next(_height)];
            stack.Push(location);

            while (stack.Count > 0)
            {
                List<Tuple<int, int>> neighbours = FetchNeighborCells(mazeArray, location);
                if (neighbours.Count > 0)
                {
                    var neighbourIndex = neighbourRandom.Next(neighbours.Count);
                    int tempRow = neighbours[neighbourIndex].Item1;
                    int tempCol = neighbours[neighbourIndex].Item2;

                    RemoveWall(mazeArray, ref location, ref mazeArray[tempCol, tempRow]);

                    stack.Push(location);
                    location = mazeArray[tempCol, tempRow];
                }
                else
                {
                    location = stack.Pop();
                }

            }

            MakeMazeBeginEnd(maze);

            if (this._startPoint.LeftWall || this._endPoint.RightWall)
            {
                return new Result() {ErrorMessage = "Error occured : maze start or end point has not been created"};

            }

            return new Result(){IsSuccessfull = true};

        }

        public string[,] MapMazeToArray(bool withSolution = false)
        {
            var mazeArray = new string[_width * 2 + 1, _height * 2 + 1];

            int row = 0;
            int col = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int b = 0; b < _height; b++)
                {
                    var x = maze[i, b].DisplayCell();

                    for (int r = 0; r < 3; r++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            mazeArray[row + r, col + k] = x[r, k];
                        }
                    }

                    if (withSolution)
                    {
                        var solutionPathResult =
                            _pathSolution.FirstOrDefault(currentMaze => currentMaze.ColIndex == maze[i, b].ColIndex &&
                                                                             currentMaze.RowIndex == maze[i, b].RowIndex);
                        if (solutionPathResult.IsSolution)
                        {
                            mazeArray[row + 1, col + 1] = solutionPathResult.IsSolution ? "X" : "";
                        }
                    }


                    col = col + 2;
                }

                row = row + 2;
                col = 0;
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
                if (!mazeArray[cell.ColIndex, cell.RowIndex][i])
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
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
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
            UnVisitAllCells(maze);

            var result = SolveWithIterativeDepthFirst(_startPoint, _endPoint);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            IsSolving = false;

            return result;

        }

        /// <summary>
        /// Solving the maze using the Iterative depth first
        /// </summary>
        /// <param name="start">Maze start cell</param>
        /// <param name="end">Maze end cell</param>
        /// <returns>return true when the solution is found</returns>
        /// unsafe shows that the current method is using pointers
        public virtual unsafe Result SolveWithIterativeDepthFirst(Cell start, Cell end)
        {
            Stack<Cell> stack = new Stack<Cell>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                Cell temp = stack.Pop();

                // mark as visited to prevent infinite loops
                maze[temp.ColIndex, temp.RowIndex].Visited = true;

                // Check every neighbor cell
                // If neighbour exists in the maze
                // and no walls between them
                // then set the neighbour cell previous pointer to the temp cell
                // and push neighbour cell into stack
                // else if no neighbour exists in the maze then complete
                
                // Left
                if (temp.RowIndex - 1 >= 0
                    && !maze[temp.ColIndex, temp.RowIndex - 1].RightWall
                    && !maze[temp.ColIndex, temp.RowIndex - 1].Visited)
                {
                    // fixed is used to show that the current memory location won't change
                    fixed (Cell* cell = &maze[temp.ColIndex, temp.RowIndex])
                        maze[temp.ColIndex, temp.RowIndex - 1].Previous = cell;
                    maze[temp.ColIndex, temp.RowIndex - 1].Path = Cell.Paths.Left;
                    maze[temp.ColIndex, temp.RowIndex - 1].IsSolution = true;
                    stack.Push(maze[temp.ColIndex, temp.RowIndex - 1]);
                }

                // Right
                if (temp.RowIndex + 1 < _width
                    && !maze[temp.ColIndex, temp.RowIndex + 1].LeftWall
                    && !maze[temp.ColIndex, temp.RowIndex + 1].Visited)
                {
                    fixed (Cell* cell = &maze[temp.ColIndex, temp.RowIndex])
                        maze[temp.ColIndex, temp.RowIndex + 1].Previous = cell;
                    maze[temp.ColIndex, temp.RowIndex + 1].Path = Cell.Paths.Right;
                    maze[temp.ColIndex, temp.RowIndex + 1].IsSolution = true;

                    stack.Push(maze[temp.ColIndex, temp.RowIndex + 1]);
                }

                // Up
                if (temp.ColIndex - 1 >= 0
                    && !maze[temp.ColIndex - 1, temp.RowIndex].DownWall
                    && !maze[temp.ColIndex - 1, temp.RowIndex].Visited)
                {
                    fixed (Cell* cell = &maze[temp.ColIndex, temp.RowIndex])
                        maze[temp.ColIndex - 1, temp.RowIndex].Previous = cell;
                    maze[temp.ColIndex - 1, temp.RowIndex].Path = Cell.Paths.Up;
                    maze[temp.ColIndex - 1, temp.RowIndex].IsSolution = true;
                    stack.Push(maze[temp.ColIndex - 1, temp.RowIndex]);
                }

                // Down
                if (temp.ColIndex + 1 < _height
                    && !maze[temp.ColIndex + 1, temp.RowIndex].UpWall
                    && !maze[temp.ColIndex + 1, temp.RowIndex].Visited)
                {
                    fixed (Cell* cell = &maze[temp.ColIndex, temp.RowIndex])
                        maze[temp.ColIndex + 1, temp.RowIndex].Previous = cell;
                    maze[temp.ColIndex + 1, temp.RowIndex].Path = Cell.Paths.Down;
                    maze[temp.ColIndex + 1, temp.RowIndex].IsSolution = true;
                    stack.Push(maze[temp.ColIndex + 1, temp.RowIndex]);
                }

                // Adding the end and start point as part of the solution
                if (temp.ColIndex == end.ColIndex && temp.RowIndex == end.RowIndex)
                {
                    // add end point to foundPath
                    temp.IsSolution = true;
                    _pathSolution.Add(temp);
                    // check all until you reach start point
                    while (temp.Previous != null)
                    {
                        _pathSolution.Add(temp);
                        temp = *temp.Previous;
                    }
                    // add begin point to foundPath
                    temp.IsSolution = true;
                    _pathSolution.Add(temp);

                    maze[temp.ColIndex, temp.RowIndex].Visited = true;
                    maze[temp.ColIndex, temp.RowIndex].IsSolution = true;
                    return new Result(){IsSuccessfull = true};
                }

            }
            // no solution found
            return new Result(){ErrorMessage = "Error occured : No solution found"};
        }




    }
}
