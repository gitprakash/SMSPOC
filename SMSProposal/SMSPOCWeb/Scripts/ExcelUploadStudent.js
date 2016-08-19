$(window).resize(function () {
    adjustmodal();
});

function adjustmodal() {
    var altura = $(window).height() - 205; //value corresponding to the modal heading + footer
    $(".resize-scroll").css({ "height": altura, "overflow-y": "auto" });
}

$(document).ready(function () {

    $('#btn-reset').on('click', function (e) {
        var $el = $('#dataFile');
        $el.wrap('<form>').closest('form').get(0).reset();
        $el.unwrap();
    });
    
    $('#btnstudentconfirm').on('click', function (e) { 
        $("#bulkuploadstudentform").submit();
    });
    $('#btnupload').on('click', function (e) {
        $(".resize-scroll").removeAttr('style');
        $("#ErrorResultArea").empty();
        $("#SuccessResultArea").empty();
        $("#btnstudentconfirm").show(); 
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
            success: function (data, status) {
                $("#btnstudentconfirm").hide();
                $("#dataFile").val('');
                if (data.Status === false && data.ErrorResult)
                {
                    builderrortable(data.ErrorResult);
                }
                else if (data.Status === true && data.SuccessResult)
                {
                    buildsuccesstable(data.SuccessResult);
                }
            },
            error: function (e, details, xhr)
            {
                debugger;
            }
        });
    });
   
    var builderrortable = function (data) {
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
            $("<td class='text-sm'></td>").text(value.ErrorMessage).appendTo(row);
            $("<td class='text-sm'></td>").text(value.ErrorDescription).appendTo(row);
        });
        adjustmodal();
    };
    var buildsuccesstable = function (data) {
        //Crate table html tag
        $("<div class='label-success'>Successfully Added " + data.length + " Students</div>").appendTo("#SuccessResultArea");
        var table = $("<table id=successtable  class='table tabe-hour table-bordered table-responsive'></table>").appendTo("#SuccessResultArea");
        //Create table header row
        var rowHeader = $("<tr class='label-info'></tr>").appendTo(table);
        $("<th></th>").text("RollNo").appendTo(rowHeader);
        $("<th></th").text("Name").appendTo(rowHeader);
        $("<th></th").text("MobileNo").appendTo(rowHeader); 
        $.each(data, function (i, value) {
            //Create new row for each record
            var row = $("<tr class='label-success'></tr>").appendTo(table);
            $("<td></td>").text(value.RollNo).appendTo(row);
            $("<td></td>").text(value.Name).appendTo(row);
            $("<td></td>").text(value.Mobile).appendTo(row);
        });
        adjustmodal();
       

    };
});