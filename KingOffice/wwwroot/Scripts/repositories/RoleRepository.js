function getRoles(controlId, appendDefault = true,defaultText ="Tất cả", value = 0, onSuccess = null) {
    $.ajax({
        type: "GET",
        url: '/roles',
        success: function (data) {
            if (onSuccess === null) {
                $(controlId).empty();
                if (appendDefault === true)
                    $(controlId).append('<option value="0">' + defaultText +'</option > ');
                if (data !== null) {
                    $.each(data.data, function (index, optionData) {
                        $(controlId).append("<option value='" + optionData.Id + "'>" + optionData.Name + "</option>");
                    });
                }
                if (value > 0) {
                    $(controlId).val(value);
                }
                $(controlId).chosen().trigger("chosen:updated");
            }
            else {
                onSuccess();
            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}