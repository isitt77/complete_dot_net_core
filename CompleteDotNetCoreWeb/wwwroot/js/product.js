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
                { "data": "title", "width": "15%" },
                { "data": "author", "width": "15%" },
                { "data": "isbn", "width": "15%" },
                { "data": "price", "width": "15%" },
                { "data": "category.name", "width": "15%" },
                { "data": "coverType.name", "width": "15%" },
                {
                    "data": "id",
                    "render": function (data) {
                        console.log(data);
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="/Admin/Product/Upsert?id=${data}"
                                    class="btn btn-warning mx-2"> <i class="bi bi-pencil"></i> Edit</a>
                                 <a class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        </div>`
                        //   return `<td class="btn-group w-50%">
                        //   <a class="btn btn-warning"
                        //       href="/Admin/Product/Upsert?id=${data}">
                        //   <i class="bi bi-pencil"></i> &nbsp; Edit</a>
                        //</td>
                        //<td class="btn-group w-50%" role="button">
                        //    <a class="btn btn-danger"
                        //        href="/Admin/Product/Delete?id=${data}">
                        //     <i class="bi bi-trash"></i> &nbsp; Delete</a>
                        //</td>
                        //`
                    },
                    "width": "15%"
                }
            ]
    });
}