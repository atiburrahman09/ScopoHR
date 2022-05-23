$(document).ready(function () {

    $('#purchaseOrderCreateDialogCall, .purchaseOrderEditDialogCall, .purchaseOrderCopyDialogCall').on('click', function (e) {
        DialogShow($(this), e);
    });

    $('#fundingCreateDialogCall, .fundingEditDialogCall').on('click', function (e) {
        DialogShow($(this), e);
    });

    $('#productionStatusCreateDialogCall, .productionStatusEditDialogCall').on('click', function (e) {
        DialogShow($(this), e);
    });

    $('#itemCategoryCreateDialogCall, .itemCategoryEditDialogCall').on('click', function (e) {
        DialogShow($(this), e);
    });

    $('.default-date-picker').datepicker({
        format: 'mm/dd/yyyy'
    })
    $(document).on('click', '.default-date-picker', function () {
        $(this).datepicker('show');
    });

    $('.combo-box').select2();
    $('.combo-box').css('width', '100%');

    $.ui.dialog.prototype._allowInteraction = function (e) {
        return !!$(e.target).closest('.ui-dialog, .ui-datepicker, .select2-drop').length;
    };
});

/***************************************** Dialog Show **************************************/
function DialogShow($this, $e) {
    $e.preventDefault();
    toastr.info('Please Wait...');
    $('<div></div>').load($this.attr('href'), function () {
        $(this)
        .addClass('dialog')
            .attr("id", $this.attr("data-dialog-id"))
                    .appendTo("body")
                    .dialog({
                        show: {
                            effect: 'fade',
                            duration: 500,
                        },
                        open: function (event, ui) {
                            if ($('#ProductionDailyReportID').val()) {
                                $('#PurchaseOrderID').trigger('change');
                            }
                        },
                        hide: {
                            effect: 'fade',
                            duration: 500
                        },
                        width: '80%',
                        close: function () { $(this).remove() },
                        modal: true,
                        draggable: true,
                        position: ['top', 100],
                        resizable: false,
                    })
                    .dialog("widget")
                        .find(".ui-dialog-titlebar, .ui-dialog-title, .ui-dialog-titlebar-close").css({
                            display: "none"
                        });
        $('.combo-box').select2();
        $('.combo-box').css('width', '100%');
    });
    toastr.clear();
};

$('.btn-cancel').on('click', function (e) {
    e.preventDefault();
    $('.dialog').dialog("close");
});

var onBeginRequest = function () {
    toastr.info('Please Wait');
};

var onSuccess = function (data, title) {
    if (data === true) {
        toastr.clear();
        toastr.success(title + ' is saved successfully');
        $('.dialog').dialog("close");

        var grid = $('.t-grid').data('tGrid');
        grid.ajaxRequest();
    } else {

    }
};