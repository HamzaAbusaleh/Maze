using Microsoft.AspNetCore.Mvc;
using WebMaze.Models.Implementation;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
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

            var maze = new Maze(width, height, new MazeSolver());
            var result = maze.Generate();
            if (!result.IsSuccessfull)
            {
                return Json(new { ErrorMessage = "Failed to generate the maze" });
            }

            var mazeArray = maze.MapMazeToArray();

            var solveResult = maze.Solve();
            if (!solveResult.IsSuccessfull)
            {
                return Json(new { solveResult.ErrorMessage });
            }

            var mazeArraySolve = maze.MapMazeToArray(true);

            return Json(new
            {
                Maze = mazeArray,
                Solution = mazeArraySolve,
                startPoint = maze.GetStartPoint(),
                endPoint = maze.GetEndPoint()
            });
        }



    }
}
