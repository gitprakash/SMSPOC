var helpers =
{
    buildDropdown: function (result, dropdown, emptyMessage) {
        // Remove current options
        dropdown.html('');
        // Add the empty option with the empty message
        dropdown.append('<option value="">' + emptyMessage + '</option>');
        // Check result isnt empty
        if (result !== '') {
            // Loop through each of the results and append the option to the dropdown
            $.each(result, function (k, v) {
                dropdown.append('<option value="' + v.Description + '">' + v.Name + '</option>');
            });
        }
    }
}
$(window).resize(function () {
    var outerwidth = $('#grid').width();
    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
});

function GetSMSMessageCount(value) {
    var chars = value.length,
            messages = Math.ceil(chars / 160),
    remaining = messages * 160 - (chars % (messages * 160) || messages * 160);
    return { 'messages': messages, 'remaining': remaining };
}

$(document).ready(function () {
    var $remaining = $('#remaining'),
    $messages = $remaining.next();
    $('#txtsms').keyup(function () {
        var messagesremaining = GetSMSMessageCount(this.value.trim()),
          remaining = messagesremaining.remaining;
        messages = messagesremaining.messages;
        $remaining.text(remaining + ' characters remaining');
        $messages.text(messages + ' message(s)');
    });
});

var selectedcontactarray = [];
$(function () {

    $('#dropdown').change(function () {
        $('#txtsms').val($(this).val());
        $('#txtsms').keyup();
    });
    $('#submitMyForm')
        .click(function () {
            $("#errormsg").html('');
            if (selectedcontactarray.length === 0) {

                showAlert("Please select a contact", "danger", 10000);

                return false;
            }
            else if ($('#txtsms').val().trim().length === 0) {
                showAlert("Please enter some message", "danger", 10000);
                return false;
            }


            else {
                var count = GetSMSMessageCount($('#txtsms').val().trim());
                showDisableLayer();
                //$('#submitMyForm').adda
                $.ajax({
                    type: 'Post',
                    url: '/Notify/SendMessage',
                    data: { messageViewModel: selectedcontactarray, Message: $('#txtsms').val().trim(), messagecount: count.messages },
                    success: function (data) {
                        hideDisableLayer();
                        if (data.Status === 'success' || data.Status === 'successwithnoinsertion') {
                            showAlert("Data Processed, please check Sent history for status", "success", 10000);
                        }
                        if (data.Status === 'error') {
                            showAlert("Error Occured "+data.error, "danger", 10000);
                        }
                    },
                    error: function (data, error) {
                        hideDisableLayer();
                        alert('problem in sending message template details' + data);
                    }
                });
            }
        });

    $.ajax({
        type: 'Get',
        url: '/Template/GetTemplates',
        success: function (data) {
            helpers.buildDropdown(
                    data,
                    $('#dropdown'),
                    'Select an option'
                );
        },
        error: function (data) {
            alert('problem in retrieving message template details');
        }
    });


    $.ajax({
        type: 'Get',
        async:false,
        url: '/Notify/GetSubscriberSMS',
        dataType: 'json',
        success: function (data,success) {
            //var result = JSON.parse(data);
            $("#opensmscnt").html(data.Openingsms);
            $("#smsbalcnt").html(data.balancesms);
        },
        error: function (data) {
            alert('problem in retrieving message balance details');
        }
    });


    $('#ContactList').on('click', '.close', function () {
        var id = $(this).attr('contactid');
        $('#' + id).remove();
        $(this).remove();
        $.each(selectedcontactarray, function (i, contact) {
            if (selectedcontactarray[i]) {
                if (selectedcontactarray[i].Id === id) // delete index
                {
                    selectedcontactarray.splice(i, 1);
                    return false;
                }
            }
        });
        return false;
    });
});

function CreateLinks() {
    $("#ContactList").empty();
    $.each(selectedcontactarray, function (i, contact) {
        var span = $("<span  contactid=" + contact.Id + " class='close'> x <span>").css({ 'padding': '2px 5px', 'background': '#ccc', 'color': 'red' });
        var a = $("<a href='javascript:void(0)'></a>")
            .css({ 'display': 'inline-block' })
            .append(span);
        $("<span id=" + contact.Id + " class='btn btn-info btn-sm' title= " + contact.Name + ":" + contact.Mobile + ">" + contact.RollNo + "</span>").css('margin-right', '10px').appendTo("#ContactList");
        $(a).appendTo('#ContactList');
    });
}

$(function () {

    $('#LoadContact').click(function () {
        $("#dialog-div")
       .dialog({
           title: "Student details",
           resizable: true,
           autoOpen: true,
           position: { my: "center top+15%", at: "center top+15%" },
           minWidth: 700,
           open: function (event, ui) {
               ConstructJqGrid();
           },
           buttons: {
               'Confirm': function () {
                   var ids = $("#list").jqGrid('getGridParam', 'selarrrow');
                   if (ids.length > 0) {

                       selectedcontactarray = [];
                       $.each(ids,
                           function (i, rowid) {
                               var name = $('#list').jqGrid('getCell', rowid, 'Name');
                               var rollno = $('#list').jqGrid('getCell', rowid, 'RollNo');
                               var standard = $('#list').jqGrid('getCell', rowid, 'Class');
                               var section = $('#list').jqGrid('getCell', rowid, 'Section');
                               var mobile = $('#list').jqGrid('getCell', rowid, 'Mobile');
                               var contactvm = {
                                   'Id': rowid,
                                   'Name': name,
                                   'RollNo': rollno,
                                   'Standard': standard,
                                   'Section': section,
                                   'Mobile': mobile
                               };
                               if (rowid) {
                                   selectedcontactarray.push(contactvm);
                               }
                           });
                       CreateLinks();
                       $(this).dialog('close');
                   } else {
                       alert('please select a contact');
                       return false;
                   }
               },
               'cancel': function () {
                   $(this).dialog('close');
               }
           }
       });
    });
});
function ConstructJqGrid() {
    $('#list').jqGrid({
        caption: "Student Details",
        url: '/Contact/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'RollNo', 'Name', 'Class', 'Section', 'Mobile'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'RollNo', index: 'RollNo', width: 40, key: false, align: 'center' },
              { name: 'Name', index: 'Name', width: 70, key: false, align: 'center' },
              { name: 'Class', index: 'Class', width: 30, key: false, align: 'center' },
              { name: 'Section', index: 'Section', width: 30, key: false, align: 'center' },
              { name: 'Mobile', index: 'Mobile', width: 70, key: false, align: 'center' }

        ],
        rowNum: 10,
        rowList: [10, 20, 50, 100],
        viewrecords: true,
        pager: jQuery("#pager"),
        //height: '100%',
        //width: '600',
        multiselect: true,
        gridview: true,
        shrinkToFit: true,
        autowidth: true
    });
    jQuery("#list").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: true });
    jQuery("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

}

function showAlert(message, type, closeDelay) {
    $("#alerts-container").empty();
    if ($("#alerts-container").length === 0) {
        // alerts-container does not exist, create it
        $("body")
            .append($('<div id="alerts-container" style="position: fixed;width: 50%; left: 25%; top: 10%;">'));
    }

    // default to alert-info; other options include success, warning, danger
    type = type || "info";

    // create the alert div
    var alert = $('<div class="alert alert-' + type + ' fade in">')
        .append(
            $('<button type="button" class="close" data-dismiss="alert">')
            .append("&times;")
        )
        .append(message);

    // add the alert div to top of alerts-container, use append() to add to bottom
    $("#alerts-container").prepend(alert);

    // if closeDelay was passed - set a timeout to close the alert
    if (closeDelay)
        window.setTimeout(function () { alert.alert("close") }, closeDelay);
}
var showDisableLayer = function () {
    $('<div id="loading" style="position:fixed; z-index: 2147483647; top:0; left:0; background-color: white; opacity:0.0;filter:alpha(opacity=0);"></div>').appendTo(document.body);
    $("#loading").height($(document).height());
    $("#loading").width($(document).width());
};

var hideDisableLayer = function () {
    $("#loading").remove();
};
