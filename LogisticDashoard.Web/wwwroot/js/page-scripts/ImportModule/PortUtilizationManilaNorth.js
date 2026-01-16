import apiCall from "./../../helpers/APICall.js";

let PortUtilizationManilaNorthModal = "";

export async function PortUtilizationManilaNorth() {
    let data = await fetch(`${API_BASE_URL}/api/ImportPortUtilizationManilas/`);
    let response = await data.json();
    let filteredData = [];

    //destroy table
    $('#port-utilization-manila-table').DataTable().destroy();
    const portutilizationmanila_status_table = $('#port-utilization-manila-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        PortUtilizationManilaNorthModal = "create";

                        $("#PortUtilizationManilaNorth-bilNo").val('');
                        $("#PortUtilizationManilaNorth-shipper").val('');
                        $("#PortUtilizationManilaNorth-originalETA").val('');
                        $("#PortUtilizationManilaNorth-revisedETA").val('');
                        $("#PortUtilizationManilaNorth-reasons").val('');
                        $("#PortUtilizationManilaNorth-actions").val('');
                        $("#PortUtilizationManilaNorth-criteria").val('');

                        $('#PortUtilizationManilaNorthModal').modal('show');
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
                    return `<button class="btn btn-secondary edit-port-utilization-manila-status" data-id="${row.id}">Edit</button>
                            <button class="btn btn-danger delete-port-utilization-manila-status" data-id="${row.id}">Delete</button>`;
                }
            }

        ]
    });
}

async function PortUtilizationManilaNorthCreateUpdate(id = "") {

    let data = {
        "blNo": $("#PortUtilizationManilaNorth-bilNo").val(),
        "shipper": $("#PortUtilizationManilaNorth-shipper").val(),
        "original_ETA_Port": $("#PortUtilizationManilaNorth-originalETA").val(),
        "revised_ETA_Port": $("#PortUtilizationManilaNorth-revisedETA").val(),
        "reasons": $("#PortUtilizationManilaNorth-reasons").val(),
        "bipH_Action": $("#PortUtilizationManilaNorth-actions").val(),
        "criteria": $("#PortUtilizationManilaNorth-criteria").val()
    };

    if (id) {
        data.id = id;
    }

    if (PortUtilizationManilaNorthModal == "create") {
        delete data.id;
        await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationManilas`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'port-utilization-manila Status Created Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#PortUtilizationManilaNorthModal').modal('hide');
                $("#PortUtilizationManilaNorth-bilNo").val('');
                $("#PortUtilizationManilaNorth-shipper").val('');
                $("#PortUtilizationManilaNorth-originalETA").val('');
                $("#PortUtilizationManilaNorth-revisedETA").val('');
                $("#PortUtilizationManilaNorth-reasons").val('');
                $("#PortUtilizationManilaNorth-actions").val('');
                $("#PortUtilizationManilaNorth-criteria").val('');

                PortUtilizationManilaNorth();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    } else {
        await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationManilas/${id}`, "POST", data)
            .then((response) => {
                swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'port-utilization-manila Status Updated Successfully',
                    showConfirmButton: false,
                    timer: 1500
                });
            })
            .then((response) => {
                $('#PortUtilizationManilaNorthModal').modal('hide');

                $("#PortUtilizationManilaNorth-bilNo").val('');
                $("#PortUtilizationManilaNorth-shipper").val('');
                $("#PortUtilizationManilaNorth-originalETA").val('');
                $("#PortUtilizationManilaNorth-revisedETA").val('');
                $("#PortUtilizationManilaNorth-reasons").val('');
                $("#PortUtilizationManilaNorth-actions").val('');
                $("#PortUtilizationManilaNorth-criteria").val('');

                PortUtilizationManilaNorth();
            })
            .catch((error) => {
                console.error("API Call Failed:", error.message);
            });
    }

}

async function PortUtilizationManilaNorthEdit(id) {
    PortUtilizationManilaNorthModal = "edit";

    let data = await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationManilas/${id}`, "GET");
    $("#PortUtilizationManilaNorth-bilNo").val(data.blNo);
    $("#PortUtilizationManilaNorth-shipper").val(data.shipper);
    $("#PortUtilizationManilaNorth-originalETA").val(
        data.original_ETA_Port?.split("T")[0]
    );

    $("#PortUtilizationManilaNorth-revisedETA").val(
        data.revised_ETA_Port?.split("T")[0]
    );
    $("#PortUtilizationManilaNorth-reasons").val(data.reasons);
    $("#PortUtilizationManilaNorth-actions").val(data.bipH_Action);
    $("#PortUtilizationManilaNorth-criteria").val(data.criteria);
    $("#PortUtilizationManilaNorthModal").modal("show");
}

async function PortUtilizationManilaNorthDelete(id) {
    await apiCall(`${API_BASE_URL}/api/ImportPortUtilizationManilas/Delete/${id}`, "POST")
        .then((response) => {
            swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'port-utilization-manila Status Deleted Successfully',
                showConfirmButton: false,
                timer: 1500
            });
        })
        .then((response) => {
            PortUtilizationManilaNorth();
        })
        .catch((error) => {
            console.error("API Call Failed:", error.message);
        });
}

$("#createPortUtilizationManilaNorthButton").on("click", async function () {
    await PortUtilizationManilaNorthCreateUpdate($("#port_utilization_id").val());
});

$(document).on("click", ".edit-port-utilization-manila-status", async function () {
    const id = $(this).data("id");
    $("#port_utilization_id").val(id);
    await PortUtilizationManilaNorthEdit(id);
});

$(document).on("click", ".delete-port-utilization-manila-status", async function () {
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
            PortUtilizationManilaNorthDelete(id);
        }
    });
});