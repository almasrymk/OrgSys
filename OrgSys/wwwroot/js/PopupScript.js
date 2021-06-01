$(function () {

    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
         
        $('#myModalContent').load(this.href, function () {
            $('.Pop').modal({
                keyboard: true
            }, 'show');
         
            bindForm(this);
        });
        return false;
    });

    //$('#logBtn').on("click", function (e) {

    //    $('#LogModalContent').load(this.href, function () {
    //        $('.modal').modal({
    //            keyboard: true
    //        }, 'show');

    //        bindForm(this);
    //    });
    //    return false;
    //});

  

});

function bindForm(dialog) {
   
    $('form', dialog).submit(function () {
        // $('#progress').show();
        toastr.options.positionClass = "toast-top-center";
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {

                    $('#myModalContent').modal('hide');
                   // $('#progress').hide();
                    location.reload();
                    //toastr.success('تم الحذف بنجاح');
                } else {
                    toastr.error(result.Error);
                    // $('#progress').hide();
                    //$('#myModalContent').html(result);
                    //bindForm();
                }
            }
        });
        return false;
    });
}
