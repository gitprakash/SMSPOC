﻿
//get file size
function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        //for IE
        if ($.browser.msie) {
            //before making an object of ActiveXObject, 
            //please make sure ActiveX is enabled in your IE browser
            var objFSO = new ActiveXObject("Scripting.FileSystemObject");
            var filePath = $("#" + fileid)[0].value;
            var objFile = objFSO.getFile(filePath);
            var fileSize = objFile.size; //size in kb
            fileSize = fileSize / 1048576; //size in mb 
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

$(function () {
    $("#ImageUpload").change(function () {
        var file = getNameFromPath($(this).val());
        $("#spanfileupload").hide();

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
            if (size > 3) {
                $("#spanfileupload").show(); 
                $("#spanfileupload").html("You can upload file up to 3 MB");
            }
            else {
                $("#spanfileupload > span").html("");
            }
        }
    });
});
