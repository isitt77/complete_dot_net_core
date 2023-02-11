﻿
document.getElementById("StripeRedirect")
    .addEventListener("click", function StripeRedirectAlert(event) {

        event.preventDefault();

        var form = document.getElementById("summaryForm");

        const swalCardNumAlert = Swal.mixin({
            customClass: {
                popup: 'bg-body text-body',
                confirmButton: 'btn btn-success'
            },
            buttonsStyling: false
        })

        swalCardNumAlert.fire({
            title: 'Redirecting to Stripe',
            icon: 'info',
            iconColor: '#0a94c2',
            html:
                'Use the following credit card number when prompted. </br>' +
                '<div class="input-group my-3">' +
                '<input id="swal-input1" class="form-control text-center" ' +
                ' value="4242424242424242" readonly"/> ' +
                ' <button id="copyCardNum" class="btn btn-light"> ' +
                ' <i id="clipboardIcon" class="bi bi-clipboard"></i></button>' +
                '</div>' +
                'Use any future date for expiration and any three digit number for CVC. </br>',
            showCloseButton: true,
            focusConfirm: false
        }).then((result) => {
            console.log(`Ok pressed: ${result.value}`);
            if (result.value == true) {
                form.submit();
            }
        });

        CopyCardNum();

    }, { once: true });



function CopyCardNum() {
    let handleCopyClick = document.getElementById("copyCardNum");

    Swal.disableButtons();

    handleCopyClick.addEventListener('click', () => {
        let input = document.getElementById("swal-input1");
        let cardNum = input.value;
        let clipboardIcon = document.getElementById("clipboardIcon");
        console.log(cardNum);

        navigator.clipboard.writeText(`${cardNum}`)
            .then(() => {
                clipboardIcon.className = "bi bi-clipboard-check";
                clipboardIcon.style.color = "#20c997";
                Swal.enableButtons();
            })
            .catch(() => {
                alert("Something went wrong. Try copying number maunally.");
                Swal.enableButtons();
            });
    }, { once: true });
}