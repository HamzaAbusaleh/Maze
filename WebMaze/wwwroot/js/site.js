

// print maze solotion when Get Solution button is clicked
$("#btnSolution").click(function () {
    var mazeSolution = JSON.parse(localStorage["mazesolution"]);
    printMaze(mazeSolution);

});

$("#btnGenerate").click(function () {

    $("#textResult").text(""); // clear message result if there is any

    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: '/Home/GenerateMaze',
        data: { width: $("#width").val(), height: $("#height").val() },
        success: function (response) {

            if (response["errorMessage"]) {
                $("#textResult").text(response["errorMessage"]);
                return;
            }

            var maze = response["maze"];
            localStorage["mazesolution"] = JSON.stringify(response["solution"]);
            localStorage["maze"] = maze;

            printMaze(maze);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#textResult").text('Error - ' + errorThrown);
        }
    });

});


/// <summary>Print the maze that is passed</summary>
/// <param name="maze" type=""></param>
printMaze = function (maze) {

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