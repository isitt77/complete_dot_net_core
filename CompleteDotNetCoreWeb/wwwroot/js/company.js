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
                            <a href="/Admin/Company/Delete/${data}"
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
