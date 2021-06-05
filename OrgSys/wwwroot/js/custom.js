var ids = [];
var page = 1;
var url = '';
var area = '';
var pageTitle = '';
var Mode = "List";

LiskChk();
function LiskChk() {
    $(".chk").on("change", function (event) {
        event.preventDefault();
        ids = [];
        $(".chk:checked").each(function (index) {
            ids.push($(this).next().val());
        });
        if (ids != null && ids.length > 0) {
            $(".btn-add").hide();
            $(".group-add").show();
        }
        else {
            $(".group-add").hide();
            $(".btn-add").show();
        }
    });
}

function DefulatMode() {
    if (typeof Storage !== "undefined") {
        if (window.localStorage.getItem(pageTitle + '_Mode'))
            Mode = window.localStorage.getItem(pageTitle + '_Mode');
        if (Mode == "Table") {
            $("#tbList").show();
            $(".mode-table").addClass("active");
        }
        else if (Mode == "Object") {
            $("#obList").show();
            $(".mode-object").addClass("active");
        }
        else {
            $("#rowList").show();
            $(".mode-list").addClass("active");
        }
    }
}

$(".mode-table").on("click", function (event) {
    event.preventDefault();
    ChangeMode('Table');
    $("#tbList").show();
    $(".mode-table").addClass("active");
});

$(".mode-list").on("click", function (event) {
    event.preventDefault();
    ChangeMode('List');
    $("#rowList").show();
    $(".mode-list").addClass("active");
});

$(".mode-object").on("click", function (event) {
    event.preventDefault();
    ChangeMode('Object');
    $("#obList").show();
    $(".mode-object").addClass("active");
});

function showNotification(message, status) {    
    if (status == "error") {
        status = "danger";
    }
    $.notify(
        {
            title: status,
            message: message,
            target: "_blank"
        },
        {
            element: "body",
            position: null,
            type: status,
            allow_dismiss: true,
            newest_on_top: true,
            showProgressbar: true,
            placement: {
                from: "top",
                align: "center"
            },
            offset: 10,
            spacing: 10,
            z_index: 1031,
            delay: 2000,
            timer: 2000,
            url_target: "_blank",
            mouse_over: null,
            animate: {
                enter: "animated fadeInDown",
                exit: "animated fadeOutUp"
            },
            onShow: null,
            onShown: null,
            onClose: null,
            onClosed: null,
            icon_type: "class",
            template:
                //'<div data-notify="container" class="col-11 col-sm-4 alert  alert-{0} rounded" role="alert">' +
                //'<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                //'<span data-notify="icon"></span> ' +
                //'<span data-notify="title" class="mb-2">{1}</span> ' +
                //'<span data-notify="message" class="mb-2">{2}</span>' +
                //'<div class="progress" data-notify="progressbar">' +
                //'<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                //"</div>" +
                //'<a href="{3}" target="{4}" data-notify="url"></a>' +
                //"</div>"
                '<div class="card d-flex flex-row col-11 col-sm-3 alert alert-{0} p-0" style="font-size:xx-large">' +
                //'<a class="d-flex" href="#"><div class="rounded-circle m-3 align-self-center list-thumbnail-letters"></div></a>' +
                '<i class="align-self-center simple-icon-check m-3"></i>' +
                '<div class="d-flex flex-grow-1 min-width-zero">' +
                '<div class="card-body pl-0 align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero">' +
                '<div class="min-width-zero">' +
                '<a href="#"><p class="list-item-heading mb-1 truncate">{1}</p></a>' +
                '<p class="mb-1 text-muted text-small">{2}</p></div></div></div></div>'
        }
    );
    ChangeUrl(area, url);
}

function ChangeUrl() {
    if (typeof (history.pushState) != "undefined") {
        var obj = { area: area, Url: url + "?ParentId=" + $("#ParentId").val() + "&TypeId=" + $("#TypeId").val()};
        history.pushState(null, obj.area, obj.Url );
    } else {
        alert("Browser does not support HTML5.");
    }
}

function change(event, _page) {
    if ($('#txtSearch').val() == "" || event.keyCode == '13') {
        search(_page);
    }
}

function search(_page) {
    page = _page;
    var Search2 = "";
    //if ($("#HTypeId").length) {
    //    Search2 = "&TypeId=" + $("#HTypeId").val();
    //}
    //alert(Search2);
    setTimeout(() => {
        $.ajax({
            url: url + "?search=" + $('#txtSearch').val() + Search2 + "&page=" + page + "&ParentId=" + $("#ParentId").val() + "&TypeId=" + $("#TypeId").val(),
            success: function (result) {
                $('#List').empty();
                $('#List').html(result);   
                $(".group-add").hide();
                $(".btn-add").show();
                LiskChk();
                DefulatMode();               
            }
        });
    }, 300);
}

function ChangeMode(modeChange) {   
    window.localStorage.setItem(pageTitle + '_Mode', modeChange);   
    $(".mode-table").removeClass("active");
    $(".mode-list").removeClass("active");
    $(".mode-object").removeClass("active"); 
    $("#tbList").hide();
    $("#rowList").hide();
    $("#obList").hide();
}

function Delete(icon , title , Msg , Ok , Id) {
    ShowConfirmMsgById(icon, title, Msg, Ok, Id);
}

function DeleteById(id) {
    setTimeout(() => {
        $.ajax({
            type: 'GET',
            url: url + "/Delete?id=" + id,
            data: { ids: ids },
            success: function (result) {
                search(page);
            }
        });
    }, 300);
}

function DeleteList() {
    setTimeout(() => {
        $.ajax({
            type: 'POST',
            url: url + "/DeleteList",
            data: { ids: ids, TypeId : $("#TypeId").val()},
            success: function (result) {
                search(page);
                ids = [];              
            }
        });
    }, 300);
}

function ShowConfirmMsg(icon, title, massage, ok) {    
    var msg =
        '<div class="modal fade" id="MsgModel" tabindex="-1" role="dialog" aria-hidden="true">' +
        '<div class="modal-dialog" role = "document" >' +
        '<div class="modal-content modal-dialog card d-flex flex-row" role="document">' +
        '<div class="position-absolute card-top-buttons">' +
        '<button class="btn btn-header-light icon-button close" aria-label="Close" data-dismiss="modal">' +
        '<i class="iconsminds-close" style="font-size:x-large"></i></button></div><div class="d-flex">' +
        '<div class="rounded-circle m-4 align-self-center list-thumbnail-letters">' +
        '<i class="' + icon +'"></i></div></div>' +
        '<div class=" d-flex flex-grow-1 min-width-zero">' +
        '<div class="card-body pl-0 align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero">' +
        '<div class="min-width-zero"><a href="#">' +
        '<p class="list-item-heading mb-2 truncate">' + title +'</p></a>' +
        '<p class="mb-2 text-muted text-small mb-2">' + massage +'</p>' +
        '<button type="button" onclick="DeleteList()" class="btn btn-xs btn-outline-primary " aria-label="Close" data-dismiss="modal">' + ok + '</button>' +
        '</div></div></div></div></div></div>';

    $(".MsgBox").empty();
    $(".MsgBox").append(msg);
}

function ShowConfirmMsgById(icon, title, massage, ok, Id) {    
    var msg =
        '<div class="modal fade" id="MsgModelId" tabindex="-1" role="dialog" aria-hidden="true">' +
        '<div class="modal-dialog" role = "document" >' +
        '<div class="modal-content modal-dialog card d-flex flex-row" role="document">' +
        '<div class="position-absolute card-top-buttons">' +
        '<button class="btn btn-header-light icon-button close" aria-label="Close" data-dismiss="modal">' +
        '<i class="iconsminds-close" style="font-size:x-large"></i></button></div><div class="d-flex">' +
        '<div class="rounded-circle m-4 align-self-center list-thumbnail-letters">' +
        '<i class="' + icon + '"></i></div></div>' +
        '<div class=" d-flex flex-grow-1 min-width-zero">' +
        '<div class="card-body pl-0 align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero">' +
        '<div class="min-width-zero"><a href="#">' +
        '<p class="list-item-heading mb-2 truncate">' + title + '</p></a>' +
        '<p class="mb-2 text-muted text-small mb-2">' + massage + '</p>' +
        '<button type="button" onclick="DeleteById('+ Id +')" class="btn btn-xs btn-outline-primary " aria-label="Close" data-dismiss="modal">' + ok + '</button>' +
        '</div></div></div></div></div></div>';

    $(".MsgBox").empty();
    $(".MsgBox").append(msg);
}

function CheckRequired(control, dvivControl , message) {
    $("#error" + control).remove();
    if ($("#" + control).val() == "") {
        $("#" + dvivControl).after('<span id="error' + control + '" class="error" style="color:red">' + message + '</span>');
        $("#" + control).focus();
        $("#" + control).on("change", function () {
            $("#error" + control).remove();
        });
        return true;
    }
    return false;
}


function CheckRequiredNoMessage(control) {
    if ($("#" + control).val() == "") {
        $("#" + control).focus();      
        return true;
    }
    return false;
}


function CheckRage(control, dvivControl , min , max , message) {
    $("#error" + control).remove();
    if ("" + $("#" + control).val() != "") {
        if (($("#" + control).val().length < min && min != 0) || ($("#" + control).val().length > max && max != 0)) {
            $("#" + dvivControl).after('<span id="error' + control + '" class="error" style="color:red">' + message + '</span>');
            $("#" + control).focus();
            $("#" + control).on("change", function () {
                $("#error" + control).remove();
            });
            return true;
        }
    }
    return false;
}

function CheckRowCount(control, dvivControl, message) {
    $("#error" + control).remove();
    var count = 0;
    $('#' + control +' > tbody > tr').each(function () {
        var currentRow = $(this);        
        if (!currentRow.hasClass('trNew')) {
            count++;
        }
    });
    if (count == 0) {
        $("#" + dvivControl).after('<span id="error' + control + '" class="error" style="color:red">' + message + '</span>');
        $("#" + control).focus();
        return true;
    }
    return false;
}

function CheckRowCountWithVal(control , checkVal, dvivControl, message) {
    $("#error" + control).remove();
    var count = 0;
    $('#' + control + ' > tbody > tr').each(function () {
        var currentRow = $(this);
        if (!currentRow.hasClass('trNew') && currentRow.find("#" + checkVal).val() > 0 ) {
            count++;
        }
    });    
    if (count == 0) {
        $("#" + dvivControl).after('<span id="error' + control + '" class="error" style="color:red">' + message + '</span>');
        $("#" + control).focus();
        return true;
    }
    return false;
}

function CheckRowCountWithValNoMessage(control, checkVal) {    
    var count = 0;
    $('#' + control + ' > tbody > tr').each(function () {
        var currentRow = $(this);
        if (!currentRow.hasClass('trNew') && currentRow.find("#" + checkVal).val() > 0) {
            count++;
        }
    });
    if (count == 0) {      
        $("#" + control).focus();
        return true;
    }
    return false;
}