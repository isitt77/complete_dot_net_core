var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#dataTable').DataTable({

        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category", "width": "15%" }
        ]
    });
}