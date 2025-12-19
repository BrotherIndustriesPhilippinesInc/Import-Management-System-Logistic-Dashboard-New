$(async function () {
    let uploadDate = await getUploadDateAndTime();
    let actualDelivery = [];

    $("#refresh").on("click", async function () {
        await refreshAllCharts(uploadDate);
    });

    //#region vesselTransitDays 
    // Fetch data once
    const vesselTransitRes = await fetch(
        `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselTransitDays?createdDateTime=${uploadDate}`
    );
    const vesselTransitData = await vesselTransitRes.json();

    $("#upload-date-time").on('change', function () {
        uploadDate = this.value;
        vesselTransitChart(uploadDate, vesselTransitData);
        //RESET FILTERS
        populateVesselCheckboxes(vesselTransitData);
    });

    // Populate filters
    populateVesselCheckboxes(vesselTransitData);

    // Initial chart
    vesselTransitChart(uploadDate, vesselTransitData);

    // Filter events
    $('#vessel-transit-origin-filter, #vessel-transit-carrier-filter, #vessel-transit-mode-filter').on('change', function () {
        const selectedOrigins = getCheckedValues($('#vessel-transit-origin-filter'));
        const selectedCarriers = getCheckedValues($('#vessel-transit-carrier-filter'));
        const selectedModes = getCheckedValues($('#vessel-transit-mode-filter'));

        const filteredData = vesselTransitData.filter(x =>
            selectedOrigins.includes(x.origin) &&
            selectedCarriers.includes(x.carrier_Forwarded) &&
            selectedModes.includes(x.mode_Of_Shipment)
        );

        vesselTransitChart(uploadDate, filteredData);
    });

    // Toggle Origin filter
    $('#vessel-transit-origin-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Carrier filter
    $('#vessel-transit-carrier-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    $('#vessel-transit-mode-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    //#endregion

    //#region OTD Achievement Rate
    const otdAchievementRes = await fetch(
        `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/OtdAchievementRate?createdDateTime=${uploadDate}`
    );
    const otdAchievementData = await otdAchievementRes.json();

    populateTruckerCheckboxes(otdAchievementData);

    // FORCE first selection render
    const first = otdAchievementData[0];
    otdAchievementPieChart(uploadDate, [first]);


    $('#otd-trucker-filter').on('change', 'input[name="otd-trucker"]', function () {
        const selected = $('#otd-trucker-filter input[name="otd-trucker"]:checked').val();

        if (!selected) return;

        const filtered = otdAchievementData.filter(x =>
            x.trucker === selected
        );

        otdAchievementPieChart(uploadDate, filtered);
    });



    $('#otd-trucker-filter .filter-label').on('click', function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text((_, old) =>
            old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇')
        );
    });

    $('#upload-date-time').on('change', function () {
        uploadDate = this.value;
        populateTruckerCheckboxes(otdAchievementData);
        otdAchievementPieChart(uploadDate, otdAchievementData);
    });

    //#endregion

    //#region OTD Achievement Rate2
    populateTruckerCheckboxes2(otdAchievementData);

    // FORCE second selection render
    const second = otdAchievementData[1];
    otdAchievementPieChart2(uploadDate, [second]);


    $('#otd-trucker-filter2').on('change', 'input[name="otd-trucker2"]', function () {
        const selected = $('#otd-trucker-filter2 input[name="otd-trucker2"]:checked').val();

        if (!selected) return;

        const filtered = otdAchievementData.filter(x =>
            x.trucker === selected
        );

        otdAchievementPieChart2(uploadDate, filtered);
    });



    $('#otd-trucker-filter2 .filter-label').on('click', function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text((_, old) =>
            old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇')
        );
    });

    $('#upload-date-time').on('change', function () {
        uploadDate = this.value;
        populateTruckerCheckboxes2(otdAchievementData);
        otdAchievementPieChart2(uploadDate, otdAchievementData);
    });

    //#endregion

    //#region Average Processing Leadtime per forwarder
    
    const averageProcessingRes = await fetch(
        `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/AverageProcessingLeadtimePerForwarder?createdDateTime=${uploadDate}&actualDelivery=${actualDelivery}`
    );
    const averageProcessingData = await averageProcessingRes.json();

    $("#upload-date-time").on('change', function () {
        uploadDate = this.value;
        averageProcessingChart(uploadDate, averageProcessingData);
        //RESET FILTERS
        populateAverageProcessingCheckboxes(averageProcessingData);
    });

    populateAverageProcessingCheckboxes(averageProcessingData);

    averageProcessingChart(uploadDate, averageProcessingData);

    // Filter events
    $('#average-processing-actual-delivery-filter, #average-processing-type-of-packaging-filter, #average-processing-trucker-filter, #average-processing-port-of-discharge-filter').on('change', function () {
        const selectedActualDeliveries = getCheckedValues($('#average-processing-actual-delivery-filter'));
        const selectedTypeOfPackagings = getCheckedValues($('#average-processing-type-of-packaging-filter'));
        const selectedTruckers = getCheckedValues($('#average-processing-trucker-filter'));
        const selectedPorts = getCheckedValues($('#average-processing-port-of-discharge-filter'));

        const filteredData = averageProcessingData.filter(x =>
            selectedActualDeliveries.includes(x.actualDelivery) &&
            selectedTypeOfPackagings.includes(x.modeOfShipment) &&
            selectedTruckers.includes(x.trucker) &&
            selectedPorts.includes(x.portOfDischarge)
        );

        averageProcessingChart(uploadDate, filteredData);
    });

    // Toggle Actual Delivery filter
    $('#average-processing-actual-delivery-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Type of Packaging filter
    $('#average-processing-type-of-packaging-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Trucker filter
    $('#average-processing-trucker-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Port of Discharge
    $('#average-processing-port-of-discharge-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });



    //#endregion

    //#region Average Processing Leadtime per forwarder2
    $("#upload-date-time").on('change', function () {
        uploadDate = this.value;
        averageProcessingChart2(uploadDate, averageProcessingData);
        //RESET FILTERS
        populateAverageProcessingCheckboxes2(averageProcessingData);
    });

    populateAverageProcessingCheckboxes2(averageProcessingData);

    averageProcessingChart2(uploadDate, averageProcessingData);

    // Filter events
    $('#average-processing-actual-delivery-filter2, #average-processing-type-of-packaging-filter2, #average-processing-trucker-filter2, #average-processing-port-of-discharge-filter2').on('change', function () {
        const selectedActualDeliveries = getCheckedValues($('#average-processing-actual-delivery-filter2'));
        const selectedTypeOfPackagings = getCheckedValues($('#average-processing-type-of-packaging-filter2'));
        const selectedTruckers = getCheckedValues($('#average-processing-trucker-filter2'));
        const selectedPorts = getCheckedValues($('#average-processing-port-of-discharge-filter2'));

        const filteredData = averageProcessingData.filter(x =>
            selectedActualDeliveries.includes(x.actualDelivery) &&
            selectedTypeOfPackagings.includes(x.modeOfShipment) &&
            selectedTruckers.includes(x.trucker) &&
            selectedPorts.includes(x.portOfDischarge)
        );

        averageProcessingChart2(uploadDate, filteredData);
    });

    // Toggle Actual Delivery filter
    $('#average-processing-actual-delivery-filter2 .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Type of Packaging filter
    $('#average-processing-type-of-packaging-filter2 .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Trucker filter
    $('#average-processing-trucker-filter2 .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Port of Discharge
    $('#average-processing-port-of-discharge-filter2 .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });



    //#endregion

    //#region Vessel Delay Per Port 
    // Fetch data once
    const vesselDelayPerPortRes = await fetch(
        `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselDelayPerPort?createdDateTime=${uploadDate}`
    );
    const vesselDelayPerPortData = await vesselDelayPerPortRes.json();

    $("#upload-date-time").on('change', function () {
        uploadDate = this.value;
        vesselDelayChart(uploadDate, vesselDelayPerPortData);
        //RESET FILTERS
        populateVesselDelayCheckboxes(vesselDelayPerPortData);
    });

    // Populate filters
    populateVesselDelayCheckboxes(vesselDelayPerPortData);

    // Initial chart
    vesselDelayChart(uploadDate, vesselDelayPerPortData);

    // Filter events
    $('#vessel-delay-per-port-port-of-discharge-filter, #vessel-delay-per-port-mode-of-shipment-filter').on('change', function () {
        const selectedPortOfDischarge = getCheckedValues($('#vessel-delay-per-port-port-of-discharge-filter'));
        const selectedModes = getCheckedValues($('#vessel-delay-per-port-mode-of-shipment-filter'));

        const filteredData = vesselDelayPerPortData.filter(x =>
            selectedPortOfDischarge.includes(x.portOfDischarge) &&
            selectedModes.includes(x.modeOfShipment)
        );

        vesselDelayChart(uploadDate, filteredData);
    });

    // Toggle Origin filter
    $('#vessel-delay-per-port-port-of-discharge-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });

    // Toggle Carrier filter
    $('#vessel-delay-per-port-mode-of-shipment-filter .filter-label').click(function () {
        $(this).siblings('.checkboxes').slideToggle(200);
        $(this).text(function (i, old) {
            return old.includes('⬇') ? old.replace('⬇', '⬆') : old.replace('⬆', '⬇');
        });
    });


    //#endregion

});


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

//#region vesselTransitChart
async function vesselTransitChart(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselTransitDays?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    const carriers = [...new Set(data.map(x => x.carrier_Forwarded))];
    const origins = [...new Set(data.map(x => x.origin))];

    const datasets = origins.map((origin, index) => {
        const hue = Math.round((360 / origins.length) * index);
        const color = `hsl(${hue}, 65%, 50%)`;

        return {
            label: origin,
            data: carriers.map(carrier => {
                const match = data.find(
                    x => x.origin === origin && x.carrier_Forwarded === carrier
                );
                return match ? Number(match.transit_Days_ATD_ATA) : 0;
            }),
            backgroundColor: color,
            borderWidth: 0
        };
    });

    // Destroy previous chart if exists
    if (window.vesselChartInstance) window.vesselChartInstance.destroy();

    window.vesselChartInstance = new Chart(document.getElementById('vessel_transit_days'), {
        type: 'bar',
        data: {
            labels: carriers,
            datasets: datasets
        },
        options: {
            responsive: true,
            datasets: {
                bar: {
                    maxBarThickness: 40 // max width for bars
                }
            },

            plugins: {
                title: {
                    display: true,
                    text: 'VESSEL TRANSIT DAYS',
                    color: '#000',
                    font: {
                        size: 18,
                        weight: 'bold',
                    },
                    padding: { top: 10, bottom: 20 }
                },
                legend: {
                    position: 'bottom',
                    labels: {
                        color: '#000' // Black legend text
                    }
                },
                datalabels: {
                    anchor: 'end',
                    align: 'end',
                    color: '#000',
                    font: { weight: 'bold' },
                    formatter: function (value) {
                        return value === 0 ? '' : value; // Hide zero values
                    }
                }
            },
            interaction: { mode: 'index', intersect: false },
            scales: {
                y: { ticks: { color: '#000' }, title: { display: false, text: 'Transit Days (ATD → ATA)', color: '#000' } },
                x: { ticks: { color: '#000' }, title: { display: false, text: 'Carrier Forwarder', color: '#000' } }
            }
        },
        plugins: [ChartDataLabels]
    });
}

function populateVesselCheckboxes(data) {
    const originContainer = $('#vessel-transit-origin-filter .checkboxes');
    const carrierContainer = $('#vessel-transit-carrier-filter .checkboxes');
    const modeContainer = $('#vessel-transit-mode-filter .checkboxes');

    originContainer.empty();
    carrierContainer.empty();
    modeContainer.empty();

    const origins = [...new Set(data.map(x => x.origin))];
    const carriers = [...new Set(data.map(x => x.carrier_Forwarded))];
    const modes = [...new Set(data.map(x => x.mode_Of_Shipment))];

    origins.forEach(o => {
        originContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${o}" checked> ${o}</label>`);
    });

    carriers.forEach(c => {
        carrierContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });

    modes.forEach(m => {
        modeContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${m}" checked> ${m}</label>`);
    });
}

function getCheckedValues(container) {
    return container.find('input[type=checkbox]:checked').map((i, el) => el.value).get();
}
//#endregion

//#region OTD Achievement Rate
async function otdAchievementPieChart(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/OtdAchievementRate?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    if (!data.length) return;

    // ONLY ONE TRUCKER SHOULD BE HERE
    const trucker = data[0];
    const achieved = trucker.achievementRate;
    const notAchieved = 100 - achieved;

    if (window.otdChartInstance) window.otdChartInstance.destroy();

    window.otdChartInstance = new Chart(
        document.getElementById('otd-achievement-rate'),
        {
            type: 'pie',
            responsive: true,
            data: {
                labels: ['Achieved', 'Not Achieved'],
                datasets: [{
                    data: [achieved, notAchieved],
                    backgroundColor: [
                        'hsl(219, 49%, 45%)',
                        'hsl(224, 42%, 69%)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: `${trucker.trucker} OTD ACHIEVEMENT RATE`,
                        font: { size: 18, weight: 'bold' },
                        color: '#000',
                    },
                    legend: { position: 'right', labels: { color: '#000' } },
                    datalabels: {
                        formatter: v => v === 0 ? '' : `${v.toFixed(2)}%`,
                        color: '#000',
                        font: { weight: 'bold' }
                    }
                }
            },
            plugins: [ChartDataLabels]
        }
    );
}

function populateTruckerCheckboxes(data) {
    const container = $('#otd-trucker-filter .checkboxes');
    container.empty();

    data.forEach((x, i) => {
        container.append(`
            <label>
                <input type="radio" name="otd-trucker" value="${x.trucker}" ${i === 0 ? 'checked' : ''}>
                ${x.trucker}
            </label>
        `);
    });

    // ✅ Make sure filter starts expanded for first load
    container.show();
    $('#otd-trucker-filter .filter-label').text(
        $('#otd-trucker-filter .filter-label').text().replace('⬆', '⬇')
    );
}


function applyTruckerFilterAndRender(uploadDate, fullData) {
    const selectedTruckers = getCheckedValues($('#otd-trucker-filter'));

    const filteredData = fullData.filter(x =>
        selectedTruckers.includes(x.trucker)
    );

    otdAchievementPieChart(uploadDate, filteredData);
}

//#endregion

//#region OTD Achievement Rate2
async function otdAchievementPieChart2(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/OtdAchievementRate?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    if (!data.length) return;

    // ONLY ONE TRUCKER SHOULD BE HERE
    const trucker = data[0];
    const achieved = trucker.achievementRate;
    const notAchieved = 100 - achieved;

    if (window.otdChartInstance2) window.otdChartInstance2.destroy();

    window.otdChartInstance2 = new Chart(
        document.getElementById('otd-achievement-rate2'),
        {
            type: 'pie',
            responsive: true,
            data: {
                labels: ['Achieved', 'Not Achieved'],
                datasets: [{
                    data: [achieved, notAchieved],
                    backgroundColor: [
                        'hsl(96, 42%, 47%)',
                        'hsl(209, 58%, 59%)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: `${trucker.trucker} OTD ACHIEVEMENT RATE`,
                        font: { size: 18, weight: 'bold' },
                        color: '#000',
                    },
                    legend: { position: 'right', labels: { color: '#000' } },
                    datalabels: {
                        formatter: v => v === 0 ? '' : `${v.toFixed(2)}%`,
                        color: '#000',
                        font: { weight: 'bold' }
                    }
                }
            },
            plugins: [ChartDataLabels]
        }
    );
}

function populateTruckerCheckboxes2(data) {
    const container = $('#otd-trucker-filter2 .checkboxes');
    container.empty();

    data.forEach((x, i) => {
        container.append(`
            <label>
                <input type="radio" name="otd-trucker2" value="${x.trucker}" ${i === 1 ? 'checked' : ''}>
                ${x.trucker}
            </label>
        `);
    });

    // ✅ Make sure filter starts expanded for first load
    container.show();
    $('#otd-trucker-filter2 .filter-label').text(
        $('#otd-trucker-filter2 .filter-label').text().replace('⬆', '⬇')
    );
}


function applyTruckerFilterAndRender2(uploadDate, fullData) {
    const selectedTruckers = getCheckedValues($('#otd-trucker-filter2'));

    const filteredData = fullData.filter(x =>
        selectedTruckers.includes(x.trucker)
    );

    otdAchievementPieChart2(uploadDate, filteredData);
}

//#endregion

//#region Average Processing Leadtime per forwarder
async function averageProcessingChart(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/AverageProcessingLeadtimePerForwarder?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    // SAME ROLE as `carriers`
    const truckers = [...new Set(data.map(x => x.trucker))];

    // SAME ROLE as `origins`
    const ports = [...new Set(data.map(x => x.portOfDischarge))];

    // SAME DATASET LOGIC
    const datasets = ports.map((port, index) => {
        const hue = Math.round((360 / ports.length) * index);
        const color = `hsl(${hue}, 65%, 50%)`;

        return {
            label: port,
            data: truckers.map(trucker => {
                const items = data.filter(
                    x => x.portOfDischarge === port && x.trucker === trucker
                );
                if (!items.length) return 0;
                // average across all items for this trucker+port
                return Math.round(items.reduce((sum, x) => sum + x.averageProcessingLeadtime, 0) / items.length * 100) / 100;
            }),

            backgroundColor: color,
            borderWidth: 0
        };
    });

    if (window.averageProcessingChartInstance)
        window.averageProcessingChartInstance.destroy();

    window.averageProcessingChartInstance = new Chart(
        document.getElementById('average-processing'),
        {
            type: 'bar',
            data: {
                labels: truckers,
                datasets
            },
            options: {
                responsive: true,
                datasets: {
                    bar: {
                        maxBarThickness: 40
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'AVERAGE PROCESSING LEAD TIME',
                        font: { size: 18, weight: 'bold' },
                        color: '#000',
                    },
                    legend: {
                        position: 'right', labels: { color: '#000' }
                    },
                    datalabels: {
                        anchor: 'end',
                        align: 'end',
                        font: { weight: 'bold' },
                        formatter: v => v === 0 ? '' : v,
                        color: '#000'
                    }
                },
                interaction: { mode: 'index', intersect: false },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Days',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000'
                        }

                    },
                    x: {
                        title: {
                            display: false,
                            text: 'Trucker',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000'
                        }

                    }
                }
            },
            plugins: [ChartDataLabels]
        }
    );
}

function populateAverageProcessingCheckboxes(data) {
    const actualDeliveryContainer = $('#average-processing-actual-delivery-filter .checkboxes');
    const typeOfPackagingContainer = $('#average-processing-type-of-packaging-filter .checkboxes');
    const truckerContainer = $('#average-processing-trucker-filter .checkboxes');
    const portOfDischargeContainer = $('#average-processing-port-of-discharge-filter .checkboxes');

    actualDeliveryContainer.empty();
    typeOfPackagingContainer.empty();
    truckerContainer.empty();
    portOfDischargeContainer.empty();


    const actualDeliveries = [...new Set(data.map(x => x.actualDelivery))];
    const typeOfPackagings = [...new Set(data.map(x => x.modeOfShipment))]; 
    const truckers = [...new Set(data.map(x => x.trucker))];
    const portsOfDischarge = [...new Set(data.map(x => x.portOfDischarge))];

    actualDeliveries.forEach(o => {
        actualDeliveryContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${o}" checked> ${o}</label>`);
    });

    typeOfPackagings.forEach(c => {
        typeOfPackagingContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });

    truckers.forEach(c => {
        truckerContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });

    portsOfDischarge.forEach(c => {
        portOfDischargeContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });
}

//#endregion

//#region Average Processing Leadtime per forwarder2
async function averageProcessingChart2(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/AverageProcessingLeadtimePerForwarder?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    // SAME ROLE as `carriers`
    const truckers = [...new Set(data.map(x => x.trucker))];

    // SAME ROLE as `origins`
    const ports = [...new Set(data.map(x => x.portOfDischarge))];

    // SAME DATASET LOGIC
    const datasets = ports.map((port, index) => {
        const hue = Math.round((360 / ports.length) * index);
        const color = `hsl(${hue}, 65%, 50%)`;

        return {
            label: port,
            data: truckers.map(trucker => {
                const items = data.filter(
                    x => x.portOfDischarge === port && x.trucker === trucker
                );
                if (!items.length) return 0;
                // average across all items for this trucker+port
                return Math.round(items.reduce((sum, x) => sum + x.averageProcessingLeadtime, 0) / items.length * 100) / 100;
            }),

            backgroundColor: color,
            borderWidth: 0
        };
    });

    if (window.averageProcessingChartInstance2)
        window.averageProcessingChartInstance2.destroy();

    window.averageProcessingChartInstance2 = new Chart(
        document.getElementById('average-processing2'),
        {
            type: 'bar',
            data: {
                labels: truckers,
                datasets
            },
            options: {
                responsive: true,
                datasets: {
                    bar: {
                        maxBarThickness: 40
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'AVERAGE PROCESSING LEAD TIME',
                        font: { size: 18, weight: 'bold' },
                        color: '#000',
                    },
                    legend: {
                        position: 'right', labels: { color: '#000' }
                    },
                    datalabels: {
                        anchor: 'end',
                        align: 'end',
                        font: { weight: 'bold' },
                        formatter: v => v === 0 ? '' : v,
                        color: '#000'
                    }
                },
                interaction: { mode: 'index', intersect: false },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Days',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000'
                        }

                    },
                    x: {
                        title: {
                            display: false,
                            text: 'Trucker',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000'
                        }

                    }
                }
            },
            plugins: [ChartDataLabels]
        }
    );
}

function populateAverageProcessingCheckboxes2(data) {
    const actualDeliveryContainer = $('#average-processing-actual-delivery-filter2 .checkboxes');
    const typeOfPackagingContainer = $('#average-processing-type-of-packaging-filter2 .checkboxes');
    const truckerContainer = $('#average-processing-trucker-filter2 .checkboxes');
    const portOfDischargeContainer = $('#average-processing-port-of-discharge-filter2 .checkboxes');

    actualDeliveryContainer.empty();
    typeOfPackagingContainer.empty();
    truckerContainer.empty();
    portOfDischargeContainer.empty();


    const actualDeliveries = [...new Set(data.map(x => x.actualDelivery))];
    const typeOfPackagings = [...new Set(data.map(x => x.modeOfShipment))];
    const truckers = [...new Set(data.map(x => x.trucker))];
    const portsOfDischarge = [...new Set(data.map(x => x.portOfDischarge))];

    actualDeliveries.forEach(o => {
        actualDeliveryContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${o}" checked> ${o}</label>`);
    });

    typeOfPackagings.forEach(c => {
        typeOfPackagingContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });

    truckers.forEach(c => {
        truckerContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });

    portsOfDischarge.forEach(c => {
        portOfDischargeContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${c}" checked> ${c}</label>`);
    });
}

//#endregion

//#region Vessel Delay Per Port
async function vesselDelayChart(uploadDate, dataOverride = null) {
    const data = dataOverride || await (async () => {
        const res = await fetch(
            `${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselDelayPerPort?createdDateTime=${uploadDate}`
        );
        return await res.json();
    })();

    // Labels = Port + Mode
    const labels = data.map(x =>
        `${x.portOfDischarge} (${x.modeOfShipment})`
    );

    const delayData = data.map(x => x.delayPct);
    const onTimeData = data.map(x => x.onTimePct);

    if (window.vesselDelayChartInstance)
        window.vesselDelayChartInstance.destroy();

    window.vesselDelayChartInstance = new Chart(
        document.getElementById('vessel-delay-per-port'),
        {
            type: 'bar',
            data: {
                labels,
                datasets: [
                    {
                        label: 'Delay %',
                        data: delayData,
                        backgroundColor: 'hsl(0, 70%, 55%)'
                    },
                    {
                        label: 'On-Time %',
                        data: onTimeData,
                        backgroundColor: 'hsl(120, 60%, 45%)'
                    }
                ]
            },
            options: {
                indexAxis: 'y', // 🔥 THIS makes it horizontal
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'VESSEL DELAY PER PORT',
                        font: { size: 18, weight: 'bold' },
                        color: '#000'
                    },
                    legend: {
                        position: 'right',
                        labels: {
                            color: '#000'
                        }
                    },
                    datalabels: {
                        formatter: v => v === 0 ? '' : `${v}%` ,
                        color: '#000',
                        font: { weight: 'bold' }
                    }
                },
                scales: {
                    x: {
                        stacked: true,
                        max: 100,
                        title: {
                            display: true,
                            text: 'Percentage (%)',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000',
                            callback: function (value) {
                                return value + '%';
                            }
                        },

                    },
                    y: {
                        stacked: true,
                        title: {
                            display: false,
                            text: 'Port + Mode',
                            color: '#000'
                        },
                        ticks: {
                            color: '#000'
                        }
                    }
                }
            },
            plugins: [ChartDataLabels]
        }
    );
}

function populateVesselDelayCheckboxes(data) {
    const portContainer = $('#vessel-delay-per-port-port-of-discharge-filter .checkboxes');
    const modeContainer = $('#vessel-delay-per-port-mode-of-shipment-filter .checkboxes');

    portContainer.empty();
    modeContainer.empty();

    const ports = [...new Set(data.map(x => x.portOfDischarge))];
    const modes = [...new Set(data.map(x => x.modeOfShipment))];

    ports.forEach(o => {
        portContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${o}" checked> ${o}</label>`);
    });


    modes.forEach(m => {
        modeContainer.append(`<label style="margin-right: 10px;"><input type="checkbox" value="${m}" checked> ${m}</label>`);
    });
}

//#endregion

async function refreshAllCharts(uploadDate) {
    const [
        vesselTransitData,
        otdAchievementData,
        averageProcessingData,
        vesselDelayPerPortData
    ] = await Promise.all([
        fetch(`${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselTransitDays?createdDateTime=${uploadDate}`).then(r => r.json()),
        fetch(`${API_BASE_URL}/api/SeaFreightScheduleMonitorings/OtdAchievementRate?createdDateTime=${uploadDate}`).then(r => r.json()),
        fetch(`${API_BASE_URL}/api/SeaFreightScheduleMonitorings/AverageProcessingLeadtimePerForwarder?createdDateTime=${uploadDate}`).then(r => r.json()),
        fetch(`${API_BASE_URL}/api/SeaFreightScheduleMonitorings/VesselDelayPerPort?createdDateTime=${uploadDate}`).then(r => r.json())
    ]);

    vesselTransitChart(uploadDate, vesselTransitData);
    otdAchievementPieChart(uploadDate, [otdAchievementData[0]]);
    otdAchievementPieChart2(uploadDate, [otdAchievementData[1]]);
    averageProcessingChart(uploadDate, averageProcessingData);
    averageProcessingChart2(uploadDate, averageProcessingData);
    vesselDelayChart(uploadDate, vesselDelayPerPortData);
}
