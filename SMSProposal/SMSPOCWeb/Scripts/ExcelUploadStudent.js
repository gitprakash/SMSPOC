﻿$(function () {
    $('#btnstudentconfirm').on('click', function (e) {
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
                debugger;
            },
            error: function (e, details, xhr)
            {
                debugger;
            }
        });
    });
});