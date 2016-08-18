$(window).resize(function () {
    ajustamodal(); 
});

function ajustamodal() {
    var altura = $(window).height() - 205; //value corresponding to the modal heading + footer
    $(".ativa-scroll").css({ "height": altura, "overflow-y": "auto" });
}

$(function () {
    $('#btnstudentconfirm').on('click', function (e) {
        $("#ErrorResultArea").empty();
        $("#bulkuploadstudentform").submit();
    });
    $('#bulkuploadstudentform').submit(function (e) {
        e.preventDefault(); 
        var data = new FormData(this); // <-- 'this' is your form element 
        $.ajax({
            url: '/Contact/Upload',
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data,status) {
                if (data.Status === 'error' && data.error)
                {
                    buiderrorable(data.error);
                }
            },
            error: function (e, details, xhr)
            {
                debugger;
            }
        });
    });
   
    var buiderrorable = function (data) {
        //Crate table html tag
        var table = $("<table id=DynamicTable  class='table table-bordered table-responsive'></table>").appendTo("#ErrorResultArea");
        //Create table header row
        var rowHeader = $("<tr class='info'></tr>").appendTo(table);
        $("<th></th>").text("Error Message").appendTo(rowHeader);
        $("<th></th").text("Error Description ").appendTo(rowHeader);
        $.each(data, function (i, value) { 
            //Create new row for each record
            var row = $("<tr class='danger'></tr>").appendTo(table);
            $("<td></td>").text(value.ErrorMessage).appendTo(row);
            $("<td></td>").text(value.ErrorDescription).appendTo(row);
        });
    };
});