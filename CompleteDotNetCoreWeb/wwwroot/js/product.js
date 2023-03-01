var dataTable;

$(document).ready(function () {
    loadDataTable();

    //console.log(dataTable);
    //dataTable.on("click", ".deleteBtn", function () {
    //    console.log("You clicked delete button");

    //    Delete();
})

//console.log(data)
//});

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all"
        }],

        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns":
            [
                { "data": "title" },
                { "data": "author" },
                { "data": "isbn" },
                { "data": "price" },
                { "data": "category.name" },
                { "data": "coverType.name" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        <td>
                           <a class="btn btn-warning mx-2"
                               href="/Admin/Product/Upsert?id=${data}">
                           <i class="bi bi-pencil"></i> <span class="d-none
                                d-xl-inline">&nbsp; Edit</span></a>
                            <a href="/Admin/Product/Delete/${data}"
                                class="btn btn-danger mx-2 deleteBtn">
                             <i class="bi bi-trash"></i> <span class="d-none
                                d-xl-inline">&nbsp; Delete</span></a>
                        </td>
                        `
                    }, "width": "24%"
                }
            ]
    });
}



//let table = document.getElementById("dataTable");

//let deleteButton = dataTable.getElementsByClassName("deleteBtn");

//console.log(deleteButton)

//console.log(table);

//document.getElementById("dataTable").addEventListener("click", Delete);

// Delete method

//Delete(url)

//function Delete(url) {
//    const swalDeleteWithBootstrapButtons = Swal.mixin({
//        customClass: {
//            popup: 'bg-body text-body',
//            confirmButton: 'btn btn-success',
//            cancelButton: 'btn btn-danger'
//        },
//        buttonsStyling: false
//    })

//    swalDeleteWithBootstrapButtons.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        iconColor: '#f0ad4e',
//        showCancelButton: true,
//        confirmButtonText: 'Yes, delete it!',
//        cancelButtonText: 'No, cancel!',
//        reverseButtons: true
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    if (data.success) {
//                        dataTable.ajax.reload()
//                        toastr.success(data.message)
//                    }
//                    else {
//                        toastr.error(data.message)
//                    }
//                }
//            })
//        } else if (
//            result.dismiss === Swal.DismissReason.cancel
//        ) {
//            swalDeleteWithBootstrapButtons.fire(
//                'Cancelled',
//                'Your file was not deleted.',
//                'info'
//            )
//        }
//    })
//}