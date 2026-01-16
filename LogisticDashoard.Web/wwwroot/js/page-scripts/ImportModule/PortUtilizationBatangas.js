import apiCall from "./../../helpers/APICall.js";

let PortUtilizationBatangasModal = "";

export async function PortUtilizationBatangas() {
    let data = await fetch(`${API_BASE_URL}/api/ImportPortUtilizationBatangas/`);
    let response = await data.json();
    let filteredData = [];

    //destroy table
    $('#port-utilization-batangas-table').DataTable().destroy();
    const portutilizationbatangas_status_table = $('#port-utilization-batangas-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        PortUtilizationBatangasModal = "create";

                        $("#PortUtilizationBatangas-bilNo").val('');
                        $("#PortUtilizationBatangas-shipper").val('');
                        $("#PortUtilizationBatangas-originalETA").val('');
                        $("#PortUtilizationBatangas-revisedETA").val('');
                        $("#PortUtilizationBatangas-reasons").val('');
                        $("#PortUtilizationBatangas-actions").val('');
                        $("#PortUtilizationBatangas-criteria").val('');

                        $('#PortUtilizationBatangasModal').modal('show');
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
                    return `<button class="btn btn-secondary edit-port-utilization-batangas-status" data-id="${row.id}">Edit</button>
                            <button class="btn btn-danger delete-port-utilization-batangas-status" data-id="${row.id}">Delete</button>`;
                }
            }

        ]
    });
}

async function PortUtilizationBatangasCreateUpdate(id = "") {

    let data = {
        "blNo": $("#PortUtilizationBatangas-bilNo").val(),
        "shipper": $("#PortUtilizationBatangas-shipper").val(),
        "original_ETA_Port": $("#PortUtilizationBatangas-originalETA").val(),
        "revised_ETA_Port": $("#PortUtilizationBatangas-revisedETA").val(),
        "reasons": $("#PortUtilizationBatangas-reasons").val(),
        "bipH_Action": $("#PortUtilizationBatangas-actions").val(),
        "criteria": $("#PortUtilizationBatangas-criteria").val()
    };

    if (id) {
        data.id = id;
    }

    if (PortUtilizationBatangasModal == "create") {
        delete data.id;
        await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationBatangas`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'port-utilization-batangas Status Created Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#PortUtilizationBatangasModal').modal('hide');
                $("#PortUtilizationBatangas-bilNo").val('');
                $("#PortUtilizationBatangas-shipper").val('');
                $("#PortUtilizationBatangas-originalETA").val('');
                $("#PortUtilizationBatangas-revisedETA").val('');
                $("#PortUtilizationBatangas-reasons").val('');
                $("#PortUtilizationBatangas-actions").val('');
                $("#PortUtilizationBatangas-criteria").val('');

                PortUtilizationBatangas();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    } else {
        await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationBatangas/${id}`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'port-utilization-batangas Status Updated Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#PortUtilizationBatangasModal').modal('hide');

                $("#PortUtilizationBatangas-bilNo").val('');
                $("#PortUtilizationBatangas-shipper").val('');
                $("#PortUtilizationBatangas-originalETA").val('');
                $("#PortUtilizationBatangas-revisedETA").val('');
                $("#PortUtilizationBatangas-reasons").val('');
                $("#PortUtilizationBatangas-actions").val('');
                $("#PortUtilizationBatangas-criteria").val('');

                PortUtilizationBatangas();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    }

}

async function PortUtilizationBatangasEdit(id) {
    PortUtilizationBatangasModal = "edit";

    let data = await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationBatangas/${id}`, "GET");
    $("#PortUtilizationBatangas-bilNo").val(data.blNo);
    $("#PortUtilizationBatangas-shipper").val(data.shipper);
    $("#PortUtilizationBatangas-originalETA").val(
        data.original_ETA_Port?.split("T")[0]
    );

    $("#PortUtilizationBatangas-revisedETA").val(
        data.revised_ETA_Port?.split("T")[0]
    );
    $("#PortUtilizationBatangas-reasons").val(data.reasons);
    $("#PortUtilizationBatangas-actions").val(data.bipH_Action);
    $("#PortUtilizationBatangas-criteria").val(data.criteria);
    $("#PortUtilizationBatangasModal").modal("show");
}

async function PortUtilizationBatangasDelete(id) {
    await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationBatangas/Delete/${id}`, "POST")
        .then((response) => {
            swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'port-utilization-batangas Status Deleted Successfully',
                showConfirmButton: false,
                timer: 1500
            });
        })
        .then((response) => {
            PortUtilizationBatangas();
        })
        .catch((error) => {
            console.error("API Call Failed:", error.message);
        });
}

$("#createPortUtilizationBatangasButton").on("click", async function () {
    await PortUtilizationBatangasCreateUpdate($("#port_utilization_id").val());
});

$(document).on("click", ".edit-port-utilization-batangas-status", async function () {
    const id = $(this).data("id");
    $("#port_utilization_id").val(id);
    await PortUtilizationBatangasEdit(id);
});

$(document).on("click", ".delete-port-utilization-batangas-status", async function () {
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
            PortUtilizationBatangasDelete(id);
        }
    });
});
