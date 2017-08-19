using Microsoft.AspNetCore.Mvc;
using WebMaze.Models;
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
            var maze = new Maze(width, height);
            var result = maze.Generate();
            if (!result.IsSuccessfull)
            {
                return Json(new { Message = "Failed to generate the maze solution" });
            }
            var mazeArray = maze.MapMazeToArray();

            var solveResult = maze.Solve();
            if (!solveResult.IsSuccessfull)
            {
                return Json(new { Message = "Failed to generate the maze solution" });
            }

            var mazeArraySolve = maze.MapMazeToArray(true);
            return Json(new { Maze = mazeArray, Solution = mazeArraySolve });
        }



    }
}
