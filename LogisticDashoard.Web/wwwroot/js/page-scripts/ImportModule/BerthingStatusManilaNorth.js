import apiCall from "./../../helpers/APICall.js";

let BerthingStatusManilaModal = "";

export async function BerthingStatusManila() {
    let data = await fetch(`${API_BASE_URL}/api/ImportBerthingStatusManilas/`);
    let response = await data.json();
    let filteredData = [];

    //destroy table
    $('#berthing-status-manila-table').DataTable().destroy();
    const portutilizationmanila_status_table = $('#berthing-status-manila-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        BerthingStatusManilaModal = "create";

                        $("#BerthingStatusManila-bilNo").val('');
                        $("#BerthingStatusManila-shipper").val('');
                        $("#BerthingStatusManila-originalETA").val('');
                        $("#BerthingStatusManila-revisedETA").val('');
                        $("#BerthingStatusManila-reasons").val('');
                        $("#BerthingStatusManila-actions").val('');
                        $("#BerthingStatusManila-criteria").val('');

                        $('#BerthingStatusManilaModal').modal('show');
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
                    return `<button class="btn btn-secondary edit-berthing-status-manila-status" data-id="${row.id}">Edit</button>
                            <button class="btn btn-danger delete-berthing-status-manila-status" data-id="${row.id}">Delete</button>`;
                }
            }

        ]
    });
}

async function BerthingStatusManilaCreateUpdate(id = "") {

    let data = {
        "blNo": $("#BerthingStatusManila-bilNo").val(),
        "shipper": $("#BerthingStatusManila-shipper").val(),
        "original_ETA_Port": $("#BerthingStatusManila-originalETA").val(),
        "revised_ETA_Port": $("#BerthingStatusManila-revisedETA").val(),
        "reasons": $("#BerthingStatusManila-reasons").val(),
        "bipH_Action": $("#BerthingStatusManila-actions").val(),
        "criteria": $("#BerthingStatusManila-criteria").val()
    };

    if (id) {
        data.id = id;
    }

    if (BerthingStatusManilaModal == "create") {
        delete data.id;
        await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusManilas`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'berthing-status-manila Status Created Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#BerthingStatusManilaModal').modal('hide');
                $("#BerthingStatusManila-bilNo").val('');
                $("#BerthingStatusManila-shipper").val('');
                $("#BerthingStatusManila-originalETA").val('');
                $("#BerthingStatusManila-revisedETA").val('');
                $("#BerthingStatusManila-reasons").val('');
                $("#BerthingStatusManila-actions").val('');
                $("#BerthingStatusManila-criteria").val('');

                BerthingStatusManila();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    } else {
        await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusManilas/${id}`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'berthing-status-manila Status Updated Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#BerthingStatusManilaModal').modal('hide');

                $("#BerthingStatusManila-bilNo").val('');
                $("#BerthingStatusManila-shipper").val('');
                $("#BerthingStatusManila-originalETA").val('');
                $("#BerthingStatusManila-revisedETA").val('');
                $("#BerthingStatusManila-reasons").val('');
                $("#BerthingStatusManila-actions").val('');
                $("#BerthingStatusManila-criteria").val('');

                BerthingStatusManila();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    }

}

async function BerthingStatusManilaEdit(id) {
    BerthingStatusManilaModal = "edit";

    let data = await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusManilas/${id}`, "GET");
    $("#BerthingStatusManila-bilNo").val(data.blNo);
    $("#BerthingStatusManila-shipper").val(data.shipper);
    $("#BerthingStatusManila-originalETA").val(
        data.original_ETA_Port?.split("T")[0]
    );

    $("#BerthingStatusManila-revisedETA").val(
        data.revised_ETA_Port?.split("T")[0]
    );
    $("#BerthingStatusManila-reasons").val(data.reasons);
    $("#BerthingStatusManila-actions").val(data.bipH_Action);
    $("#BerthingStatusManila-criteria").val(data.criteria);
    $("#BerthingStatusManilaModal").modal("show");
}

async function BerthingStatusManilaDelete(id) {
    await apiCall(`${API_BASE_URL}/api/ImportBerthingStatusManilas/Delete/${id}`, "POST")
        .then((response) => {
            swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'berthing-status-manila Status Deleted Successfully',
                showConfirmButton: false,
                timer: 1500
            });
        })
        .then((response) => {
            BerthingStatusManila();
        })
        .catch((error) => {
            console.error("API Call Failed:", error.message);
        });
}

$("#createBerthingStatusManilaButton").on("click", async function () {
    await BerthingStatusManilaCreateUpdate($("#port_utilization_id").val());
});

$(document).on("click", ".edit-berthing-status-manila-status", async function () {
    const id = $(this).data("id");
    $("#port_utilization_id").val(id);
    await BerthingStatusManilaEdit(id);
});

$(document).on("click", ".delete-berthing-status-manila-status", async function () {
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
            BerthingStatusManilaDelete(id);
        }
    });
});