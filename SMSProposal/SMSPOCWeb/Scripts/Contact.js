function mobilenovalidation(value, colname) {
    return /^[789]\d{9}$/.test(value) ?
        [true] :
        [false, "Please enter 10 digit mobile number"];
}
var clsarray = {};
$(document).ready(function myfunction() {

    $('#list').jqGrid({
        caption: "Student Details",
        url: '/Contact/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'RollNo', 'Name', 'Class','Class List', 'Section', 'Mobile', 'BloodGroup'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'RollNo', index: 'RollNo', width: 40, key: false, editable: true, align: 'center', editrules: { required: true } },
              { name: 'Name', index: 'Name', width: 70, key: false, editable: true, align: 'center', editrules: { required: true }, editoptions: { size: 100, minlength: 2 } },
              {
                  name: 'Class', index: 'Class', width: 30, key: false, editable: false, align: 'center'
              },
              {
                  name: 'SubscriberStandardId', index: 'SubscriberStandardId', width: 30, key: false, hidden: true, editable: true, editrules: { edithidden: true }, hidedlg: true,
                  align: 'center', edittype: "select",
                  editoptions: {
                      value: eval('(' + standarList + ')'), dataInit: function (elem) {


                      }
                  }
              },
              { name: 'Section', index: 'Section', width: 30, key: false, editable: true, align: 'center', editrules: { required: true } },
              { name: 'Mobile', index: 'Mobile', width: 70, key: false, editable: true, align: 'center', editrules: { required: true, custom: true, custom_func: mobilenovalidation } },
              { name: 'BloodGroup', index: 'BloodGroup', width: 30, key: false, editable: true, align: 'center' },
               ,

        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        viewrecords: true,
        pager: jQuery("#pager"),
        height: '100%',
        autowidth: true
        
    });

    jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, edittitle: 'Edit Student', add: true, addtitle: 'Add Student', del: true, deltitle: 'Delete Student', refresh: false },
        {
            width: 400,
            zIndex: 100,
            url: '/Contact/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterSubmit: function (response) {
                var result = jQuery.parseJSON(response.responseText);
                if (result.Status === "success") {
                    alert('Successfully Update Student details');
                    $(this).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                    return [true, ''];
                }
                else {
                    return [false, result.error];
                }
            }
        },
       {
           width: 400,
           zIndex: 100,
           url: "/Contact/Add",
           closeOnEscape: true,
           closeAfterAdd: true,
           drag: true,
           editData: {
               Id: 0
           },
           afterSubmit: function (response) {

               var result = jQuery.parseJSON(response.responseText);
               if (result.Status === "success") {
                   alert('Successfully added Student details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, ''];
               }
               else {
                   return [false, result.error];
               }
           }

       },
       {
           zIndex: 100,
           url: "/Contact/Delete",
           closeOnEscape: true,
           closeAfterDelete: true,
           drag: true,
           afterSubmit: function (response) {
               var result = jQuery.parseJSON(response.responseText);
               if (result.Status == "success") {
                   alert('Successfully Deleted Student details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, ''];
               }
               else {
                   return [false, result.error];
               }
           }

       }

        );
});

var data = $.ajax({
    url: '/Contact/GetStandards',
    dataType: 'json', async: false,
    success: function (data, result) {
        if (!result) alert('Failure to retrieve the ContactList related lookup data.');
    }
}).responseText;
var Standards = eval('(' + data + ')');

 

var standarList = '{';
$(Standards).each(function () {
    standarList += this.SubscriberStandardId + ':"' + this.StandardName + '",';
});
standarList += '}';