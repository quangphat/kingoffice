function getProvinces(controlId, value = null, districtValue = 0) {
    $.ajax({
        type: "GET",
        url: '/locations/provinces',
        success: function (data) {
            $(controlId).empty();
            $(controlId).append("<option value='0'></option>");
            if (data !== null) {
                $.each(data.data, function (index, optionData) {
                    $(controlId).append("<option value='" + optionData.ID + "'>" + optionData.Ten + "</option>");
                });
            }
            if (value !== null)
                $(controlId).val(value);
            $(controlId).chosen().trigger("chosen:updated");

        },
        complete: function () {
            setTimeout(function () {
                getDistricts(value, "#ddlDistrict", districtValue);
            }, 1000);
            
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function getDistricts(provinceId, controlId, value = null) {
    $.ajax({
        type: "GET",
        url: '/locations/districts/' + provinceId,
        success: function (data) {
            $(controlId).empty();
            $(controlId).append("<option value='0'></option>");
            if (data !== null) {
                $.each(data.data, function (index, optionData) {
                    $(controlId).append("<option value='" + optionData.ID + "'>" + optionData.Ten + "</option>");
                });
            }
            if (value !== null)
                $(controlId).val(value);
            $(controlId).chosen().trigger("chosen:updated");
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}