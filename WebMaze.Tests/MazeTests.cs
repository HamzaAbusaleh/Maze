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
            maze.Generate();

            // Assert
            Assert.False(maze.IsRunning);
        }

        [Fact]
        public void Generate_CheckBeginPointExist()
        {
            // Arrange
            var maze = new Maze(10, 10);

            // Act
            maze.Generate();

            // Assert
            Assert.Equal(maze.GetStartPoint().ColIndex, 0);
            Assert.InRange(maze.GetStartPoint().RowIndex, 0, 9);
        }

        [Fact]
        public void Generate_CheckEndPointExist()
        {
            // Arrange
            var maze = new Maze(10, 10);

            // Act
            maze.Generate();

            // Assert
            Assert.Equal(maze.GetEndPoint().ColIndex, 9);
            Assert.InRange(maze.GetEndPoint().RowIndex, 0, 9);
        }
        
    }
}
