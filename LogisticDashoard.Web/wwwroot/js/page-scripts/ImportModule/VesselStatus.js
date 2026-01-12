export async function VesselStatus(){
    let data = await fetch(`${API_BASE_URL}/api/ImportDashboards/`);
    let response = await data.json();
    let filteredData = [];

    const vessel_status_table = $('#vessel-status-table').DataTable({
        layout: {
            topStart: {
                buttons: ['colvis', {
                    text: 'Create New',
                    className: 'create-new-button btn btn-primary',
                    action: function (e, dt, node, config) {
                        $('#testModal').modal('show');
                    }
                }]
            },
            topEnd: ['search', 'pageLength']
        },
        data: response,

        order: [[0, "desc"]],
        columnDefs: [
            { className: "p-1", target: "_all" }
        ],
        columns: [
            // SHIPMENT DETAILS
            { data: 'id', visible: false },
            { data: 'bl' },
            { data: 'shipper' },
            {
                data: 'original_ETA',
                render: function (data, type) {
                    if (!data) return '';

                    if (type === 'display') {
                        const d = new Date(data);
                        return new Intl.DateTimeFormat('en-GB', {
                            day: '2-digit',
                            month: 'short'
                        }).format(d);
                    }

                    // sort, filter, type → return ORIGINAL value
                    return data;
                }
            },
            {
                data: 'latest_ETA',
                render: function (data, type) {
                    if (!data) return '';

                    if (type === 'display') {
                        const d = new Date(data);
                        return new Intl.DateTimeFormat('en-GB', {
                            day: '2-digit',
                            month: 'short'
                        }).format(d);
                    }

                    return data;
                }
            },
            { data: 'vessel_Remarks' },
            { data: 'vessel_Status_BIPH_Action' },
            //ACTIONS 
            {
                data: null, orderable: false, searchable: false, render: function (data, type, row) {
                    return `<button class="btn btn-primary edit-vessel-status" data-id="${row.id}">Edit</button>
                                    <button class="btn btn-danger delete-vessel-status" data-id="${row.id}">Delete</button>`;
                }
            }

        ]});
}

export async function VesselStatusUpdate() {

}