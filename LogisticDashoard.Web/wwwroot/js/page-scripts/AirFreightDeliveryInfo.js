import apiCall from "../helpers/APICall.js";

$(async function () {
    let uploadDate = await getUploadDateAndTime();

    const table = $('#air-freight-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis']
            },
            topEnd: ['search', 'pageLength'],
        },
        ajax: {
            url: `${API_BASE_URL}/api/AirFreightScheduleMonitorings?createdDateTime=${uploadDate}`,
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
            /*{ data: 'origin', title: 'Origin'},*/
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
            { data: 'ata', title: 'ATA'},
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

        let user = localStorage.getItem('user');
        user = JSON.parse(user);
        let adid = user.EmpNo;

        if (!file) {
            console.warn('No file selected.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: `${API_BASE_URL}/api/AirFreightScheduleMonitorings/uploadNew?createdBy=${adid}`, // 🔥 Replace with your actual API route
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

    $("#upload-date-time").on("change", function () {
        const selectedDateTime = $(this).val();

        const url = `${API_BASE_URL}/api/AirFreightScheduleMonitorings?createdDateTime=${selectedDateTime}`;
        table.ajax.url(url).load();
    });

    /*FUNCITONS*/
    async function searchButtons(item_category, status) {
        /*console.log(`CLICKED: ${item_category}, ${status}`);*/

        const url = `${API_BASE_URL}/api/AirFreightScheduleMonitorings/category_status?item_category=${item_category}&actual_status=${status}`;
        table.ajax.url(url).load();
        
    }

    async function getUploadDateAndTime() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: `${API_BASE_URL}/api/AirFreightScheduleMonitorings/UploadDateTime`,
                type: 'GET',
                success: function (response) {

                    const $select = $('#upload-date-time');
                    $select.empty();

                    if (!response || response.length === 0) {
                        resolve(null);
                        return;
                    }

                    response.forEach((item, index) => {
                        let date = new Date(item.dateCreated);

                        let display = date.toLocaleString('en-US', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit',
                            hour12: true
                        });

                        let value = date.toISOString();

                        $select.append(
                            `<option value="${value}">${display}</option>`
                        );
                    });

                    // ✅ select FIRST (latest) upload explicitly
                    const selectedValue = response[0]
                        ? new Date(response[0].dateCreated).toISOString()
                        : null;

                    $select.val(selectedValue);

                    resolve(selectedValue);
                },
                error: reject
            });
        });
    }

}); 