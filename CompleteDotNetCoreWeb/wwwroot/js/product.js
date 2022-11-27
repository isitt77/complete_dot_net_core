var dataTable;

$(document).ready(function () {
    loadDataTable();
});

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
                        console.log(data);
                        return `
                        <td>
                           <a class="btn btn-warning mx-2"
                               href="/Admin/Product/Upsert?id=${data}">
                           <i class="bi bi-pencil"></i> <span class="d-none
                                d-xl-inline">&nbsp; Edit</span></a>
                            <a class="btn btn-danger mx-2"
                                href="/Admin/Product/Delete?id=${data}">
                             <i class="bi bi-trash"></i> <span class="d-none
                                d-xl-inline">&nbsp; Delete</span></a>
                        </td>
                        `
                    }, "width": "24%"
                }
            ]
    });
}