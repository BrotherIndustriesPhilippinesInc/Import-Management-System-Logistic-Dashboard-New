import apiCall from "./../../helpers/APICall.js";

let BerthingStatusBatangasModal = "";

export async function BerthingStatusBatangas() {
    let data = await fetch(`${API_BASE_URL}/api/ImportBerthingStatusBatangas/`);
    let response = await data.json();
    let filteredData = [];

    //destroy table
    $('#berthing-status-batangas-table').DataTable().destroy();
    const portutilizationBatangas_status_table = $('#berthing-status-batangas-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        BerthingStatusBatangasModal = "create";

                        $("#BerthingStatusBatangas-bilNo").val('');
                        $("#BerthingStatusBatangas-shipper").val('');
                        $("#BerthingStatusBatangas-originalETA").val('');
                        $("#BerthingStatusBatangas-revisedETA").val('');
                        $("#BerthingStatusBatangas-reasons").val('');
                        $("#BerthingStatusBatangas-actions").val('');
                        $("#BerthingStatusBatangas-criteria").val('');

                        $('#BerthingStatusBatangasModal').modal('show');
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
                    return `<button class="btn btn-secondary edit-berthing-status-Batangas-status" data-id="${row.id}">Edit</button>
                            <button class="btn btn-danger delete-berthing-status-Batangas-status" data-id="${row.id}">Delete</button>`;
                }
            }

        ]
    });
}

async function BerthingStatusBatangasCreateUpdate(id = "") {

    let data = {
        "blNo": $("#BerthingStatusBatangas-bilNo").val(),
        "shipper": $("#BerthingStatusBatangas-shipper").val(),
        "original_ETA_Port": $("#BerthingStatusBatangas-originalETA").val(),
        "revised_ETA_Port": $("#BerthingStatusBatangas-revisedETA").val(),
        "reasons": $("#BerthingStatusBatangas-reasons").val(),
        "bipH_Action": $("#BerthingStatusBatangas-actions").val(),
        "criteria": $("#BerthingStatusBatangas-criteria").val()
    };

    if (id) {
        data.id = id;
    }

    if (BerthingStatusBatangasModal == "create") {
        delete data.id;
        await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusBatangas`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'berthing-status-Batangas Status Created Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#BerthingStatusBatangasModal').modal('hide');
                $("#BerthingStatusBatangas-bilNo").val('');
                $("#BerthingStatusBatangas-shipper").val('');
                $("#BerthingStatusBatangas-originalETA").val('');
                $("#BerthingStatusBatangas-revisedETA").val('');
                $("#BerthingStatusBatangas-reasons").val('');
                $("#BerthingStatusBatangas-actions").val('');
                $("#BerthingStatusBatangas-criteria").val('');

                BerthingStatusBatangas();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    } else {
        await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusBatangas/${id}`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'berthing-status-Batangas Status Updated Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#BerthingStatusBatangasModal').modal('hide');

                $("#BerthingStatusBatangas-bilNo").val('');
                $("#BerthingStatusBatangas-shipper").val('');
                $("#BerthingStatusBatangas-originalETA").val('');
                $("#BerthingStatusBatangas-revisedETA").val('');
                $("#BerthingStatusBatangas-reasons").val('');
                $("#BerthingStatusBatangas-actions").val('');
                $("#BerthingStatusBatangas-criteria").val('');

                BerthingStatusBatangas();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    }

}

async function BerthingStatusBatangasEdit(id) {
    BerthingStatusBatangasModal = "edit";

    let data = await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusBatangas/${id}`, "GET");
    $("#BerthingStatusBatangas-bilNo").val(data.blNo);
    $("#BerthingStatusBatangas-shipper").val(data.shipper);
    $("#BerthingStatusBatangas-originalETA").val(
        data.original_ETA_Port?.split("T")[0]
    );

    $("#BerthingStatusBatangas-revisedETA").val(
        data.revised_ETA_Port?.split("T")[0]
    );
    $("#BerthingStatusBatangas-reasons").val(data.reasons);
    $("#BerthingStatusBatangas-actions").val(data.bipH_Action);
    $("#BerthingStatusBatangas-criteria").val(data.criteria);
    $("#BerthingStatusBatangasModal").modal("show");
}

async function BerthingStatusBatangasDelete(id) {
    await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusBatangas/Delete/${id}`, "POST")
        .then((response) => {
            swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'berthing-status-Batangas Status Deleted Successfully',
                showConfirmButton: false,
                timer: 1500
            });
        })
        .then((response) => {
            BerthingStatusBatangas();
        })
        .catch((error) => {
            console.error("API Call Failed:", error.message);
        });
}

$("#createBerthingStatusBatangasButton").on("click", async function () {
    await BerthingStatusBatangasCreateUpdate($("#port_utilization_id").val());
});

$(document).on("click", ".edit-berthing-status-Batangas-status", async function () {
    const id = $(this).data("id");
    $("#port_utilization_id").val(id);
    await BerthingStatusBatangasEdit(id);
});

$(document).on("click", ".delete-berthing-status-Batangas-status", async function () {
    const id = $(this).data("id");

    swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
    }).then((result) => {
        if (result.isConfirmed) {
            BerthingStatusBatangasDelete(id);
        }
    });
});