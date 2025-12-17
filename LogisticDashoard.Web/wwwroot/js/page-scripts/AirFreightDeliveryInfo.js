import apiCall from "../helpers/APICall.js";

$(async function () {
    const table = $('#air-freight-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis']
            },
            topEnd: ['search', 'pageLength'],
        },
        ajax: {
            url: `${API_BASE_URL}/api/AirFreightScheduleMonitorings`,
            method: "GET",
            dataSrc: '',
            error: function (xhr, status, error) {
                if (xhr.status === 404) {
                    table.clear().draw();
                    alert("No Data Found (404)");
                } else {
                    alert("Error: " + xhr.status + " " + error);
                }
            }
        },
        fixedHeader: true,
        autoWidth: true,
        scrollX: true,
        scrollY: true,
        order: [[0, "desc"]], 
        columnDefs: [
            { className: "p-1", target: "_all" }
        ],
        columns: [
            // SHIPMENT DETAILS
            { data: 'id', title: 'ID', visible: false},
            { data: 'itemCategory', title: 'Item Category' },
            { data: 'shipper', title: 'Shipper' },
            { data: 'awb', title: 'AWB' },
            { data: 'forwarder_Courier', title: 'Forwarder / Courier' },
            { data: 'broker', title: 'Broker' },
            { data: 'flight_Detail', title: 'Flight Detail' },
            { data: 'invoice_No', title: 'Invoice No', visible: false },
            { data: 'freight_Term', title: 'Freight Term', visible: false },
            { data: 'no_Of_Pkgs', title: 'No. Of PKGS', visible: false },

            // FLIGHT STATUS
            { data: 'original_ETD', title: 'Original ETD', visible: false },
            { data: 'atd', title: 'ATD', visible: false },
            { data: 'original_ETA', title: 'Original ETA', visible: false },
            { data: 'latest_ETA', title: 'Latest ETA', visible: false },
            { data: 'ata', title: 'ATA' },
            { data: 'flight_Status_Remarks', title: 'Flight Status Remarks', visible: false },

            // DELIVERY
            { data: 'import_Permit_Status', title: 'Import Permit Status', visible: false },

            // SPECIAL REQUIREMENTS
            { data: 'have_Arrangement', title: 'Have Arrangement', visible: false },
            { data: 'with_Special_Permit', title: 'With Special Permit', visible: false },

            // DELIVERY CONTINUED
            { data: 'atA_Port_BIPH_Leadtime', title: 'ATA Port / BIPH Leadtime', visible: false },
            { data: 'etA_BIPH_Manual_Computation', title: 'ETA BIPH Manual Computation', visible: false },
            { data: 'requested_Del_Date_To_Ship', title: 'Requested Del. Date To Ship', visible: false },
            { data: 'earliest_Shortage_Date', title: 'Earliest Shortage Date', visible: false },
            { data: 'actual_Del', title: 'Actual Del' },
            { data: 'status', title: 'Status' },
            { data: 'import_Remarks', title: 'Import Remarks', visible: false },
            { data: 'system_Update', title: 'System Update', visible: false }
        ]
    });


    /*EVENTS*/
    $('#uploadInput').on('change', function () {
        const file = this.files[0];

        if (!file) {
            console.warn('No file selected.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: `${API_BASE_URL}/api/AirFreightScheduleMonitorings/upload`, // 🔥 Replace with your actual API route
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log('Upload success:', response);
                // 😎 Do your success handling here (toast, reload, etc.)
            },
            error: function (xhr, status, error) {
                console.error('Upload failed:', error);
                // 😤 Handle upload errors
            }
        });
    });

    $(".air-freight-button").on("click", async function () {
        await searchButtons($(this).data("item_category"), $(this).data("status"));
    });

    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        const min = $('#start-date').val();
        const max = $('#end-date').val();
        const dateStr = data[13]; // e.g., 5 for 6th column
        const date = new Date(dateStr);

        if ((min === "" || date >= new Date(min)) &&
            (max === "" || date <= new Date(max))) {
            return true;
        }
        return false;
    });

    $('#start-date, #end-date').on('change', function () {
        table.draw();
    });

    /*FUNCITONS*/
    async function searchButtons(item_category, status) {
        /*console.log(`CLICKED: ${item_category}, ${status}`);*/

        const url = `${API_BASE_URL}/api/AirFreightScheduleMonitorings/category_status?item_category=${item_category}&actual_status=${status}`;
        table.ajax.url(url).load();
        
    }
}); 