import { startEndDateSearch, lineChartPortUtilizationInit, updateChartsFromTables, getData, postData } from "../helpers/functions.js"

$(async function () {
    //VARIABLES
    let berths = [];
    let tableA = null;
    let tableB = null;

    const toastWatchEdit = document.getElementById('watchEdit')

    const ctx = document.getElementById('berthUtilizationChart');
    let chart = lineChartPortUtilizationInit(ctx, false, "Vessels at Berth-Manila North (MICT)", "Vessels at Berth-Batangas (POB)");

    //EVENTS
    $(document).on("change", "#startDate, #endDate", function () {
        const startVal = $("#startDate").val();
        const endVal = $("#endDate").val();

        // Now you just pass the table instance
        startEndDateSearch(tableA, startVal, endVal, [1, 2]);
        startEndDateSearch(tableB, startVal, endVal, [1, 2]);

        lineChartBerthUtilizationUpdate(chart, tableA, tableB);
    });

    $("#saveBerth").on("click", function (e) {
        addBerth($("#saveBerth").val(), $("#cyear").val());
    });


    //////////////////////////////////// ADD #YEAR EVENT //////////////////////////////
    $(document).on("change", "#year, #showAll", async function () {

        await initTables($("#showAll").is(":checked"));

        lineChartBerthUtilizationUpdate(chart, tableA, tableB);
    });


    berths = await getBerthYear();
    assignYear();

    $('#berthListA-table').on(
        'blur',
        'td[contenteditable="true"], td input[type="date"]',
        async function () {
            let $cell = $(this);
            let $row = $cell.closest('tr');
            let rowId = $row.data('berthutil-id');

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
                let rowData = await waitForRowData(tableA, $row);

                // apply update/api/Berth/ByCurrentWeeks/2025
                rowData[field] = newValue;
                rowData.id = rowId;


                // send update to backend
                await fetch(`${API_BASE_URL}/api/BerthingStatus/update/${rowId}`, {
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

    $('#berthListA-table').on('change blur', 'td input[type="date"]', function () {
        if ($(this).val()) {
            $(this).removeClass('empty-date');
        } else {
            $(this).addClass('empty-date');
        }
    });

    $('#berthListB-table').on(
        'blur',
        'td[contenteditable="true"], td input[type="date"]',
        async function () {
            let $cell = $(this);
            let $row = $cell.closest('tr');
            let rowId = $row.data('berthutil-id');

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
                await fetch(`${API_BASE_URL}/api/BerthingStatus/update/${rowId}`, {
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

    $('#berthListB-table').on('change blur', 'td input[type="date"]', function () {
        if ($(this).val()) {
            $(this).removeClass('empty-date');
        } else {
            $(this).addClass('empty-date');
        }
    });

    $("#saveBerth").on("click", async function () {
        await addBerth($("#berth").val(), $("#cyear").val());
    });

    //FUNCTIONS
    async function initTables(showAll) {
        let year = $("#year").val();
        let selectedBerth = "Batangas";
        let selectedBerthB = "Manila North (MICT)"

        let berthInfo = null;
        let berthInfoB = null;

        if (showAll) {
            berthInfo = await getData(`BerthingStatus/SpecificBerth/${selectedBerth}/${year}`);
            berthInfoB = await getData(`BerthingStatus/SpecificBerth/${selectedBerthB}/${year}`);
        } else {
            berthInfo = await getData(`BerthingStatus/ByCurrentWeek/${year}/${selectedBerth}`);
            berthInfoB = await getData(`BerthingStatus/ByCurrentWeek/${year}/${selectedBerthB}`);
        }

        const cols = [
            {
                data: 'date',
                render: function (data, type) {
                    if (!data) return '';

                    const d = new Date(data);

                    if (type === 'display') {
                        // show Jan 1 style
                        return d.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
                    }

                    // For sorting/searching, return ISO string
                    return d.toISOString();
                },
                title: 'Date'
            },
            //{ data: 'week', title: 'Week', visible: false },
            { data: 'vesselsAtBerth', title: 'Vessels At Berth' },
            { data: 'vesselsAtAnchorage', title: 'Vessels At Anchorage (Waiting)' },
        ];


        if (tableA === null) {
            // ✅ First initialization
            tableA = $('#berthListA-table').DataTable({
                //fixedColumns: {
                //    start: 4,
                //},
                layout: {
                    topStart: { /*buttons: ['colvis']*/ },
                    topEnd: ['search', 'pageLength'],
                },
                //order: [[0, 'desc']],
                data: berthInfo,
                fixedHeader: true,
                autoWidth: true,
                scrollX: false,
                columnDefs: [
                    { className: "p-1 text-nowrap", target: "_all" },
                ],
                columns: cols,
                createdRow: function (row, data) {
                    $(row).attr('data-berthutil-id', data.id);

                    $('td', row).each(function (i) {
                        let colName = cols[i].data;
                        $(this).attr("data-field", colName);

                        if (colName === 'date') return; // first column not editable

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
                            $(this).addClass('berthUtil-delay-row');
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
            tableA.clear().rows.add(berthInfo).draw();
        }



        if (tableB === null) {
            // ✅ First initialization
            tableB = $('#berthListB-table').DataTable({
                fixedColumns: {
                    start: 4,
                },
                layout: {
                    topStart: { /*buttons: ['colvis']*/ },
                    topEnd: ['search', 'pageLength'],
                },
                //order: [[0, 'desc']],
                data: berthInfoB,
                fixedHeader: true,
                autoWidth: true,
                scrollX: false,
                columnDefs: [
                    { className: "p-1 text-nowrap", target: "_all" },
                ],
                columns: cols,
                createdRow: function (row, data) {
                    $(row).attr('data-berthutil-id', data.id);

                    $('td', row).each(function (i) {
                        let colName = cols[i].data;
                        $(this).attr("data-field", colName);

                        if (colName === 'date') return; // first column not editable

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
                            $(this).addClass('berthUtil-delay-row');
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
            tableB.clear().rows.add(berthInfoB).draw();
        }
    }

    async function getBerthYear() {
        const data = await getData("BerthingStatus/distinct-ports");

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
    function sanitizeData(arr) {
        return arr.map(v => {
            if (v === "" || v === null) return null;
            let num = Number(v);
            return isNaN(num) ? null : num;
        });
    }

    function lineChartBerthUtilizationUpdate(chart, tableA, tableB) {
        // Weeks from first column (only filtered rows)
        let weeksRaw = tableA.column(0, { search: 'applied' }).data().toArray();

        // Convert to "Mon-22" style labels
        let weeks = weeksRaw.map(d => {
            let date = new Date(d);
            let options = { month: 'short', day: 'numeric' }; // e.g. "Sep 22"
            return date.toLocaleDateString('en-US', options);
        });

        // Utilization data (only filtered rows)
        let utilizationA = sanitizeData(tableA.column(1, { search: 'applied' }).data().toArray());
        let utilizationB = sanitizeData(tableB.column(1, { search: 'applied' }).data().toArray());

        chart.data.labels = weeks;
        chart.data.datasets[0].data = utilizationA;
        chart.data.datasets[1].data = utilizationB;

        chart.options.plugins.title.text = [
            $("#routeName").text(),
            'Berthing Status'
        ];

        chart.update();
    }

    function assignYear() {
        const selectYear = $("#year");
        selectYear.empty();

        // collect all years from all ports
        const allYears = berths.flatMap(berth => berth.years);

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

    async function addBerth(portName, year) {
        await postData("BerthingStatus", { "portName": portName, "calendarYear": year }).then((res) => {
            if (res) {
                swal.fire({
                    icon: "success",
                    title: "Berth saved successfully!",
                    showConfirmButton: false,
                    timer: 1000,
                }).then(() => {
                    // reload AFTER swal closes
                    location.reload();
                });
            }
        });
    }

});