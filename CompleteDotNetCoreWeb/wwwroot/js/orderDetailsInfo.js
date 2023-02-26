document.getElementById("ValidateShippingInfo")
    .addEventListener("click", ValidateShippingInfo);

function ValidateShippingInfo(e) {

    //e.preventDefault();

    var form = document.getElementById("orderDetailsForm");

    const swalShippingAlert = Swal.mixin({
        customClass: {
            popup: 'bg-body text-body',
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    if (document.getElementById("trackingNumber").value == "") {
        swalShippingAlert.fire({
            icon: 'error',
            title: 'Umm...',
            text: 'Please enter tracking number!'
        })
        //return false;
        e.preventDefault();
    }
    if (document.getElementById("carrier").value == "") {
        swalShippingAlert.fire({
            icon: 'error',
            title: 'Umm...',
            text: 'Please enter carrier!'
        })
        //return false;
        e.preventDefault();
    }
    //if (document.getElementById("trackingNumber").value != ""
    //    && document.getElementById("carrier").value != "") {
    //    form.submit();
    //}
    //else {
    //    form.submit();
    //}
    //return true;
    //form.submit();
}
