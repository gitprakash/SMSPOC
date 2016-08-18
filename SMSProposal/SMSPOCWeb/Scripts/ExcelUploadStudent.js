$(function () {
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
                if (data.Status === 'error' && data.error)
                {
                    alert('exception occured' + data.error);
                }
            },
            error: function (e, details, xhr)
            {
                debugger;
            }
        });
    });
});