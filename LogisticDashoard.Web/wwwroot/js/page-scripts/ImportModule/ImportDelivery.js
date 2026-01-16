import apiCall from "./../../helpers/APICall.js";

let importDeliveryModal = "";

export async function ImportDelivery() {
    let data = await fetch(`${API_BASE_URL}/api/ImportDeliveryDashboards/`);
    let response = await data.json();
    let filteredData = [];

    //destroy table
    $('#import-delivery-table').DataTable().destroy();
    const import_delivery_table = $('#import-delivery-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        importDeliveryModal = "create";

                        $("#importDelivery-bilNo").val('');
                        $("#importDelivery-shipper").val('');
                        $("#importDelivery-originalETA").val('');
                        $("#importDelivery-revisedETA").val('');
                        $("#importDelivery-reasons").val('');
                        $("#importDelivery-actions").val('');
                        $("#importDelivery-criteria").val('');

                        $('#ImportDeliveryModal').modal('show');
                    }
                }]
            },
            topEnd: ['search', 'pageLength']
        },
        data: response,

        order: [[0, "desc"]],
        columnDefs: [
            { className: "p-1", target: "_all" }
        ],
        columns: [
            // SHIPMENT DETAILS
            { data: 'id', visible: false },
            { data: 'blNo' },
            { data: 'shipper' },
            {
                data: 'original_ETA_Port',
                render: function (data, type) {
                    if (!data) return '';

                    if (type === 'display') {
                        const d = new Date(data);
                        return new Intl.DateTimeFormat('en-GB', {
                            day: '2-digit',
                            month: 'short'
                        }).format(d);
                    }

                    // sort, filter, type → return ORIGINAL value
                    return data;
                }
            },
            {
                data: 'revised_ETA_Port',
                render: function (data, type) {
                    if (!data) return '';

                    if (type === 'display') {
                        const d = new Date(data);
                        return new Intl.DateTimeFormat('en-GB', {
                            day: '2-digit',
                            month: 'short'
                        }).format(d);
                    }

                    return data;
                }
            },
            { data: 'reasons' },
            { data: 'bipH_Action' },
            { data: 'criteria', visible: false },
            //ACTIONS 
            {
                data: null, orderable: false, searchable: false, render: function (data, type, row) {
                    return `<button class="btn btn-secondary edit-import-delivery" data-id="${row.id}">Edit</button>
                            <button class="btn btn-danger delete-import-delivery" data-id="${row.id}">Delete</button>`;
                }
            }

        ]
    });

    return import_delivery_table;
}

async function ImportDeliveryCreateUpdate(id = "") {

    let data = {
        "blNo": $("#importDelivery-bilNo").val(),
        "shipper": $("#importDelivery-shipper").val(),
        "original_ETA_Port": $("#importDelivery-originalETA").val(),
        "revised_ETA_Port": $("#importDelivery-revisedETA").val(),
        "reasons": $("#importDelivery-reasons").val(),
        "bipH_Action": $("#importDelivery-actions").val(),
        "criteria": $("#importDelivery-criteria").val()
    };

    if (id) {
        data.id = id;
    }

    if (importDeliveryModal == "create") {
        delete data.id;
        await apiCall(`${API_BASE_URL}/api/ImportDeliveryDashboards`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Import Delivery Created Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#ImportDeliveryModal').modal('hide');
                $("#importDelivery-bilNo").val('');
                $("#importDelivery-shipper").val('');
                $("#importDelivery-originalETA").val('');
                $("#importDelivery-revisedETA").val('');
                $("#importDelivery-reasons").val('');
                $("#importDelivery-actions").val('');
                $("#importDelivery-criteria").val('');

                ImportDelivery();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    } else {
        await apiCall(`${API_BASE_URL}/api/ImportDeliveryDashboards/${id}`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Import Delivery Updated Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#ImportDeliveryModal').modal('hide');

                $("#importDelivery-bilNo").val('');
                $("#importDelivery-shipper").val('');
                $("#importDelivery-originalETA").val('');
                $("#importDelivery-revisedETA").val('');
                $("#importDelivery-reasons").val('');
                $("#importDelivery-actions").val('');
                $("#importDelivery-criteria").val('');

                ImportDelivery();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    }

}

async function ImportDeliveryEdit(id) {
    importDeliveryModal = "edit";

    let data = await apiCall(`${API_BASE_URL}/api/ImportDeliveryDashboards/${id}`, "GET");
    $("#importDelivery-bilNo").val(data.blNo);
    $("#importDelivery-shipper").val(data.shipper);
    $("#importDelivery-originalETA").val(
        data.original_ETA_Port?.split("T")[0]
    );

    $("#importDelivery-revisedETA").val(
        data.revised_ETA_Port?.split("T")[0]
    );
    $("#importDelivery-reasons").val(data.reasons);
    $("#importDelivery-actions").val(data.bipH_Action);
    $("#importDelivery-criteria").val(data.criteria);
    $("#ImportDeliveryModal").modal("show");
}

async function ImportDeliveryDelete(id) {
    await apiCall(`${API_BASE_URL}/api/ImportDeliveryDashboards/Delete/${id}`, "POST")
        .then((response) => {
            swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Import Delivery Deleted Successfully',
                showConfirmButton: false,
                timer: 1500
            });
        })
        .then((response) => {
            ImportDelivery();
        })
        .catch((error) => {
            console.error("API Call Failed:", error.message);
        });
}

$("#createImportDeliveryButton").on("click", async function () {
    await ImportDeliveryCreateUpdate($("#import_delivery_id").val());
});

$(document).on("click", ".edit-import-delivery", async function () {
    const id = $(this).data("id");
    $("#import_delivery_id").val(id);
    await ImportDeliveryEdit(id);
});

$(document).on("click", ".delete-import-delivery", async function () {
    const id = $(this).data("id");

    swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
    }).then(async (result) => {
        if (result.isConfirmed) {
            await ImportDeliveryDelete(id);
        }
    });
});
