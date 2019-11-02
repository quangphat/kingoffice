function getRoles(controlId, appendDefault = true, onSuccess = null) {
    $.ajax({
        type: "GET",
        url: '/roles',
        success: function (data) {
            if (onSuccess === null) {
                $(controlId).empty();
                if (appendDefault === true)
                    $(controlId).append('<option value="0">Tất cả</option>');
                if (data !== null) {
                    $.each(data.data, function (index, optionData) {
                        $(controlId).append("<option value='" + optionData.Id + "'>" + optionData.Name + "</option>");
                    });
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