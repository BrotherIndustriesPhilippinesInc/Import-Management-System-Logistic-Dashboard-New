import apiCall from "../helpers/APICall.js";

$(async function () {
    let uploadDate = await getUploadDateAndTime();

    const table = $('#sea-freight-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis']
            },
            topEnd: ['search', 'pageLength'],

        },
        ajax: {
            url: `${API_BASE_URL}/api/SeaFreightScheduleMonitorings?createdDateTime=${uploadDate}`,
            method: "GET",
            dataSrc: '',
            error: function (xhr, status, error) {
                if (xhr.status === 404) {
                    table.clear().draw();
                    alert("No Data Found (404)");
                } else {
                    alert("Error: " + xhr.status + " " + thrown);
                }
            }
        },

        fixedHeader: true,
        autoWidth: true,
        scrollX: true,
        order: [[0, "desc"]], 
        columnDefs: [
            {className: "p-1", target: "_all"}
        ],
        columns: [
            // SHIPMENT DETAILS
            { data: 'id', title: 'ID', visible: false },
            { data: 'itemCategory', title: 'Item Category' },
            { data: 'shipper', title: 'Shipper' },
            { data: 'origin', title: 'Origin' },
            { data: 'bl', title: 'BL' },
            { data: 'inv', title: 'INV' },
            { data: 'carrier_Forwarded', title: 'Carrier Forwarded', visible: false },
            { data: 'port_Of_Discharge', title: 'Port Of Discharge', visible: false },
            { data: 'vessel_Name', title: 'Vessel Name', visible: false },
            { data: 'mode_Of_Shipment', title: 'Mode Of Shipment' },
            { data: 'container_Size_No_Of_PKGS', title: 'Container Size / No. Of PKGS' },
            { data: 'container_No', title: 'Container No' },
            { data: 'trucker', title: 'Trucker' },

            // VESSEL STATUS
            { data: 'original_ETD', title: 'Original ETD', visible: false },
            { data: 'atd', title: 'ATD', visible: false },
            { data: 'original_ETA', title: 'Original ETA', visible: false },
            { data: 'latest_ETA', title: 'Latest ETA', visible: false },
            { data: 'ata', title: 'ATA', class: "text-nowrap" },
            { data: 'atB_Date', title: 'ATB Date', visible: false },
            { data: 'atB_Time', title: 'ATB Time', visible: false },
            { data: 'no_Of_Days_Delayed_ETD_ATD', title: 'Days Delayed ETD→ATD', visible: false },
            { data: 'no_Of_Days_Delayed_ETA_ATA', title: 'Days Delayed ETA→ATA', visible: false },
            { data: 'no_Of_Days_Delayed_ETA_ATB', title: 'Days Delayed ETA→ATB', visible: false },
            { data: 'transit_Days_ATD_ATA', title: 'Transit Days ATD→ATA', visible: false },
            { data: 'vessel_Status', title: 'Vessel Status', visible: false },
            { data: 'vessel_Remarks', title: 'Vessel Remarks', visible: false },

            // SPECIAL REQUIREMENTS
            { data: 'have_Job_Operation', title: 'Have Job Operation', visible: false },
            { data: 'with_Special_Permit', title: 'With Special Permit', visible: false },

            // DELIVERY
            { data: 'bertH_Leadtime', title: 'BERTH Leadtime', visible: false },
            { data: 'etA_BIPH', title: 'ETA BIPH', visible: false  },
            { data: 'orig_RDD', title: 'Original RDD', visible: false },
            { data: 'requested_Del_Date_To_Trucker', title: 'Requested Delivery Date To Trucker' },
            { data: 'requested_Del_Time_To_Trucker', title: 'Requested Delivery Time To Trucker' },
            { data: 'actual_Delivery', title: 'Actual Delivery', visible: false },
            { data: 'actual_Del_Time_To_Trucker', title: 'Actual Delivery Time To Trucker', visible: false },
            { data: 'based_On_BERTH_BIPH_Leadtime', title: 'Based On BERTH/BIPH Leadtime', visible: false },
            { data: 'actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend', title: 'Actual Leadtime (ATA Port → ATA BIPH excl. weekend)', visible: false },

            // SHIPMENT PROCESSING STATUS
            { data: 'step_1', title: 'Step 1', visible: false },
            { data: 'step_2', title: 'Step 2', visible: false },
            { data: 'step_3', title: 'Step 3', visible: false },
            { data: 'step_4', title: 'Step 4', visible: false },
            { data: 'step_5', title: 'Step 5', visible: false },
            { data: 'step_6', title: 'Step 6', visible: false },
            { data: 'actual_Status', title: 'Actual Status', visible: false },
            { data: 'shipment_Processing_Remarks', title: 'Shipment Processing Remarks', visible: false },

            // BOBTAIL / DETENTION
            { data: 'bobtail_Date', title: 'Bobtail Date', visible: false },
            { data: 'requested_Pick_Up_Date', title: 'Requested Pick-Up Date', visible: false },
            { data: 'date_Return_of_Empty_Cntr', title: 'Date Return of Empty Container', visible: false },
            { data: 'freeTime_Valid_Until', title: 'Free Time Valid Until', visible: false },
            { data: 'no_of_Days_with_Detention_Estimate_Only', title: 'Days with Detention (Estimate Only)', visible: false },
            { data: 'no_of_Days_of_Free_Time', title: 'Days of Free Time', visible: false },

            // MP / PURCHASING
            { data: 'requested_Del_Date_To_Ship', title: 'Requested Delivery Date To Ship', visible: false },
            { data: 'priority_Container', title: 'Priority Container', visible: false },
            { data: 'earliest_Shortage_Date', title: 'Earliest Shortage Date', visible: false },
            { data: 'request_to_Unload_AM_or_PM', title: 'Request to Unload AM/PM', visible: false },

            // OTHER
            { data: 'random_Boolean', title: 'Random Boolean', visible: false },
            { data: 'final_Remarks', title: 'Final Remarks', visible: false }
        ],
        
    });

    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

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
            url: `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/uploadNew?createdBy=${adid}`, // 🔥 Replace with your actual API route
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log('Upload success:', response);
                // 😎 Do your success handling here (toast, reload, etc.)
                alert('Upload success');
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('Upload failed:', error);
                // 😤 Handle upload errors
            }
        });
    });

    $(".ship-freight-button").on("click", async function () {
        await searchButtons($(this).data("item_category"), $(this).data("status"));
    });

    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        const min = $('#start-date').val();
        const max = $('#end-date').val();
        const dateStr = data[17]; // e.g., 5 for 6th column
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

        const url = `${API_BASE_URL}/api/SeaFreightScheduleMonitorings?createdDateTime=${selectedDateTime}`;
        table.ajax.url(url).load();
    });

    /*FUNCITONS*/
    async function searchButtons(item_category, status) {
        /*console.log(`CLICKED: ${item_category}, ${status}`);*/
        const url = `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/category_status?item_category=${item_category}&actual_status=${status}`;
        table.ajax.url(url).load();
    }

    async function getUploadDateAndTime() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/UploadDateTime`,
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