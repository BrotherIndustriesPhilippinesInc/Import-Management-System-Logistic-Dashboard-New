import { startEndDateSearch } from "../helpers/functions.js"

$(async function () {
    //VARIABLES
    let routes = [];
    let table = null;
    const toastWatchEdit = document.getElementById('watchEdit')

    const ctx = document.getElementById('sailingScheduleChart');
    let chart = chartInit();


    //EVENTS
    $(document).on("change", "#startDate, #endDate", function () {
        const startVal = $("#startDate").val();
        const endVal = $("#endDate").val();

        const table = $('#sailing-schedule-table').DataTable();

        // Now you just pass the table instance
        startEndDateSearch(table, startVal, endVal, [1, 2]);

        updateChartFromTable(table, chart);
    });

    $("#saveRoute").on("click", async function () {
        let from = $("#from").val();
        let to = $("#to").val();
        let fiscalYear = $("#fyear").val();

        let data = {
            from: from,
            to: to,
            fiscalYear : fiscalYear
        };

        let url = `${API_BASE_URL}/api/Routes/`;
        await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
            .then(response => response.json())
            .then(data => {
                if (data.id) {
                    swal.fire({
                        icon: "success",
                        title: "Route saved successfully!",
                        showConfirmButton: false,
                        timer: 1000,

                    }).then(() => {
                        // reload AFTER swal closes
                        location.reload();
                    });

                } else {
                    swal.fire({
                        icon: "error",
                        title: "Error saving route!",
                        showConfirmButton: false,
                        timer: 1000,
                    });
                    throw new Error("Error saving route!");
                }
            })
            .catch(error => {
                console.error("Error:", error);
            });
    });

    $(document).on("change", "#year, #showAll", async function () {
        let fiscalYear = $("#year").val();

        // Split "From - To"
        let [from, to] = $("#route").val().split(" - ");

        // Fetch route info
        let routeInfo = null;
        if ($("#showAll").is(":checked")) {
            routeInfo = await getRoute(from, to, fiscalYear);
        } else {
            routeInfo = await getLatestRouteByWeek(from, to, fiscalYear);
        }
        

        // Define columns once (reuse for both init + row attrs)
        const cols = [
            { data: 'weekNumber', title: 'Week'},
            //{ data: 'week', title: 'Week', visible: false },
            { data: 'start', title: 'Start Date' },
            { data: 'end', title: 'End Date' },
            { data: 'vesselName', title: 'Vessel Name' },
            { data: 'voyNo', title: 'Voy No' },
            { data: 'origin', title: 'Origin' },
            { data: 'originalETD', title: 'Original ETD' },
            { data: 'originalETAMNL', title: 'Original ETA MNL' },
            { data: 'latestETD', title: 'Latest ETD' },
            { data: 'latestETAMNL', title: 'Latest ETA MNL' },
            { data: 'transitDays', title: 'Transit Days' },
            { data: 'delayDeparture', title: 'Departure' },
            { data: 'delayArrival', title: 'Arrival' },
            { data: 'remarks', title: 'Remarks' },
        ];

        if (table === null) {
            // ✅ First initialization
            table = $('#sailing-schedule-table').DataTable({
                fixedColumns: {
                    start: 4,
                },
                layout: {
                    topStart: { buttons: ['colvis'] },
                    topEnd: ['search', 'pageLength'],
                },
                //order: [[0, 'desc']],
                data: routeInfo.schedules,
                fixedHeader: true,
                autoWidth: true,
                scrollX: true,
                columnDefs: [
                    { className: "p-1 text-nowrap", target: "_all" }
                ],
                columns: cols,
                createdRow: function (row, data) {
                    $(row).attr('data-schedule_id', data.id);

                    $('td', row).each(function (i) {
                        let colName = cols[i].data;
                        $(this).attr("data-field", colName);

                        if (colName === 'week' || colName === 'weekNumber') return; // first column not editable

                        if (colName === 'start' || colName === 'end' || colName === 'latestETD' || colName === 'latestETAMNL') {
                            let currentVal = data[colName];
                            let displayVal = currentVal ? currentVal.split('T')[0] : '';
                            let emptyClass = displayVal ? '' : 'empty-date';

                            // Wrap the cell text in a hidden span for search
                            $(this).html(`
                                <span class="d-none">${displayVal}</span>
                                <input type="date" 
                                       class="${emptyClass}" 
                                       data-field="${colName}" 
                                       value="${displayVal}" />
                            `);
                        }
                        else if (colName === 'delayDeparture' || colName === 'delayArrival') {
                            // Add custom class for delay columns
                            $(this).addClass('sailingSchedule-delay-row');
                            $(this).attr("contenteditable", true); // if you still want it editable
                        }
                        else {
                            $(this).attr("contenteditable", true);
                        }
                    });
                }
            });
        } else {
            // ✅ Just update data, no flicker
            table.clear().rows.add(routeInfo.schedules).draw();
        }

        //Redraw chart
        const { weeks, transitDays } = mapSchedules(routeInfo.schedules);

        chart.data.labels = weeks;
        chart.data.datasets[0].data = transitDays;
        chart.options.plugins.title.text = [$("#routeName").text(), 'Transit Days'];
        chart.update();
    });

    $("#route").on("change", function () {
        const selectedRoute = $(this).val();
        const selectYear = $("#year");
        selectYear.empty();

        const route = routes.find(
            item => (item.from + " - " + item.to) === selectedRoute
        );

        if (route && route.years) {
            route.years.forEach(year => {
                const option = $("<option>").val(year).text(year);
                selectYear.append(option);
            });
        }

        //change routeName
        $("#routeName").text(selectedRoute);

        //Select 1st option
        selectYear.val(route.years[0]);
        $("#year").trigger("change");
    });

    routes = await getRouteYear();
    if ($("#route option").length > 0) {
        $("#route").trigger("change");
    }

    $('#sailing-schedule-table').on(
        'blur',
        'td[contenteditable="true"], td input[type="date"]',
        async function () {
            let $cell = $(this);
            let $row = $cell.closest('tr');
            let rowId = $row.data('schedule_id');

            // ✅ get field name
            let field = $cell.is('td')
                ? $cell.data('field')
                : $cell.closest('td').data('field');

            // get new value
            let newValue = $cell.is('td') ? $cell.text() : $cell.val();

            // convert empty date to null
            if ($cell.is('input[type="date"]') && newValue === '') {
                newValue = null;
            }

            try {
                // ✅ wait until DataTables gives us a rowData
                let rowData = await waitForRowData($row);

                // apply update
                rowData[field] = newValue;
                rowData.id = rowId;

                // 🚀 If latestETD or latestETAMNL is edited, recalc transitDays
                if (field === "latestETD" || field === "latestETAMNL") {
                    let etd = rowData.latestETD ? new Date(rowData.latestETD) : null;
                    let eta = rowData.latestETAMNL ? new Date(rowData.latestETAMNL) : null;

                    if (etd && eta) {
                        // compute difference in days
                        let diffMs = eta - etd;
                        let diffDays = Math.ceil(diffMs / (1000 * 60 * 60 * 24));

                        // ✅ send as string (your DTO expects string)
                        rowData.transitDays = diffDays.toString();

                        // ✅ update UI
                        $row.find('td[data-field="transitDays"]').text(diffDays);
                    }
                }



                // send update to backend
                await fetch(`${API_BASE_URL}/api/SailingSchedules/update/${rowId}`, {
                    method: 'POST', // or PUT
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(rowData)
                })
                    .then(r => r.json())
                    .then(response => {
                        if (response.status == 200) {
                            showToast();
                        } else {
                            swal.fire({
                                icon: "error",
                                title: "Update failed!",
                                showConfirmButton: false,
                                timer: 1000
                            });
                        }
                    });

                console.log(`Updated: ID=${rowId}, Field=${field}, Value=${newValue}`);
                $("#year").trigger("change");
            } catch (err) {
                console.error('Update failed:', err);
            }
        }
    );

    $('#sailing-schedule-table').on('change blur', 'td input[type="date"]', function () {
        if ($(this).val()) {
            $(this).removeClass('empty-date');
        } else {
            $(this).addClass('empty-date');
        }
    });

    //FUNCTIONS
    async function getRouteYear() {
        const url = `${API_BASE_URL}/api/Routes/distinct-routes`;
        const response = await fetch(url);
        const data = await response.json();

        const selectRoute = $("#route");
        selectRoute.empty();

        data.forEach(item => {
            const option = $("<option>")
                .val(item.from + " - " + item.to)
                .text(item.from + " - " + item.to);
            selectRoute.append(option);
        });

        return data; // still return so you can use it
    }

    async function getRoute(from, to, fiscalYear) {
        const url = `${API_BASE_URL}/api/Routes/ID?from=${from}&to=${to}&fiscalYear=${fiscalYear}`;
        const response = await fetch(url);
        const data = await response.json();

        return data;
    }

    async function getLatestRouteByWeek(from, to, fiscalYear) {
        const url = `${API_BASE_URL}/api/Routes/ByCurrentWeeks?from=${from}&to=${to}&fiscalYear=${fiscalYear}`;
        const response = await fetch(url);
        const data = await response.json();

        return data;
    }

    function showToast() {
        const toast = new bootstrap.Toast(toastWatchEdit);
        toast.show();
    }

    function chartInit(data) {

        return new Chart(ctx, {
            type: 'line',
            data: {
                datasets: [{
                    label: 'Transit Days',
                    data: data,
                    borderColor: 'rgb(0, 0, 204)',
                    borderWidth: 4,
                    pointBorderColor: 'rgb(0, 0, 255)',
                    pointBackgroundColor: 'rgb(255, 0, 0)',
                    pointBorderWidth: 2,
                    pointRadius: 5,
                    pointStyle: 'rectRounded',
                    rotation: 45,
                }]
            },
            options: {
                layout: {
                    padding: { left: 20, right: 20, top: 20, bottom: 20 }
                },
                scales: {
                    x: {
                        ticks: {
                            color: 'black',
                            font: { size: 12, weight: 'bold' }
                        },
                        grid: { display: false },
                        offset: true
                    },
                    y: {
                        ticks: {
                            color: 'black',
                            font: { size: 12, weight: 'bold' }
                        },
                        grid: { display: true },
                        beginAtZero: true
                    }
                },
                plugins: {
                    datalabels: {
                        anchor: 'start',
                        align: 'bottom',
                        color: '#000',
                        font: { weight: 'bold' },
                        formatter: (value) => value
                    },
                    title: {
                        display: true,
                        text: [$("#routeName").text(), 'Transit Days'],
                        color: 'black',
                        font: { size: 20, weight: 'bold' },
                        padding: { top: 10, bottom: 30 },
                        align: 'center'
                    }
                }
            },
            plugins: [ChartDataLabels]
        });

    }

    function mapSchedules(schedules) {
        const weeks = [];
        const transitDays = [];

        schedules.forEach(s => {
            if (s.transitDays && Number(s.transitDays) > 0) {
                // format start and end dates like "Dec 19" and "Jan 04"
                const formatDate = (dateStr) => {
                    if (!dateStr) return '';
                    const date = new Date(dateStr);
                    return date.toLocaleDateString('en-US', {
                        month: 'short',
                        day: '2-digit'
                    });
                };

                const subLabel = (s.start && s.end)
                    ? `${formatDate(s.start)} - ${formatDate(s.end)}`
                    : '';

                weeks.push([s.week, subLabel]); // ✅ multi-line label
                transitDays.push(Number(s.transitDays));
            }
        });

        return { weeks, transitDays };
    }

    function updateChartFromTable(table, chart) {
        // get filtered rows
        const filteredData = table.rows({ filter: 'applied' }).data().toArray();

        // map into weeks and transit days
        const { weeks, transitDays } = mapSchedules(filteredData);

        // update chart
        chart.data.labels = weeks;
        chart.data.datasets[0].data = transitDays;
        chart.options.plugins.title.text = [$("#routeName").text(), 'Transit Days'];
        chart.update();
    }

    function waitForRowData($row, timeout = 500) {
        return new Promise((resolve, reject) => {
            let waited = 0;
            let check = () => {
                let rowData = table.row($row).data();
                if (rowData && Object.keys(rowData).length > 0) {
                    resolve(rowData);
                } else if (waited >= timeout) {
                    reject("Timeout waiting for rowData");
                } else {
                    waited += 50;
                    setTimeout(check, 50);
                }
            };
            check();
        });
    }

});