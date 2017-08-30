using Microsoft.AspNetCore.Mvc;
using WebMaze.Models.Implementation;
using WebMaze.Models.Interface;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMaze _maze;
        private readonly IMazeActions _actions;

        public HomeController(IMaze maze,IMazeSolver mazeSolver,IMazeActions actions)
        {
            _maze = maze;
            _actions = actions;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateMaze(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return Json(new { ErrorMessage = "Failed to generate the maze" });
            }

            var result = _maze.Generate(width, height);
            if (!result.IsSuccessfull)
            {
                return Json(new { ErrorMessage = "Failed to generate the maze" });
            }

            var maze = result.Data;
            var mazeArray = _actions.MapMazeToArray(maze);

            var solveResult = _maze.Solve(maze);
            if (!solveResult.IsSuccessfull)
            {
                return Json(new { solveResult.ErrorMessage });
            }

            var mazeArraySolve = _actions.MapMazeToArray(maze,true);

            return Json(new
            {
                Maze = mazeArray,
                Solution = mazeArraySolve,
                startPoint = maze.StartPoint,
                endPoint = maze.EndPoint
            });
        }



    }
}
