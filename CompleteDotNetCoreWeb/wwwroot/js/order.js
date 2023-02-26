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
                           <a class="btn btn-info mx-2 d-flex justify-content-center"
                               href="/Admin/Order/Details?orderId=${data}">
                                    <i class="bi bi-list-check"></i>
                               <span class="d-none
                                d-xl-inline">&nbsp; Details</span>
                           </a>
                        </td>
                        `
                    }, "width": "13%"
                }
            ]
    });
}

