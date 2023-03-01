
tinymce.init({
    selector: 'textarea',
    //plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect',
    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
    tinycomments_mode: 'embedded',
    tinycomments_author: 'Author name',
    mergetags_list: [
        { value: 'First.Name', title: 'First Name' },
        { value: 'Email', title: 'Email' },
    ]
});


var productUppsertButton = document.getElementById("productUpsert");

if (productUppsertButton != null) {

    productUppsertButton.addEventListener("click", function ValidateInput(e) {

        if ($('#productUpsertForm').valid() === false) {
            return;
        }

        var form = document.getElementById("productUpsertForm");

        if (document.getElementById("uploadBox").value == "") {

            e.preventDefault();

            const swalWithBootstrapTheme = Swal.mixin({
                customClass: {
                    popup: 'bg-body text-body',
                    confirmButton: 'btn btn-success',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })

            swalWithBootstrapTheme.fire({
                iconColor: '#df4759',
                iconHtml: '<i class="bi bi-exclamation-lg"></i>',
                title: 'Umm...',
                text: 'Please upload an image!'
            })
        }
    });

}