<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>AddS AJAX Example</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>
    <h1>AJAX Request to AddS Method</h1>
    <label for="num1">Number 1:</label>
    <input type="number" id="num1">
    <label for="num2">Number 2:</label>
    <input type="number" id="num2">
    <button id="btnAdd">Add</button>
    <h2>Result: <span id="result"></span></h2>
    
    <script>
        $(document).ready(function () {
            $('#btnAdd').click(function () {
                var x = parseInt($('#num1').val());
                var y = parseInt($('#num2').val());
                
                $.ajax({
                    type: "POST",
                    url: "http://localhost:52187/Simplex.asmx/AddS",
                    data: JSON.stringify({ x: x, y: y }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#result').text(response.d);
                    },
                    error: function (xhr, status, error) {
                        alert("Error: " + error);
                    }
                });
            });
        });
    </script>
</body>
</html>
