$(document).ready(function myfunction() {

    var allusers = $.ajax({
        url: '/Account/GetAllUsers', async: false,
        success: function (data, result) {
            if (!result)
                alert('Failure to User list');
        }
    }).responseText;
    var allroles = $.ajax({
        url: '/Role/GetAllRoles', async: false,
        success: function (data, result) {
            if (!result)
                alert('Failure to User list');
        }
    }).responseText;

    $('#list').jqGrid({
        caption: "Users Role Details",
        url: '/ManageUserRoles/UserRoles/',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Role",
        colNames: ['Id', 'User', 'Role'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'User', index: 'User', width: 80, key: false, editable: true, edittype: "select", editrules: { required: true }},
              { name: 'Role', index: 'Role', width: 80, key: false, editable: true, edittype: "select", editrules: { required: true } }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        viewrecords: true,
        pager: jQuery("#pager"),
        width: '600%',
        height: '100%',
        loadComplete: function () {
            $('#list').setColProp('User', { editoptions: { value: JSON.parse(allusers) } });
            $('#list').setColProp('Role', { editoptions: { value: JSON.parse(allroles) } });
        }
    });

    jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true, refresh: false },
        {
            zIndex: 100,
            url: '/Role/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterSubmit: function (response) {
              
                var result = jQuery.parseJSON(response.responseText);
                if (result.Status == "success") {
                    alert('Successfully added user roles details');
                    $(this).jqGrid('setGridParam',
                      { datatype: 'json' }).trigger('reloadGrid');
                    return [true, '']
                }
                else {
                    //error
                    return [false, result.error]
                }
            }
        },
       {
           zIndex: 100,
           url: "/ManageUserRoles/Add",
           closeOnEscape: true,
           closeAfterAdd: true,
           drag: true,
           editData: {
               User: function () {
                   var id = $("#User").val();
                   return $("#User option[value='" + id + "']").text();
               },
               Role: function () {
                   var id = $("#Role").val();
                   return $("#Role option[value='" + id + "']").text();
               }
           },

           afterSubmit: function (response) {

               var result = jQuery.parseJSON(response.responseText);
               if (result.Status == "success") {
                   alert('Successfully added user roles details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, '']
               }
               else {
                   //error
                   debugger;
                   return [false, result.error]
               }
           }

       }

        );
});