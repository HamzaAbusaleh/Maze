using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using WebMaze.Models;
using WebMaze.Models.Implementation;
using WebMaze.Models.Interface;

namespace WebMaze.Tests
{
    public class MazeTests
    {
        private readonly Mock<IMazeActions> _mazeActionsMock;
        private readonly Mock<IMazeGenerator> _mazeGeneratorMock;
        private readonly Mock<IMazeSolver> _mazeSolverMock;
        private readonly Mock<IMaze> _mazeMock;
        private readonly Maze SUT;
        public MazeTests()
        {
            _mazeActionsMock = new Mock<IMazeActions>();
            _mazeGeneratorMock = new Mock<IMazeGenerator>();
            _mazeSolverMock = new Mock<IMazeSolver>();
            _mazeMock = new Mock<IMaze>();
            SUT = new Maze(_mazeSolverMock.Object, _mazeGeneratorMock.Object);

        }

        [Fact]
        public void Generate_CheckMazeGeneratStartAndEndPoint()
        {
            // Arrange
            _mazeGeneratorMock.Setup(e => e.DepthFirstSearchMazeGeneration(SUT))
                .Returns(new Result<Cell[,]>() { IsSuccessfull = true });
            _mazeGeneratorMock.Setup(e => e.GenerateMazeStartPoint(SUT))
                           .Returns(new Cell(0,0));
            _mazeGeneratorMock.Setup(e => e.GenerateMazeEndPoint(SUT))
                           .Returns(new Cell(0,9));

            //Act
            var result = SUT.Generate(10, 10);

            // Assert
            Assert.True(result.IsSuccessfull);
            Assert.Null(result.ErrorMessage);
            _mazeGeneratorMock.Verify(e => e.GenerateMazeEndPoint(SUT),Times.Once);
            _mazeGeneratorMock.Verify(e => e.GenerateMazeEndPoint(SUT),Times.Once);
            Assert.Equal(result.Data.StartPoint.RowIndex, 0);
            Assert.Equal(result.Data.StartPoint.ColIndex, 0);
            Assert.Equal(result.Data.EndPoint.RowIndex, 0);
            Assert.Equal(result.Data.EndPoint.ColIndex, 9);
        }

        [Fact]
        public void Generate_ReturnErrorMessageWhenAlgorithimFails()
        {
            // Arrange
            _mazeGeneratorMock.Setup(e => e.DepthFirstSearchMazeGeneration(SUT))
                .Returns(new Result<Cell[,]>() { ErrorMessage = "Fail Test"});
          
            //Act
            var result = SUT.Generate(10, 10);

            // Assert
            Assert.False(result.IsSuccessfull);
            Assert.Equal(result.ErrorMessage, "Fail Test");
            _mazeGeneratorMock.Verify(e => e.GenerateMazeEndPoint(SUT), Times.Never);
            _mazeGeneratorMock.Verify(e => e.GenerateMazeEndPoint(SUT), Times.Never);


        }

        [Fact]
        public void Solve_CheckPathSolutionSuccess()
        {
            // Arrange
            var maze = new Maze(new MazeSolver(), new MazeGenerator(new MazeActions()));
            var generatedMazeResult = maze.Generate(10,10);
            _mazeSolverMock.Setup(e => e.SolveWithIterativeDepthFirst(generatedMazeResult.Data))
                .Returns(new Result<List<Cell>>() { IsSuccessfull = true, Data = new List<Cell>(){new Cell(5,5)}});

            //Act
            var result = SUT.Solve(generatedMazeResult.Data);

            // Assert
            Assert.True(result.IsSuccessfull);
            Assert.Null(result.ErrorMessage);
            _mazeSolverMock.Verify(e => e.SolveWithIterativeDepthFirst(generatedMazeResult.Data), Times.Once);
            Assert.Equal(result.Data.PathSolution.Count,1);
            Assert.Equal(result.Data.PathSolution.FirstOrDefault().ColIndex,5);
            Assert.Equal(result.Data.PathSolution.FirstOrDefault().RowIndex,5);



        }

        [Fact]
        public void Solve_ReturnErrorMessageWhenFail()
        {
            // Arrange
            var maze = new Maze(new MazeSolver(), new MazeGenerator(new MazeActions()));
            var generatedMazeResult = maze.Generate(10, 10);
            _mazeSolverMock.Setup(e => e.SolveWithIterativeDepthFirst(generatedMazeResult.Data))
                .Returns(new Result<List<Cell>>() { ErrorMessage = "Fail Test"});

            //Act
            var result = SUT.Solve(generatedMazeResult.Data);

            // Assert
            Assert.False(result.IsSuccessfull);
            Assert.Equal(result.ErrorMessage, "Fail Test");
            _mazeSolverMock.Verify(e => e.SolveWithIterativeDepthFirst(generatedMazeResult.Data), Times.Once);
            Assert.Null(result.Data);



        }

    }
}
