import apiCall from "./../../helpers/APICall.js";


export async function GetPortStatuses() {
    await LoadPortStatuses();
}

$("#savePortStatus").on("click", async function () {
    // 1. Define the ports we expect to scrape. 
    // Ideally, this comes from a config or the server, but we'll map IDs manually here based on your requirement.
    const targetPorts = [
        { id: 1, name: "Manila North (MICT)" },
        { id: 2, name: "Batangas (POB)" }
    ];

    let payload = [];

    // 2. Iterate through each port and find its status in the table
    targetPorts.forEach(port => {
        let portData = {
            Id: port.id,
            Name: port.name,
            // Get the status text (e.g., "Normal") for this port + type combo
            PortUtilizationStatus: getStatusFromTable(port.name, "PortUtilizationStatus"),
            BerthingStatus: getStatusFromTable(port.name, "BerthingStatus")
        };
        payload.push(portData);
    });

    console.table(payload); // Debugging: Check the console to see the clean object

    // 3. Send to Backend
    await UpdatePortStatuses(payload);
});

// Helper function to find the checked status based on hidden input values
function getStatusFromTable(portName, statusType) {
    // logic: 
    // 1. Find all inputs with value = portName (e.g., "Manila North (MICT)")
    // 2. Filter to find the one that shares a parent <td> with an input value = statusType
    // 3. In that same <td>, look for a CHECKED checkbox
    // 4. If checked, get the text of the <label> next to it.

    // Select all hidden inputs matching the port name
    let $portInputs = $(`input[type='hidden'][value='${portName}']`);

    let status = "Unknown"; // Default if nothing is checked

    $portInputs.each(function () {
        let $parentTd = $(this).closest('td');

        // Check if this TD also concerns the correct Status Type (e.g., PortUtilizationStatus)
        let isCorrectType = $parentTd.find(`input[type='hidden'][value='${statusType}']`).length > 0;

        if (isCorrectType) {
            // Check if the checkbox in this cell is checked
            let $checkbox = $parentTd.find("input[type='checkbox']");
            if ($checkbox.is(":checked")) {
                // Get the label text (Normal/Critical/Serious)
                status = $parentTd.find("label").text().trim();
                return false; // Break the loop once found
            }
        }
    });

    return status;
}

async function UpdatePortStatuses(payload) {
    try {
        // Ensure your headers and stringify are correct
        let response = await fetch(`${API_BASE_URL}/api/ImportPortStatus`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error("Network response was not ok");

        swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Port Statuses Updated Successfully',
            showConfirmButton: false,
            timer: 1500
        });

    } catch (error) {
        console.error("Failed to update:", error);
    }
}
$(".berthStatus, .portUtil").on("click", async function () {
    let data = [];

    //get this element and get all input values
    let inputs = this.parentElement.querySelectorAll("input");

    for (let i = 0; i < inputs.length; i++) {
        //check if checkbox for true or false
        if (inputs[i].type == "checkbox") {
            data.push(inputs[i].checked);
            continue;
        }
        data.push(inputs[i].value);
    }
});

async function LoadPortStatuses() {
    try {
        let response = await fetch(`${API_BASE_URL}/api/ImportPortStatus`);

        if (!response.ok) throw new Error("Failed to fetch port statuses");

        let data = await response.json();

        // Iterate through the data from the DB
        data.forEach(port => {
            // Update Port Utilization Column
            setCheckboxState(port.name, "PortUtilizationStatus", port.portUtilizationStatus);
            //Trigger change event
            

            // Update Berthing Status Column
            setCheckboxState(port.name, "BerthingStatus", port.berthingStatus);
            //Trigger change event
            
        });

    } catch (error) {
        console.error("Error loading statuses:", error);
    }
}

/**
 * Finds the correct checkbox in your table grid and checks it.
 * @param {string} portName - e.g., "Manila North (MICT)"
 * @param {string} statusType - e.g., "PortUtilizationStatus"
 * @param {string} targetValue - e.g., "Critical" (The value coming from DB)
 */
function setCheckboxState(portName, statusType, targetValue) {
    let $portInputs = $(`input[type='hidden'][value='${portName}']`);

    // 2. Clear ALL checkboxes for this port/type first
    $portInputs.each(function () {
        let $cell = $(this).closest('td');
        if ($cell.find(`input[type='hidden'][value='${statusType}']`).length > 0) {
            let $cb = $cell.find("input[type='checkbox']");
            // Only trigger if it was actually checked to avoid unnecessary events
            if ($cb.is(":checked")) {
                $cb.prop('checked', false).trigger("change");
            }
        }
    });

    // 3. Find the SPECIFIC cell and Check it
    $portInputs.each(function () {
        let $cell = $(this).closest('td');
        let isCorrectType = $cell.find(`input[type='hidden'][value='${statusType}']`).length > 0;

        if (isCorrectType) {
            let labelText = $cell.find("label").text().trim();

            if (labelText === targetValue) {
                // HERE IS WHAT YOU MISSED:
                $cell.find("input[type='checkbox']")
                    .prop('checked', true)
                    .trigger("change"); // <--- You need to yell "I CHANGED!"

                return false;
            }
        }
    });
}