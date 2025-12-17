$(function () {
    let datatable = new DataTable("#delivery-leadtime");
    let leadTimeName = "";

    // Make table cells editable except header & action column
    $('#delivery-leadtime tbody').on('click', 'td', function () {
        let $cell = $(this);
        let columnIndex = $cell.index();

        // ❌ Skip last column (actions)
        let totalColumns = $cell.closest('tr').find('td').length;
        if (columnIndex === totalColumns - 1) return;

        // Skip if already editing
        if ($cell.find('input').length > 0) return;

        let original = $cell.text().trim();
        let $input = $('<input type="text" class="form-control form-control-sm"/>')
            .val(original);

        $cell.html($input);
        $input.focus();

        $input.on('blur', function () {
            let newValue = $(this).val().trim();
            $cell.text(newValue);

            if (newValue !== original) {
                let id = $cell.closest('tr').data('id');
                let row = $cell.closest('tr');
                let carrier = row.find('td:eq(0)').text();
                let origin = row.find('td:eq(1)').text();
                let dest = row.find('td:eq(2)').text();

                // Determine which column was changed
                let field;
                switch (columnIndex) {
                    case 0: field = "Carrier"; break;
                    case 1: field = "OriginPort"; break;
                    case 2: field = "DestinationPort"; break;
                    case 3: field = "VesselTransitLeadtime"; break;
                    case 4: field = "CustomsClearanceLeadtime"; break;
                    case 5: field = "TotalLeadtime"; break;
                }

                // 🔥 Auto-save instantly
                $.ajax({
                    url: `${API_BASE_URL}/api/DeliveryLeadtimes/UpdateCell`,
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        id: id,
                        carrier: carrier,
                        origin: origin,
                        destination: dest,
                        field: field,
                        value: newValue
                    }),
                    success: function () {
                        console.log('✅ Saved successfully!');
                    },
                    error: function (xhr) {
                        console.error('❌ Error saving data:', xhr.responseText);
                        alert('Error saving data! Check console for details.');
                    }
                });
            }
        });
    });

    $("#saveLeadtime").click(function () {
        let carrier = $("#carrier").val();
        let origin = $("#origin_port").val();
        let destination = $("#destination_port").val();
        let vessel_transit_leadtime = $("#vessel_transit_leadtime").val();
        let customs_clearance_leadtime = $("#customs_clearance_leadtime").val();
        let total_leadtime = $("#total_leadtime").val();

        $.ajax({
            url: `${API_BASE_URL}/api/DeliveryLeadtimes`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                Carrier: carrier,
                OriginPort: origin,
                DestinationPort: destination,
                VesselTransitLeadtime: vessel_transit_leadtime,
                CustomsClearanceLeadtime: customs_clearance_leadtime,
                TotalLeadtime: total_leadtime
            }),
            success: function () {
                console.log('✅ Saved successfully!');
                location.reload();
            },
            error: function (xhr) {
                console.error('❌ Error saving data:', xhr.responseText);
                swal.fire('Error', 'Error saving data! Check console for details.', 'error');
            }
        });
    });

    $(".delete-btn").click(function () {
        let id = $(this).data('id');
        $.ajax({
            url: `${API_BASE_URL}/api/DeliveryLeadtimes/Delete/${id}`,
            type: 'POST',
            success: function () {
                console.log('✅ Deleted successfully!');
                location.reload();
            },
            error: function (xhr) {
                console.error('❌ Error deleting data:', xhr.responseText);
                swal.fire('Error', 'Error deleting data! Check console for details.', 'error');
            }
        });
    });

    $('#uploadInput').on('change', function () {
        const file = this.files[0];

        if (!file) {
            console.warn('No file selected.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: `${API_BASE_URL}/api/DeliveryLeadtimeDatas/Upload`, // 🔥 Replace with your actual API route
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

    $(".leadTime-btn").on("click", async function () {
        leadTimeName = $(this).data("id");
        let response = await fetch(`${API_BASE_URL}/api/DeliveryLeadtimeDatas/GetChartData?leadTimeName=${leadTimeName}`);
        const data = await response.json();

        createLeadtimeChart(data);
    });

    let leadtimeChart = null;

    function createLeadtimeChart(data) {
        const ctx = document.getElementById('leadtimeChartCanvas').getContext('2d');

        if (leadtimeChart) {
            leadtimeChart.destroy();
        }

        data.datasets["noOfBL"] = data.datasets["noOfBL"].map(v => Math.round(v));
        data.datasets["actualAve"] = data.datasets["actualAve"].map(v => Math.round(v));
        data.datasets["targetMax"] = data.datasets["targetMax"].map(v => Math.round(v));
        data.datasets["targetMin"] = data.datasets["targetMin"].map(v => Math.round(v));

        data.datasets["noOfBL"] = data.datasets["noOfBL"].map(v => v === 0 ? null : v);
        data.datasets["actualAve"] = data.datasets["actualAve"].map(v => v === 0 ? null : v);

        leadtimeChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: data.labels,

                datasets: [
                    {
                        label: 'No. of BL',
                        data: data.datasets["noOfBL"],
                        backgroundColor: 'rgba(0, 178, 80, 1)',
                        borderColor: 'green',
                        borderWidth: 1,
                        yAxisID: 'y',
                        barThickness: 40, // fixed bar width
                        order: 2,
                    },
                    {
                        label: 'Actual (Ave)',
                        data: data.datasets["actualAve"],
                        type: 'line',
                        borderColor: 'blue',
                        backgroundColor: 'blue',
                        tension: 0,
                        borderWidth: 5,
                        yAxisID: 'y',
                        pointRadius: 5,

                    },
                    {
                        label: 'Target (Max)',
                        data: data.datasets["targetMax"],
                        type: 'line',
                        borderColor: 'red',
                        borderWidth: 5,
                        //borderDash: [5, 5],
                        yAxisID: 'y',
                        pointRadius: 0,
                        datalabels: { display: false}
                    },
                    {
                        label: 'Target (Min)',
                        data: data.datasets["targetMin"],
                        type: 'line',
                        borderColor: 'red',
                        borderWidth: 5,
                        //borderDash: [5, 5],
                        yAxisID: 'y',
                        pointRadius: 0,
                        datalabels: { display: false }
                    }
                ]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        suggestedMax: 20, // gives room above 14
                        ticks: {
                            stepSize: 2,
                        },
                        title: {
                            display: true,
                            text: 'Days / Count'
                        }
                    }
                },
                plugins: {
                    legend: {
                        position: 'bottom'
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false
                    },
                    datalabels: {
                        anchor: 'start',
                        align: 'top',
                        color: '#000',
                        font: { weight: 'bold' },
                        formatter: (value) => value //Math.round(value)
                    },
                }
            },
            plugins: [ChartDataLabels]
        });
    }

    /**
     * updateChart(newData)
     * newData: { labels, datasets }
     */
     function updateChart(newData = {}) {
        if (!leadtimeChart) {
            return createChart({ labels: newData.labels, datasets: newData.datasets });
        }

        if (newData.labels) leadtimeChart.data.labels = newData.labels;
        if (newData.datasets) leadtimeChart.data.datasets = newData.datasets;

        leadtimeChart.update();
    }

    /**
     * destroyChart()
     */
     function destroyChart() {
        if (leadtimeChart) {
            leadtimeChart.destroy();
            leadtimeChart = null;
        }
    }

});