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
        // The maze solver is used to solve the maze
        private readonly IMazeSolver _mazeSolver;

        private readonly IMazeGenerator _mazeGenerator;

        // Properties 

        // The width specefied by the user
        private int _width;

        // The Height specifued by the user
        private int _height;

        // The two dimension array the represent the maze
        private Cell[,] _mazeArray;

        // The maze start point
        private Cell _startPoint;

        // The maze end point
        private Cell _endPoint;

        // Define if the process of generating is on or not
        public bool IsRunning;

        // solution path
        private List<Cell> _pathSolution;

        // Define if the process of solving the maze is on or not
        public bool IsSolving;

        /// Get maze width
        public int Width => _width;

        /// Get maze height
        public int Height => _height;

        /// Get start point of maze
        public Cell StartPoint => _startPoint;

        /// Get end point of maze
        public Cell EndPoint => _endPoint;

        // The maze cell array
        public Cell[,] MazeArray => _mazeArray;
        
        // Get maze solution path
        public List<Cell> PathSolution => _pathSolution;

        // Constructor 
        /// <summary>
        /// Initializes a maze with a maximum size
        /// </summary>
        public Maze(IMazeSolver mazeSolver, IMazeGenerator mazeGenerator)
        {
            _mazeSolver = mazeSolver;
            _mazeGenerator = mazeGenerator;
        }

        /// <summary>
        /// Generate the maze array
        /// </summary>
        /// <param name="totalWidth">The maximum width of the maze</param>
        /// <param name="totalHeight">The maximum height of the maze</param>
        public Result<Maze> Generate(int totalWidth, int totalHeight)
        {
            _mazeArray = new Cell[totalHeight, totalWidth];
            Initailze(_mazeArray, totalWidth, totalHeight);

            var result = _mazeGenerator.DepthFirstSearchMazeGeneration(this);
            this._mazeArray = result.Data;
            if (!result.IsSuccessfull)
            {
                return new Result<Maze>() { ErrorMessage = result.ErrorMessage };
            }

            this._startPoint = _mazeGenerator.GenerateMazeStartPoint(this);
            this._endPoint = _mazeGenerator.GenerateMazeEndPoint(this);

            return new Result<Maze>() { IsSuccessfull = true, Data = this };
        }
      
        /// <summary>
        /// Solve the maze
        /// </summary>
        public Result<Maze> Solve(Maze maze)
        {
            // initialize
            IsSolving = true;
            _pathSolution = new List<Cell>();
            UnVisitAllCells(maze.MazeArray);
            
            var result = _mazeSolver.SolveWithIterativeDepthFirst(maze);

            if (!result.IsSuccessfull)
            {
                return new Result<Maze>(){ErrorMessage = result.ErrorMessage};
            }

            maze._pathSolution = result.Data;
            IsSolving = false;

            return new Result<Maze>(){Data = maze ,IsSuccessfull = true};
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
        /// Set all passed array cells (Maze array)  to unvisited and clear the path,solution
        /// </summary>
        /// <param name="mazeArray">The maze array to reset elements</param>
        private void UnVisitAllCells(Cell[,] mazeArray)
        {
            for (int i = 0; i < mazeArray.GetLength(0); i++)
            {
                for (int j = 0; j < mazeArray.GetLength(1); j++)
                {
                    mazeArray[i, j].Visited = false;
                    mazeArray[i, j].Path = Cell.Paths.None;
                    mazeArray[i, j].IsSolution = false;
                }
            }
        }

    }
}
