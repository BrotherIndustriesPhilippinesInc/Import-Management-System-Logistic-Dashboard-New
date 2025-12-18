$(async function () {
    let uploadDate = await getUploadDateAndTime();

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
            data: {
                labels: ['Achieved', 'Not Achieved'],
                datasets: [{
                    data: [achieved, notAchieved],
                    backgroundColor: [
                        'hsl(140, 65%, 50%)',
                        'hsl(0, 70%, 55%)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: `OTD ACHIEVEMENT – ${trucker.trucker}`,
                        font: { size: 18, weight: 'bold' }
                    },
                    legend: { position: 'bottom' },
                    datalabels: {
                        formatter: v => `${v}%`,
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
