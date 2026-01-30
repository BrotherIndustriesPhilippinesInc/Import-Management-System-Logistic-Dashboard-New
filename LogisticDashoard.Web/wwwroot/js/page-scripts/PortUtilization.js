import { startEndDateSearch, lineChartPortUtilizationInit, updateChartsFromTables, getData } from "../helpers/functions.js"

$(async function () {
    //VARIABLES
    let ports = [];
    let tableA = null;
    let tableB = null;

    const toastWatchEdit = document.getElementById('watchEdit')

    const ctx = document.getElementById('portUtilizationChart');
    let chart = lineChartPortUtilizationInit(ctx, true, "Overall Yard Utilization (Batangas)", "Overall Yard Utilization (MICT)");

    //EVENTS
    $(document).on("change", "#startDate, #endDate", function () {
        const startVal = $("#startDate").val();
        const endVal = $("#endDate").val();

        // Now you just pass the table instance
        startEndDateSearch(tableA, startVal, endVal, [1, 2]);
        startEndDateSearch(tableB, startVal, endVal, [1, 2]);

        lineChartPortUtilizationUpdate(chart, tableA, tableB);
    });

    $("#savePort").on("click", function (e) {
        addPort($("#port").val(), $("#cyear").val());
    });


    //////////////////////////////////// ADD #YEAR EVENT //////////////////////////////
    $(document).on("change", "#year, #showAll", async function () {

        await initTables($("#showAll").is(":checked"));

        
        lineChartPortUtilizationUpdate(chart, tableA, tableB);
    });


    ports = await getPortYear();
    assignYear();

    if ($("#port option").length > 0) {
        $("#port").trigger("change");
    }

    $('#portListA-table').on(
        'blur',
        'td[contenteditable="true"], td input[type="date"]',
        async function () {
            let $cell = $(this);
            let $row = $cell.closest('tr');
            let rowId = $row.data('portutil-id');

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
                let rowData = await waitForRowData(tableA,$row);

                // apply update/api/Ports/ByCurrentWeeks/2025
                rowData[field] = newValue;
                rowData.id = rowId;


                // send update to backend
                await fetch(`${API_BASE_URL}/api/PortUtilizations/update/${rowId}`, {
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

    $('#portListA-table').on('change blur', 'td input[type="date"]', function () {
        if ($(this).val()) {
            $(this).removeClass('empty-date');
        } else {
            $(this).addClass('empty-date');
        }
    });

    $('#portListB-table').on(
        'blur',
        'td[contenteditable="true"], td input[type="date"]',
        async function () {
            let $cell = $(this);
            let $row = $cell.closest('tr');
            let rowId = $row.data('portutil-id');

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
                let rowData = await waitForRowData(tableB, $row);

                // apply update
                rowData[field] = newValue;
                rowData.id = rowId;


                // send update to backend
                await fetch(`${API_BASE_URL}/api/PortUtilizations/update/${rowId}`, {
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

    $('#portListB-table').on('change blur', 'td input[type="date"]', function () {
        if ($(this).val()) {
            $(this).removeClass('empty-date');
        } else {
            $(this).addClass('empty-date');
        }
    });

    //FUNCTIONS
    async function initTables(showAll) {
        let year = $("#year").val();
        let selectedPort = "Batangas (POB)";
        let selectedPortB = "Manila North (MICT)"

        let portInfo = null;
        let portInfoB = null;

        if (showAll) {
            portInfo = await getData(`Ports/SpecificPorts/${selectedPort}/${year}`);
            portInfoB = await getData(`Ports/SpecificPorts/${selectedPortB}/${year}`);
        } else {
            portInfo = await getData(`Ports/ByCurrentWeeks/${year}/${selectedPort}`);
            portInfoB = await getData(`Ports/ByCurrentWeeks/${year}/${selectedPortB}`);
        }

        const cols = [
            { data: 'week', title: 'Week' },
            //{ data: 'week', title: 'Week', visible: false },
            { data: 'startDate', title: 'Start Date' },
            { data: 'endDate', title: 'End Date' },
            { data: 'overall_Yard_Utilization', title: 'Overall <br>Yard <br>Utilization' },
            { data: 'vessels_At_Berth', title: 'At <br>Berth' },
            { data: 'vessels_At_Anchorage_Waiting', title: 'At <br>Anchorage <br>(Waiting)' },
        ];


        if (tableA === null) {
            // ✅ First initialization
            tableA = $('#portListA-table').DataTable({
                fixedColumns: {
                    start: 4,
                },
                layout: {
                    topStart: { buttons: ['colvis'] },
                    topEnd: ['search', 'pageLength'],
                },
                //order: [[0, 'desc']],
                data: portInfo.portUtilizations,
                fixedHeader: true,
                autoWidth: false,
                scrollX: true,
                columnDefs: [
                    { className: "p-1 text-nowrap", target: "_all" },

                ],
                columns: cols,
                createdRow: function (row, data) {
                    $(row).attr('data-portutil-id', data.id);

                    $('td', row).each(function (i) {
                        let colName = cols[i].data;
                        $(this).attr("data-field", colName);

                        if (colName === 'week') return; // first column not editable

                        if (colName === 'startDate' || colName === 'endDate') {
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
                            $(this).addClass('portUtil-delay-row');
                            $(this).attr("contenteditable", true); // if you still want it editable
                        }
                        else {
                            $(this).attr("contenteditable", true);
                        }
                    });
                }
            });
        }
        else {
            // ✅ Just update data, no flicker
            tableA.clear().rows.add(portInfo.portUtilizations).draw();
        }



        if (tableB === null) {
            // ✅ First initialization
            tableB = $('#portListB-table').DataTable({
                fixedColumns: {
                    start: 4,
                },
                layout: {
                    topStart: { buttons: ['colvis'] },
                    topEnd: ['search', 'pageLength'],
                },
                //order: [[0, 'desc']],
                data: portInfoB.portUtilizations,
                fixedHeader: true,
                autoWidth: false,
                scrollX: true,
                columnDefs: [
                    { className: "p-1 text-nowrap", target: "_all" },
                ],
                columns: cols,
                createdRow: function (row, data) {
                    $(row).attr('data-portutil-id', data.id);

                    $('td', row).each(function (i) {
                        let colName = cols[i].data;
                        $(this).attr("data-field", colName);

                        if (colName === 'week') return; // first column not editable

                        if (colName === 'startDate' || colName === 'endDate') {
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
                            $(this).addClass('portUtil-delay-row');
                            $(this).attr("contenteditable", true); // if you still want it editable
                        }
                        else {
                            $(this).attr("contenteditable", true);
                        }
                    });
                }
            });
        }
        else {
            // ✅ Just update data, no flicker
            tableB.clear().rows.add(portInfoB.portUtilizations).draw();
        }
    }

    async function getPortYear() {
        const data = await getData("Ports/distinct-ports");

        //const selectRoute = $("#port");
        //selectRoute.empty();

        //data.forEach(item => {
        //    const option = $("<option>")
        //        .val(item.name)
        //        .text(item.name);
        //    selectRoute.append(option);
        //});

        return data; // still return so you can use it
    }

    function showToast() {
        const toast = new bootstrap.Toast(toastWatchEdit);
        toast.show();
    }

    function waitForRowData(table, $row, timeout = 500) {
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

    function lineChartPortUtilizationUpdate(chart, tableA, tableB) {
        // 1. Get the actual data objects for the filtered rows
        const filteredDataA = tableA.rows({ search: 'applied' }).data().toArray();
        const filteredDataB = tableB.rows({ search: 'applied' }).data().toArray();

        // 2. Helper to format dates like "Aug 01"
        const formatDate = (dateStr) => {
            if (!dateStr) return '';
            const date = new Date(dateStr);
            return date.toLocaleDateString('en-US', {
                month: 'short',
                day: '2-digit'
            });
        };

        // 3. Generate the multi-line labels: ["Week 1", "Dec 28 - Jan 03"]
        const labels = filteredDataA.map(row => {
            const weekLabel = `Week ${row.week}`;
            const dateRange = (row.startDate && row.endDate)
                ? `${formatDate(row.startDate)} - ${formatDate(row.endDate)}`
                : '';
            return [weekLabel, dateRange]; // This array creates the line break
        });

        // 4. Extract utilization data
        const utilizationA = filteredDataA.map(row => row.overall_Yard_Utilization);
        const utilizationB = filteredDataB.map(row => row.overall_Yard_Utilization);

        // 5. Update the chart
        chart.data.labels = labels;
        chart.data.datasets[0].data = utilizationA;
        chart.data.datasets[1].data = utilizationB;

        chart.options.plugins.title.text = [
            $("#routeName").text() || "Port Summary", // Fallback if text is empty
            'Overall Yard Utilization'
        ];

        chart.update();
    }
    function assignYear() {
        const selectYear = $("#year");
        selectYear.empty();

        // collect all years from all ports
        const allYears = ports.flatMap(port => port.years);

        // make them unique & sorted
        const uniqueYears = [...new Set(allYears)].sort();

        // append to dropdown
        uniqueYears.forEach(year => {
            const option = $("<option>").val(year).text(year);
            selectYear.append(option);
        });

        // select first year by default
        if (uniqueYears.length > 0) {
            selectYear.val(uniqueYears[0]);
            $("#year").trigger("change");
        }
    }

    async function addPort(portName, year) {
        await getData(`Ports/SpecificPorts/${portName}/${year}`).then(data => {
            swal.fire({
                icon: "success",
                title: "Port saved successfully!",
                showConfirmButton: false,
                timer: 1000,
            }).then(() => {
                // reload AFTER swal closes
                location.reload();
            }).catch(error =>
                console.log(error)
            );
        });
    }

});