function GetAllTeam(controlId) {
    $(controlId).empty();
    $(controlId).append("<option value='0'></option>");
    $.ajax({
        type: "GET",
        url: '/teams/simplelist',
        data: {},
        success: function (data) {
            if (data !== null) {
                if (data.data !== null && data.data.length > 0) {
                    $.each(data.data, function (index, item) {
                        $(controlId).append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                    });
                    if (data.data.length === 1) {
                        $(controlId).val(data.data[0].Id);
                    }
                }
            }
            $(controlId).chosen().trigger("chosen:updated");
            $(controlId).trigger("change");
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}