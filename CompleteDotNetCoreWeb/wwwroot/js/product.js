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
                        <div class="row justify-content-around">
                        <td>
                           <a class="btn btn-warning col-md-3 col-lg-4 col-xl-5"
                               href="/Admin/Product/Upsert?id=${data}">
                           <i class="bi bi-pencil"></i> <span class="d-none
                                d-xl-inline">&nbsp; Edit</span></a>
                        </td>
                         <td>
                            <a class="btn btn-danger col-md-3 col-lg-4 col-xl-5"
                                href="/Admin/Product/Delete?id=${data}">
                             <i class="bi bi-trash"></i> <span class="d-none
                                d-xl-inline">&nbsp; Delete</span></a>
                        </td>
                         </div>
                        `
                    }
                }
            ]
    });
}