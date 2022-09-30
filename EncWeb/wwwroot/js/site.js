function FileTableSelectedItems_Changed() {
    $('#SelectedSequencesClient').val('');
    var s = '';
    $('.FileTableItem').each(function () {
        s += (this.checked) ? '1,' : '0,';
    });
    $('#SelectedSequencesClient').val(s);
    return true;
}
function SelectAllToggle(obj) {
    if (obj.checked) {
        $('.FileTableItem').each(function () {
            $(this).prop("checked", true);
        });
    }
    else {
        $('.FileTableItem').each(function () {
            $(this).prop("checked", false);
        });
    }
}

$(function () {
    $('#startDate').datepicker();
    $('#endDate').datepicker();
});
