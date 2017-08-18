using System;
using Xunit;
using WebMaze.Models;

namespace WebMaze.Tests
{
    public class MazeTests
    {
        [Fact]
        public void Maze_CheckMazeInitilizedSuccessfully()
        {
            //Act
            var maze = new Maze(10, 10);

            Assert.Equal(maze.GetHeight(), 10);
            Assert.Equal(maze.GetWidth(), 10);

        }

        [Fact]
        public void Generate_CheckIsRunningSetToFalseAftterGenerateFinish()
        {
            // Arrange
            var maze = new Maze(10, 10);

            // Act
            var result = maze.Generate();

            // Assert
            Assert.False(maze.IsRunning);
        }

        [Fact]
        public void Generate_CheckBeginPointExist()
        {
            // Arrange
            var maze = new Maze(10, 10);

            // Act
            var result = maze.Generate();

            // Assert
            Assert.Equal(maze.GetStartPoint().RowIndex,0);
            Assert.NotEqual(maze.GetStartPoint().ColIndex,0);
        }
    }
}
