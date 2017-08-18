using Microsoft.AspNetCore.Mvc;
using WebMaze.Models;

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
            var maze = new Maze(width, height);
            var result = maze.Generate();
            if (!result)
            {

                return Json(new { Message = "Failed to generate the maze" });
            }
            var mazeArray = maze.MapMazeToArray();
            var solveResult = maze.Solve();
            if (!solveResult)
            {
                return Json(new { Message = "Failed to generate the maze solution" });
            }

            var mazeArraySolve = maze.MapMazeToArray(true);
            return Json(new { Maze = mazeArray, Solution = mazeArraySolve });
        }



    }
}
