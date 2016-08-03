$(document).ready(function myfunction() { 
    //loading template drodown  
    $('#list').jqGrid({
        caption: "Student Details",
        url: '/Notify/GetMessageHistory',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "SentDateTime",
        colNames: ['Id', 'RollNo', 'Name', 'Class', 'Section', 'Message', 'MobileNo', 'Status', 'SentDateTime'],
        colModel: [
               { name: 'Id', index: 'Id', key: true, hidden: true },
              {
                  name: 'RollNo', index: 'RollNo', width: 40, key: false, editable: true, align: 'center',
                  editrules: { required: true }
              },
              {
                  name: 'Name', index: 'Name', width: 40, key: false, editable: true, align: 'center' 
              },
              {
                  name: 'Class', index: 'Class', width: 10, key: false, editable: false, align: 'center'
              },
               {
                   name: 'Section', index: 'Section', width: 10, key: false, editable: false, align: 'center'
               },
               {
                   name: 'Message', index: 'Message', width: 80, key: false, editable: false, align: 'center'
               },
               {
                   name: 'MobileNo',
                   index: 'MobileNo',
                   width: 40,
                   key: false
               },
                { name: 'Status', index: 'Status', width: 30, key: false, align: 'center' }
                 ,
                    {
                        name: 'SentDateTime',
                        index: 'SentDateTime',
                        formatter: 'date',
                        width: 40,
                        key: false
                    }

       
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        jsonReader: { id: "0" },
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        shrinkToFit: true
    });
});