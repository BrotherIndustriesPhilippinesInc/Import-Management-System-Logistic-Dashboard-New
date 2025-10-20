export function startEndDateSearch(table, startDateInput, endDateInput, indexedDateColumns) {
    // Remove old filters for this table
    $.fn.dataTable.ext.search = $.fn.dataTable.ext.search.filter(
        fn => fn._tableId !== table.table().node().id
    );

    const filterFn = function (settings, data) {
        if (settings.nTable.id !== table.table().node().id) return true;

        const min = startDateInput ? new Date(startDateInput) : null;
        const max = endDateInput ? new Date(endDateInput) : null;

        // ✅ If both are empty → show all
        if (!min && !max) return true;

        const columns = Array.isArray(indexedDateColumns) ? indexedDateColumns : [indexedDateColumns];

        for (let colIndex of columns) {
            let dateStr = data[colIndex];
            if (!dateStr) continue;

            // 🔹 Normalize: allow formats like "Sep 22", "Sept-22", "2025-09-22T00:00:00Z"
            dateStr = dateStr.replace(/-/g, " "); // handle Sept-22 → "Sept 22"
            const date = new Date(dateStr);

            if (!isNaN(date)) {
                if ((!min || date >= min) && (!max || date <= max)) {
                    return true; // ✅ at least one match
                }
            }
        }
        return false; // ❌ nothing matched
    };

    filterFn._tableId = table.table().node().id;
    $.fn.dataTable.ext.search.push(filterFn);

    table.draw(); // ✅ trigger redraw
}


export function chartInit(ctx, data) {

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

export function lineChartPortUtilizationInit(ctx, isPercentage, firstlabel, secondlabel) {
    return new Chart(ctx, {
        type: 'line',
        data: {
            labels: [], // empty at start
            datasets: [
                {
                    label: firstlabel,
                    data: [],
                    borderColor: 'rgb(0, 0, 204)',
                    borderWidth: 4,
                    pointBorderColor: 'rgb(0, 0, 255)',
                    pointBackgroundColor: 'rgb(255, 0, 0)',
                    pointBorderWidth: 2,
                    pointRadius: 5,
                    pointStyle: 'circle',
                    rotation: 45,
                },
                {
                    label: secondlabel,
                    data: [],
                    borderColor: 'rgb(112, 48, 160)',
                    borderWidth: 4,
                    pointBorderColor: 'rgb(112, 48, 160)',
                    pointBackgroundColor: 'rgb(255, 0, 0)',
                    pointBorderWidth: 2,
                    pointRadius: 5,
                    pointStyle: 'circle',
                    rotation: 45,
                }
            ]
        },
        options: {
            layout: { padding: { left: 20, right: 20, top: 20, bottom: 20 } },
            scales: {
                x: { ticks: { color: 'black', font: { size: 12, weight: 'bold' }, }, grid: { display: false }, offset: true },
                y: { ticks: { color: 'black', font: { size: 12, weight: 'bold' }, callback: (value) => value + (isPercentage ? "%" : "") }, grid: { display: true }, beginAtZero: true }
            },
            plugins: {
                datalabels: {
                    anchor: 'start',
                    align: 'top',
                    color: '#000',
                    font: { weight: 'bold' },
                    formatter: (value) => value + (isPercentage ? "%" : "")
                },
                title: {
                    display: true,
                    text: ['', ''], // empty for now
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

export function mapSchedules(schedules) {
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

export function updateChartsFromTables(tables, charts) {
    // if only single table+chart passed, wrap into arrays
    if (!Array.isArray(tables)) tables = [tables];
    if (!Array.isArray(charts)) charts = [charts];

    tables.forEach((table, i) => {
        const chart = charts[i];
        if (!chart) return;

        const filteredData = table.rows({ filter: 'applied' }).data().toArray();
        const { weeks, transitDays } = mapSchedules(filteredData);

        chart.data.labels = weeks;
        chart.data.datasets[0].data = transitDays;
        chart.options.plugins.title.text = [
            $(`#routeName${i}`).text(),
            'Transit Days'
        ];
        chart.update();
    });
}

export async function getData(endpoint) {
    const url = `${API_BASE_URL}/api/${endpoint}`;
    const response = await fetch(url);
    const data = await response.json();

    return data;
}

export async function postData(endpoint, body) {
    const url = `${API_BASE_URL}/api/${endpoint}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
    });
    const data = await response.json();

    return data; 
}