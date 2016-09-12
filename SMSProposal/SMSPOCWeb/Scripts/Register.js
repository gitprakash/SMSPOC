jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

//get file size
function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        //for IE
        if ($.browser) {
            if ($.browser.msie) {
                //before making an object of ActiveXObject, 
                //please make sure ActiveX is enabled in your IE browser
                var objFSO = new ActiveXObject("Scripting.FileSystemObject");
                var filePath = $("#" + fileid)[0].value;
                var objFile = objFSO.getFile(filePath);
                var fileSize = objFile.size; //size in kb
                fileSize = fileSize / 1048576; //size in mb 
            }
        }
            //for FF, Safari, Opeara and Others
        else {
            fileSize = $("#" + fileid)[0].files[0].size //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }

        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
    }
}

//get file path from client system
function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);
    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}
function IsValidFile() {
    var file = getNameFromPath($("#ImageUpload").val());
    $("#spanfileupload").hide();
   // $("#submit").hide();
    var flag;
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension) {
            case 'pdf':
                flag = true;
                break;
            default:
                flag = false;
        }
    }
    if (flag == false) {
        $("#spanfileupload").show();
        $("#spanfileupload").html("You can upload only pdf extension file");
        return false;
    }
    else {
        var size = GetFileSize('ImageUpload');
        if (size > 5) {
            $("#spanfileupload").show();
            $("#spanfileupload").html("You can upload file up to 5 MB");
            return false;
        }
        else {
            $("#spanfileupload > span").html("");
            return true;
        }
    }
}
$(function () {
    $('#submit').click(function () {
        if ($("#ImageUpload").val().trim() !== "") {
            var isvalid = IsValidFile();
            if (isvalid)
                return true;
            else
                return false;
        }
    });

    $("#ImageUpload").change(function () { 
        return IsValidFile();
    });
    
});
