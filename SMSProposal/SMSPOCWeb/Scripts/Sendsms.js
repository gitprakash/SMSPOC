﻿
var selectedcontactarray = [];

function CreateLinks() {
    
    $.each(selectedcontactarray, function (i, contact) {
        $("<a >", { href: '#', text: contact.Rollno, id: contact.Id, title: contact.Name }).css('margin-left','10px').appendTo("#ContactList");
    });
}

$(function () {

    $('#LoadContact').click(function () {
        $("#dialog-div")
       .dialog({
           title:"Student details",
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
                           function(i, rowid) {
                               var name = $('#list').jqGrid('getCell', rowid, 'Name');
                               var rollno = $('#list').jqGrid('getCell', rowid, 'RollNo');
                               var standard = $('#list').jqGrid('getCell', rowid, 'Class');
                               var section = $('#list').jqGrid('getCell', rowid, 'Section');
                               var mobile = $('#list').jqGrid('getCell', rowid, 'Mobile');
                               var contactvm = {
                                   'Id': rowid,
                                   'Name': name,
                                   'Rollno': rollno,
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
        height: '100%',
        width: '600',
        multiselect: true,
        gridview:true
    });
    jQuery("#list").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: true });
    jQuery("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

}