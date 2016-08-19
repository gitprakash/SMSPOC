$(window).resize(function () {
    adjustmodal();
});

function adjustmodal() {
    var altura = $(window).height() - 205; //value corresponding to the modal heading + footer
    $(".resize-scroll").css({ "height": altura, "overflow-y": "auto" });
}

$(document).ready(function() {
    
    $('#btnstudentconfirm').on('click', function (e) { 
        $("#bulkuploadstudentform").submit();
    });
    $('#btnupload').on('click', function (e) {
        $(".resize-scroll").removeAttr('style');
        $("#ErrorResultArea").empty(); 
    });

    $('#bulkuploadstudentform').submit(function (e) {
        e.preventDefault();
        var data = new FormData(this); // <-- 'this' is your form element 
        $.ajax({
            url: '/Contact/SaveExcelFile',
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data,status) {
                if (data.Status === false && data.ErrorResult)
                {
                    buiderrortable(data.ErrorResult);
                }
            },
            error: function (e, details, xhr)
            {
                debugger;
            }
        });
    });
   
    var buiderrortable = function (data) {
        //Crate table html tag
        $("<div class='alert-danger'>Error occured </div>").appendTo("#ErrorResultArea");
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
        adjustmodal();
    };
});