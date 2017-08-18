$.ajax({
    type: 'GET',
    dataType: 'json',
    url: '/Home/GenerateMaze',
    data: { width: 20, height: 20 },
    success: function (response) {
        //alert('success');
        console.log(response);
        var maze = response["maze"];
        localStorage["mazesolution"] = JSON.stringify(response["solution"]);
        localStorage["maze"] = maze;

        printMaze(maze);


    },
    error: function (jqXHR, textStatus, errorThrown) {
        alert('Error - ' + errorThrown);
    }
});

// print maze solotion when Get Solution button is clicked
$("#btnSolution").click(function () {
    var mazeSolution = JSON.parse(localStorage["mazesolution"]);
    printMaze(mazeSolution);

});

// Print the maze that is passed
printMaze = function (maze) {
    /// <summary></summary>
    /// <param name="maze" type=""></param>
    var tbody = $('#maze tbody');
    tbody.empty();

    $.each(maze, function (i) {
        var tr = $('<tr>');
        for (var x = 0; x < maze[i].length; x++) {
            $('<td class="td-height-10">').html(maze[i][x]).appendTo(tr);

        }
        tbody.append(tr);

    });
}