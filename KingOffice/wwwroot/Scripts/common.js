﻿function isNotValidFileSize(file, imageMaxSize = 2) {
    //in MB
    if (file.type) {
        let size = file.size, maxSize = imageMaxSize * 1024 * 1024;
        if (size > maxSize)
            return true;
    }
    return false;
}
function isNullOrNoItem(arr) {
    if (arr === null || arr === undefined || arr.length === 0)
        return true;
    return false;
}
function isNullOrUndefined(value) {
    if (value === null || value === undefined)
        return true;
    return false;
}
function isNullOrWhiteSpace(text) {
    if (text === null || text === undefined || text === '' || text.toString().trim() === '')
        return true;
    return false;
}
function renderGhichu(noidung) {
    if (isNullOrWhiteSpace(noidung))
        return
    $('#dsGhichu').prepend(
        '<div class="timeline-event-content  active">' +
        '<div class="timeline-item">' +
        '<div class="timeline-body">' +
        '<div class="timeline__message-container">'
        + noidung +
        '</div></div></div></div>'

    )
}
function LayDanhsachGhichu(hosoId) {
    //$('#ddlTrangThai').empty();
    $.ajax({
        type: "GET",
        url: '/hoso/comments/' + hosoId,
        success: function (data) {
            if (data.data !== null) {
                $.each(data.data, function (index, item) {
                    $('#dsGhichu').append(
                        '<div class="timeline-event-content  active">' +
                        '<div class="timeline-item">' +
                        '<div class="timeline-body">' +
                        '<div class="timeline__message-container">' +
                        '<strong>' + item.Commentator + ' (' + FormatDateTimeDMY(item.CommentTime) + '): </strong><span>' + item.Noidung + '</span>' +
                        '</div></div></div></div>'
                    );
                });
            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function LayTrangThai(value, control) {
    $(control).empty();
    $.ajax({
        type: "GET",
        url: '/hoso/statuses',
        success: function (data) {
            $(control).append("<option value='0'></option>");
            if (data.data !== null) {
                $.each(data.data, function (index, item) {
                    $(control).append(renderOptionItem(item.Id, item.Name));
                });
                $(control).val(value);
                $(control).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function getSalesCode(controlId, defaultValue = 0) {
    
    $.ajax({
        type: "GET",
        url: '/employee/sale',
        success: function (data) {
            $(controlId).empty();
            $(controlId).append("<option value='0'></option>");
            var ten = '';
            if (data !== null) {
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option  fullname='" + item.Name + "' value='" + item.Id + "'>" + item.Code + "</option>");
                });
            }
            if (defaultValue > 0) {
                $(controlId).val(defaultValue);
                $(controlId).trigger("change");
            $(controlId).chosen().trigger("chosen:updated");
            }
            

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function getPartners(controlId, value = null, productId = null, callbackControl = null) {
    $.ajax({
        type: "GET",
        url: '/partners',
        success: function (data) {
            $(controlId).empty();
            $(controlId).append("<option value='0'></option>");
            if (data !== null) {
                $.each(data.data, function (index, item) {
                    $(controlId).append(renderOptionItem(item.Id, item.Name));
                });
            }
            if (!isNullOrUndefined(value)) {
                $(controlId).val(value);
            }
            $(controlId).chosen().trigger("chosen:updated");
        },
        complete: function () {
            setTimeout(function () {
                getProductByPartnetId(value, callbackControl, productId);
            }, 1000);
            
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}


function getProductByPartnetId(madoitac, controlId, value = null) {
    if (isNullOrUndefined(madoitac) || isNullOrUndefined(controlId))
        return;
    let partnerId = parseInt(madoitac);
    $.ajax({
        type: "GET",
        url: `/products/${partnerId}`,
        success: function (data) {
            $(controlId).empty();
            $(controlId).append("<option value='0'></option>");
            if (data !== null) {
                $.each(data.data, function (index, item) {
                    $(controlId).append(renderOptionItem(item.Id, item.Name));
                });
            }
            if (!isNullOrUndefined(value)) {
                $(controlId).val(value);
            }
            $(controlId).chosen().trigger("chosen:updated");

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function LayDsKetQua(value,controlId) {
    $(controlId).empty();
    $.ajax({
        type: "GET",
        url: '/hoso/results',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='1'></option>");
            if (data.data !== null) {
                $.each(data.data, function (index, item) {
                    $(controlId).append(renderOptionItem(item.Id,item.Name));
                });
                if (!isNullOrUndefined(value)) {
                    $(controlId).val(value);
                }
                $(controlId).chosen().trigger("chosen:updated");
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function renderOptionItem(id, name) {
    if (isNullOrUndefined(id) || isNullOrUndefined(name))
        return;
    return "<option value='" + id + "'>" + name + "</option>"
}
function renderOneItemFile(key, fileId, titleName, isRequire = false, className = '', generateInput = false,
    _initialPreview = [],
    _initialPreviewConfig = [],
    allowUpload = false,
    isFileExist = false,
    onUpload = null,
    onDelete = null,
    filesUploaded = []) {

    let content = "<div class='col-sm-3' style='width:20%'>";
    
    if (!isNullOrUndefined(titleName)) {
        if (isRequire) {
            content += '<h5  class="header green ' + className + '">' + titleName + '<span class="required">(*)</span></h5>';
        }
        else {
            content += '<h5  class="header green ' + className + '">' + titleName + '<span > </span></h5>';
        }
    }

    content += "<div class=\"file-loading\">";
    content += "<input class='attachFile' key=" + key + " id=\"attachFile-" + fileId + "\" type=\"file\">";
    content += "</div>";
    content += "</div>";
    $('#tailieu-' + key).append(content);
    if (generateInput === true) {
        let item = $("#attachFile-" + fileId);

        fileId = (isFileExist === true) ? fileId : getNewGuid();
        let uploadUrl = isFileExist === true ? '/media/uploadtemp/' + key + "/" + fileId : '/media/uploadtemp/' + key + "/0";
        $(item).fileinput({
            uploadUrl: allowUpload === true ? uploadUrl : null,
            validateInitialCount: true,
            maxFileSize: 25 * 1024,
            msgSizeTooLarge: 'File "{name}" (<b>{size} KB</b>)'
                + 'exceeds maximum allowed upload size of <b>{25} MB</b>. '
                + 'Please retry your upload!',
            allowedFileExtensions: ['png', 'jpg', 'pdf'],
            initialPreviewAsData: true, // identify if you are sending preview data only and not the raw markup
            initialPreviewFileType: 'image',
            overwriteInitial: false,
            showUploadedThumbs: false,
            uploadAsync: false,
            showClose: false,
            showCaption: false,
            showBrowse: false,
            showUpload: false, // hide upload button
            showRemove: false, // hide remove button
            browseOnZoneClick: true,
            removeLabel: '',
            fileId: fileId,
            btnDeleteId: 'btn-remove-file-' + fileId,
            dropZoneTitle: 'Kéo và thả tập tin vào đây',
            dropZoneClickTitle: '<br>(hoặc nhấp để chọn)',
            removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
            removeTitle: 'Cancel or reset changes',
            elErrorContainer: '#kv-avatar-errors-2',
            msgErrorClass: 'alert alert-block alert-danger',
            //layoutTemplates: { main2: '{preview} ' + btnCust + ' {remove} {browse}' },
            //layoutTemplates: { footer: '' },
            initialPreview: _initialPreview,
            initialPreviewDownloadUrl: _initialPreview,
            initialPreviewConfig: _initialPreviewConfig,
            fileActionSettings: {
                showDownload: true,
                showRemove: true,
                showUpload: true,
                showZoom: true,
                showDrag: false
            },
            append: true

        }).on("filebatchselected", function (event, files) {
            if (countFilesByKey(filesUploaded, parseInt(key)) >= 5)
                return;
            $(item).fileinput("upload");
        }).on("filebeforedelete", function (event, key2, fileId) {
            
            if (onDelete !== null) {
                onDelete(key, fileId, isFileExist);
            }
        }).on('filebatchuploadsuccess', function (event, data) {
            if (onUpload !== null) {
                onUpload(key, fileId, data.response, isFileExist);
            }
        });

    }

}
function isReach5Files(filesUpload,key) {
    if (isNullOrWhiteSpace(key))
        return true;
    if (isNullOrNoItem(filesUpload))
        return false;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return false;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    if (sameKeyFile.files.length === 5)
        return true;
    return false;
}
function countFilesByKey(filesUpload,key) {
    if (isNullOrWhiteSpace(key))
        return 0;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return 0;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    return sameKeyFile.files.length;
}
function getNewGuid() {
    const s4 = () => {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    };
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}
function showMessage(title, message, type = 'success', showConfirmButton = true, callback = null) {
    if (callback === null) {
        swal({
            title: title,
            text: message,
            type: !isNullOrWhiteSpace(type) ? type : "success",
            timer: 4000,
            showConfirmButton: showConfirmButton
        });
        return;
    }
    swal({
        title: title,
        text: message,
        type: !isNullOrWhiteSpace(type) ? type : "success",
        timer: 4000,
        showConfirmButton: showConfirmButton
    }, callback());
}
function setDateTimeInput(controlId, isSetDefaultDate = true, day = 0, format = 'dd/mm/yy') {

    $(controlId).datepicker({
        dateFormat: format//'mm-dd-yy'
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });
    if (isSetDefaultDate === true) {
        if (isNullOrUndefined(day))
            day = 0;
        if (day === 0) {
            $(controlId).datepicker({ dateFormat: format }).datepicker("setDate", new Date());
        }
        else {
            $(controlId).datepicker({ dateFormat: format }).datepicker("setDate", new Date().getDay + day);
        }
        
    }
}
function getTotalPage(totalRecord, limit = 10) {
    return totalRecord > limit ? Math.ceil(totalRecord / limit) : 1;
}
function renderGoPreviousPage(page) {
    let newCurrentPage = page;
    if (page > 1) {
        newCurrentPage = page - 1;
        return "<li class='paginate_button previous' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")'>"
            + "<a href='javascript:;'>Trước</a>"
            + "</li>";

    } else {
        return "";
    }
}
function renderGoNextPage(page) {
    if (page < totalPage) {
        newCurrentPage = page + 1;
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")' >"
            + "<a href='javascript:;'>Sau</a>"
            + "</li>";

    } else {
        return "";
    }
}
function renderGoLastPage(page) {

    if (totalPage > page) {
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + totalPage + ")' >"
            + "<a href='javascript:;'>Trang cuối</a>"
            + "</li>";
    } else {
        return "";
    }
}
function renderGoFirstPage(page) {
    if (totalPage > 1 && page > 1) {
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + 1 + ")' >"
            + "<a href='javascript:;'>Trang đầu</a>"
            + "</li>";
    } else {
        return "";
    }
}
function renderTotalPage(totalPage) {
    if (totalPage > 0)
        return "<label>Tổng: " + totalPage + "</label>";
    return "";
}
function renderPageList(page, limit, totalRc) {
    let pageMargin = 2;
    totalPage = getTotalPage(totalRc, limit);
    var startPage = page > pageMargin ? page - pageMargin : 1;
    var endPage = pageMargin + page > totalPage ? totalPage : pageMargin + page;
    var paging = $("#paging");
    paging.empty();
    var first = renderGoFirstPage(page);
    var next = renderGoNextPage(page);
    var prev = renderGoPreviousPage(page);
    var last = renderGoLastPage(page);
    paging.append(first);
    paging.append(prev);
    for (var i = startPage; i <= endPage; i++) {
        var active = page === i ? ' active' : '';
        var item = "<li class='paginate_button" + active + " aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + i + ")' >"
            + "<a href='javascript:;'>" + i + "</a>"
            + "</li>";
        paging.append(item);
    }
    paging.append(next);
    paging.append(last);
}
function getValueDisplay(value, type) {
    if (isNullOrWhiteSpace(type)) {
        if (isNullOrWhiteSpace(value))
            return "";
        return value;
    }

    var display = null;
    switch (type) {
        case 'datetime':
            display = FormatDateTimeDMY(value);
            break;
        default: break;
    }
    return display;
}
function renderTextLeft(value, type, className = '') {
    return "<td class='text-left " + className + "'>" + getValueDisplay(value, type) + "</td>";
}
function renderTextCenter(value, type) {
    return "<td class='text-center'>" + getValueDisplay(value, type) + "</td>";
}
function renderAction(id) {
    let thaoTac = "<div class='action-buttons'><a title='Chỉnh sửa' class='green' style='cursor: pointer'  onclick='onEdit(" + id + ")' >";
    thaoTac += "<i class=\"ace-icon fa fa-pencil bigger-130\">";
    thaoTac += "</i>";
    thaoTac += "</a>";
    thaoTac += "</a></div>";
    return "<td class='text-center'>" + thaoTac + "</td>";
}

function preventTxtSearchEnter(controlId = "txtFreeText", btnSearchId = "#btnSearch") {
    var input = document.getElementById(controlId);
    input.addEventListener("keydown", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();

        }
    });
    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            $(btnSearchId).click();
        }
    });
}
function setTableLimit(controlId = "#ddlLimit") {
    $(controlId).chosen({ width: '100%', allow_single_deselect: true });
}
jQuery.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key === 8 ||
                    key === 9 ||
                    key === 13 ||
                    key === 46 ||
                    key === 110 ||
                    key === 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };
function setCheckedValueOfRadioButtonGroup(name, vValue) {
    var radios = document.getElementsByName(name);
    for (var j = 0; j < radios.length; j++) {
        if (radios[j].value === vValue) {
            radios[j].checked = true;
            break;
        }
    }
}


function showBlock(div, text) {
    div.block({
        css: {
            border: 'none',
            padding: '15px',
            //backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            //color: '#fff'
        },
        message: '<img src="/images/busy.gif" />' + text
    });
}

function showBlockUI(text) {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            //backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            //color: '#fff'
        },
        message: '<img src="/images/busy.gif" />' + text
    });
}


function addDefaultOption(selectList, objectText) {
    selectList.empty();
    selectList
        .append($("<option></option>")
            .attr("value", "-1")
            .text("Chọn " + objectText));
}

function addSelectListWithDefaultValue(selectList, results, defaultValue) {
    //debugger;
    if (results.length === 0) {
        selectList.html('<option value="-1">Không có dữ liệu</option>');
        return;
    }
    $.each(results, function (i, value) {
        var transformedResult = value.Text;
        var option = $("<option></option>").attr("value", value.Value).text(transformedResult);
        if (defaultValue !== null && defaultValue === value.Value) {
            option.attr('selected', 'selected');
        }
        selectList.append(option);
    });
}

function addSelectListItems(selectList, results) {
    if (results.length === 0) {
        selectList.html('<option value="-1">Không có dữ liệu</option>');
        return;
    }
    if (results.length === 1) {
        selectList
            .append($("<option></option>")
                .attr("value", results[0].Value)
                .attr("selected", true)
                .text(results[0].Text));
        selectList.change();
        return;
    }
    $.each(results, function (i, value) {
        selectList
            .append($("<option></option>")
                .attr("value", value.Value)
                .text(value.Text));
    });
}

function shortDateFormat(intDate) {
    if (intDate !== null && intDate !== '')
        return kendo.toString(new Date(parseInt(intDate) + (new Date().getTimezoneOffset()) * 60 * 1000), "dd/MM/yyyy");
    return intDate;
}

function fullDateFormat(intDate) {
    if (intDate !== null && intDate !== '')
        return kendo.toString(new Date(parseInt(intDate) + (new Date().getTimezoneOffset()) * 60 * 1000), "dd/MM/yyyy HH:mm");
    return intDate;
}

function FormatString(alias) {
    var str = alias;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ắ|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
    /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, ""); //cắt bỏ ký tự - ở đầu và cuối chuỗi 
    str = str.replace(/[^a-zA-Z0-9\-]/g, '');//halm 11/10/2014: cắt bỏ ký tự đặc biệt
    return str;
}

function addAlert(elementId, message, success = false) {
    $('#' + elementId).empty();
    //var mess = messages.find(p => p.code === message)
    if (success === true) {
        $('#' + elementId).append(
            '<div class="alert alert-block alert-success"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button>'
            + '<p><strong><i class="ace-icon fa fa-check"></i></strong> ' + message + '</p>'
            + '</div>');
    } else {
        $('#' + elementId).append(
            '<div class="alert alert-danger"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button>'
            + '<p><strong><i class="ace-icon fa fa-times"></i></strong> ' + message + '</p>'
            + '</div>');
    }
}
function convertMDYToDMYFromStr(input) {

    var mydate = moment(input, 'MM/DD/YYYY');

    return moment(mydate).format("DD/MM/YYYY");
}
function convertDMYToMDYFromStr(input) {
    var mydate = moment(input, 'DD/MM/YYYY');
    return moment(mydate).format("MM/DD/YYYY");
}
function convertStringToDate(strDatetime, format = "MM-DD-YYYY") {
    var mydate = moment(strDatetime, 'DD/MM/YYYY');

    return moment(mydate).format("MM/DD/YYYY");
}
function FormatDateTimeDMYHM(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "/" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "/" + dateObj.getFullYear() + " " + ('00' + dateObj.getHours()).slice(-2) + ":" + ('00' + dateObj.getMinutes()).slice(-2);
            return dateStr;
        }
    } catch (e) {
        return "";
    }
}
function getDateShort(inputDate, defaultFormat = 'DD/MM/YYYY hh:mm A') {
    let date = new Date(inputDate);

    var utcDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds());

    return moment(utcDate).format(defaultFormat);
}
function convertStringtoShortDate(inputDate, format = "DD/MM/YYYY") {
    let s = inputDate.substring(0, 10)
    let d = moment(inputDate, format).toDate();
    return getDateShort(d, format).toString();
}
function formatDateFromString(input, format = "DD/MM/YYYY") {
    let date = convertStringtoShortDate(input);
    let d = moment(date, format).toDate();
    return getDateShortFromDate(d, false, format);
}
function getDateShortFromDate(inputDate, fromNow = false, defaultFormat = 'DD/MM/YYYY hh:mm:ss A') {
    return formatDate(inputDate, fromNow, defaultFormat)
}
function formatDate(inputDate, timeNow = false, defaultFormat = 'DD/MM/YYYY hh:mm A') {

    let date = new Date(inputDate);

    let today = new Date();
    today.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());

    let yesterday = new Date();
    yesterday.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
    yesterday.setDate(today.getDate() - 1);

    let tomorrow = new Date();
    tomorrow.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
    tomorrow.setDate(today.getDate() + 1);

    let lastWeek = new Date();
    lastWeek.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
    lastWeek.setDate(today.getDate() - 7);

    let lastMonth = new Date();
    lastMonth.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
    lastMonth.setDate(today.getMonth() - 1);

    let result = null;

    switch (date.getTime()) {

        case today.getTime():
            if (timeNow) {
                result = MomentLocale(today, 'vi').fromNow();
            } else {
                result = MomentLocale(today, 'vi').calendar();
            }
            break;
        case yesterday.getTime():
            result = MomentLocale(yesterday, 'vi').calendar();
            break;
        case tomorrow.getTime():
            result = MomentLocale(tomorrow, 'vi').calendar();
            break;
        case lastWeek.getTime():
            result = MomentLocale(lastWeek, 'vi').calendar();
            break;
        case lastMonth.getTime():
            result = MomentLocale(lastMonth, 'vi').calendar();
            break;
        default:
            result = MomentLocale(inputDate, 'vi').format(defaultFormat);
            break;
    }
    return result;
}
function formartDatetimeWithMomentJs(datetime, format) {
    var dateMomentObject = moment("11-29-2019", "dd/mm/yy"); // 1st argument - string, 2nd argument - format
    var dateObject = dateMomentObject.toDate();

    //return dateObject.getDate() + "-" + dateObject.getMonth() + "-" + dateObject.getFullYear();
    return dateObject;
}
function MomentLocale(date = new Date(), locale = 'vi') {
    let m = moment(date).locale(locale);
    m['_locale']._invalidDate = 'Ngày không hợp lệ'
    m['_locale']._meridiemParse = /SA|CH/i
    m['_locale']._longDateFormat.LT = 'hh:mm A'
    if (m['_locale']._abbr === 'vi') {

        m['_locale']._calendar.sameDay = '[Hôm nay lúc] LT'
        m['_locale']._calendar.nextDay = '[Ngày mai]'
        m['_locale']._calendar.nextWeek = '[Tuần tới]'
        m['_locale']._calendar.lastDay = '[Hôm qua lúc] LT'
        m['_locale']._calendar.lastWeek = '[Tuần rồi]'
    }
    return m
}
function FormatDateTimeDMY(datetime) {
    if (isNullOrUndefined(datetime))
        return "";
    try {

        var d = new Date(datetime);
        return d.toLocaleDateString();
        //var valueDate = parseInt(datetime.substr(6));
        //if (valueDate < 0)
        //    return "";
        //else {
        //    var dateObj = new Date(valueDate);
        //    var dateStr = ('00' + dateObj.getDate()).slice(-2) + "/" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "/" + dateObj.getFullYear();
        //    return dateStr;
        //}
        //return dateStr;
    } catch (e) {
        return "";
    }
}
function SetFormatDateTime(datetime) {
    try {
        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "-" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "-" + dateObj.getFullYear();
            return dateStr;
        }
    } catch (e) {
        return "";
    }
}
function SetFormatDateTimeDMY(datetime) {
    try {

        var valueDate = parseInt(datetime.substr(6));
        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "-" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "-" + dateObj.getFullYear();
            return dateStr;
        }
    } catch (e) {
        return "";
    }
}

$('.num').keyup(function () {
    this.value = this.value.replace(/[^0-9\.]/g, '');
});

function GetDiffDate(startDate, endDate) {
    var temp = endDate.setHours(0, 0, 0, 0) - startDate.setHours(0, 0, 0, 0);
    var diff_date = Math.round(temp / (24 * 60 * 60 * 1000));
    return diff_date;
}
function strip(html, numchar) {
    var tmp = document.createElement("DIV");
    tmp.innerHTML = html.replace(/<p[^>]*>/g, '').replace(/<\/p>/g, '<br/>');
    //var result = tmp.textContent || tmp.innerText || "";
    var result = tmp.textContent;
    if (result.length > numchar)
        return result.substring(0, numchar) + "...";
    return result
}
function formatCurrency(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + '';
}
function formatCurrencyVND(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + 'VND';
}
function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}
function isExtension(ext, extnArray) {
    var result = false;
    var i;
    if (ext) {
        ext = ext.toLowerCase();
        for (i = 0; i < extnArray.length; i++) {
            if (extnArray[i].toLowerCase() === ext) {
                result = true;
                break;
            }
        }
    }
    return result;
}

const messages = [
    { code: "invalid_username_or_pass", value_vi: "Tên đăng nhập hoặc mật khẫu không đúng" },
    { code: "", value_vi: "" },
]