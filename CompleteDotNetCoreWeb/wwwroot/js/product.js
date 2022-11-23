﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({

        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "coverType.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return
                    `<td class="btn-group w-50%" role="button">
                        <a class="btn btn-warning"
                            href="/Admin/Product/Upsert?id=${data}">
                        <i class="bi bi-pencil"></i> &nbsp; Edit</a>
                     </td>
                     <td class="btn-group w-50%" role="button">
                         <a class="btn btn-danger"
                             href="/Admin/Product/Delete?id=${data}">
                          <i class="bi bi-trash"></i> &nbsp; Delete</a>
                     </td>
                     `
                },
                "width": "15%"
            }
        ]
    });
}