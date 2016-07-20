$(document).ready(function myfunction() {

    $('#list').jqGrid({
        caption: "Role Details",
        url: '/Subscriber/UserRoles/',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Role",
        colNames: ['User', 'Role '],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'User', index: 'User', width: 80, key: false, editable: true },
              { name: 'Role', index: 'Role', width: 80, key: false, editable: true }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        height: '100%',
    });

    jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true, refresh: true },
        {
            zIndex: 100,
            url: '/Role/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Role/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Role/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete Role... ? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});