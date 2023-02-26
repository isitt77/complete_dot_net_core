var shipOrderBtn = document.getElementById("ValidateShippingInfo")

if (shipOrderBtn != null) {
    shipOrderBtn.addEventListener("click", ValidateShippingInfo);
}

function ValidateShippingInfo(e) {

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
        e.preventDefault();
    }
    if (document.getElementById("carrier").value == "") {
        swalShippingAlert.fire({
            icon: 'error',
            title: 'Umm...',
            text: 'Please enter carrier!'
        })
        e.preventDefault();
    }
}
