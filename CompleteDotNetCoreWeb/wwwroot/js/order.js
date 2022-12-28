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
            "url": "/Admin/Order/GetAll"
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
                                    Details
                           </a>
                        </td>
                        `
                    }, "width": "24%"
                }
            ]
    });
}

