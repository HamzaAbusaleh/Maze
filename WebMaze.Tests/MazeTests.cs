using System;
using Moq;
using Xunit;
using WebMaze.Models;
using WebMaze.Models.Implementation;
using WebMaze.Models.Interface;

namespace WebMaze.Tests
{
    public class MazeTests
    {
        [Fact]
        public void Maze_CheckMazeInitilizedSuccessfully()
        {
            //Act
         /*   var maze = new Maze(10, 10, new MazeSolver(), new MazeActions());

            Assert.Equal(maze.Height, 10);
            Assert.Equal(maze.Width, 10);*/

        }

        [Fact]
        public void Generate_CheckIsRunningSetToFalseAftterGenerateFinish()
        {
            // Arrange
            var maze = new Maze(new MazeSolver(),new MazeActions(), new MazeGenerator(new MazeActions()));

            // Act
            maze.Generate(10,10);

            // Assert
            Assert.False(maze.IsRunning);
        }

        [Fact]
        public void Generate_CheckBeginPointExist()
        {
            // Arrange
            var maze = new Maze(new MazeSolver(), new MazeActions(), new MazeGenerator(new MazeActions()));

            // Act
            maze.Generate(10,10);

            // Assert
            Assert.Equal(maze.StartPoint.ColIndex, 0);
            Assert.InRange(maze.StartPoint.RowIndex, 0, 9);
        }

        [Fact]
        public void Generate_CheckEndPointExist()
        {
            // Arrange
            var maze = new Maze(new MazeSolver(), new MazeActions(), new MazeGenerator(new MazeActions()));

            // Act
            maze.Generate(10,10);

            // Assert
            Assert.Equal(maze.EndPoint.ColIndex, 9);
            Assert.InRange(maze.EndPoint.RowIndex, 0, 9);
        }
        
    }
}
