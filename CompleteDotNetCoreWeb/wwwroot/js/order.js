var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inProcess")) {
        loadDataTable("inProcess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }

});

function loadDataTable(status) {
    dataTable = $("#dataTable").DataTable({
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all"
        }],

        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns":
            [
                { "data": "id" },
                { "data": "name" },
                { "data": "phoneNumber" },
                { "data": "applicationUser.email" },
                { "data": "orderStatus" },
                { "data": "orderTotal" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        <td>
                           <a class="btn btn-info mx-2"
                               href="/Admin/Order/Details?orderId=${data}">
                                    <i class="bi bi-list-check"></i>
                               <span class="d-none
                                d-xl-inline">&nbsp; Details</span>
                           </a>
                            <a onClick=Delete('/Admin/Order/Delete/${data}')
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
    const swalDeleteOrder = Swal.mixin({
        customClass: {
            popup: 'bg-body text-body',
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalDeleteOrder.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        iconColor: '#f0ad4e',
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
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalDeleteOrder.fire(
                'Cancelled',
                'Your file was not deleted.',
                'info'
            )
        }
    })
}