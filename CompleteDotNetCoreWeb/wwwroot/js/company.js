var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#dataTable2").DataTable({
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all"
        }],

        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns":
            [
                { "data": "name" },
                { "data": "address" },
                { "data": "city" },
                { "data": "state" },
                { "data": "zipCode" },
                { "data": "phoneNumber" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        <td>
                           <a class="btn btn-warning mx-2"
                               href="/Admin/Company/Upsert?id=${data}">
                           <i class="bi bi-pencil"></i> <span class="d-none
                                d-xl-inline">&nbsp; Edit</span></a>
                            <a onClick=Delete('/Admin/Company/Delete/${data}')
                                class="btn btn-danger mx-2">
                             <i class="bi bi-trash"></i> <span class="d-none
                                d-xl-inline">&nbsp; Delete</span></a>
                        </td>
                        `
                    }, "width": "24%"
                }
            ]
    });
}

function Delete(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload()
                        toastr.success(data.message)
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your file was not deleted. :)',
                'error'
            )
        }
    })
}