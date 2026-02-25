import apiCall from "./../../helpers/APICall.js";


export async function GetPortStatuses() {
    await LoadPortStatuses();
}

$("#savePortStatus").on("click", async function () {
    let payload = [];

    // 1. Process the actual ports (IDs 1 & 2)
    const targetPorts = [
        { id: 1, name: "Manila North (MICT)" },
        { id: 2, name: "Batangas (POB)" }
    ];

    targetPorts.forEach(port => {
        payload.push({
            Id: port.id,
            Name: port.name,
            PortUtilizationStatus: getStatusFromTable(port.name, "PortUtilizationStatus"),
            BerthingStatus: getStatusFromTable(port.name, "BerthingStatus")
        });
    });

    // Aiko's helper to read your global checkboxes
    function getGlobalStatus(checkboxClass) {
        let status = "Unknown";
        let $checked = $(`.${checkboxClass}:checked`);
        if ($checked.length > 0) {
            status = $checked.next('label').text().trim();
        }
        return status;
    }

    // 2. Process the fake ports (IDs 3 & 4)
    // Shoving the global statuses into the 'PortUtilizationStatus' property
    payload.push({
        Id: 3,
        Name: "Vessel Updates",
        PortUtilizationStatus: getGlobalStatus("vessel_Updates"),
        BerthingStatus: "Normal" // Dummy data because the column exists
    });

    payload.push({
        Id: 4,
        Name: "Import Delivery",
        PortUtilizationStatus: getGlobalStatus("importDelivery_Updates"),
        BerthingStatus: "Normal" // Dummy data
    });

    console.table(payload);
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
            // Check if it's a REAL port (IDs 1 & 2)
            if (port.id === 1 || port.id === 2) {
                setCheckboxState(port.name, "PortUtilizationStatus", port.portUtilizationStatus);
                setCheckboxState(port.name, "BerthingStatus", port.berthingStatus);
            }
            // Intercept your "Dummy Data" ports
            else if (port.id === 3) {
                // ID 3 is Vessel Updates. The data is hiding in PortUtilizationStatus.
                setGlobalCheckboxState("vessel_Updates", port.portUtilizationStatus);
            }
            else if (port.id === 4) {
                // ID 4 is Import Delivery.
                setGlobalCheckboxState("importDelivery_Updates", port.portUtilizationStatus);
            }
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

/**
 * Aiko's helper to set and trigger your global column checkboxes.
 * @param {string} checkboxClass - e.g., "vessel_Updates"
 * @param {string} targetValue - e.g., "Critical" (The value coming from DB)
 */
function setGlobalCheckboxState(checkboxClass, targetValue) {
    // 1. Uncheck ALL checkboxes with this class first
    $(`.${checkboxClass}`).prop('checked', false);

    // 2. Loop through them to find the one matching the database value
    $(`.${checkboxClass}`).each(function () {
        let labelText = $(this).next('label').text().trim();

        if (labelText === targetValue) {
            // Check it and trigger the change event to fire your filters!
            $(this).prop('checked', true).trigger('change');
            return false; // Break the loop once found
        }
    });
}
// Aiko's helper for your global columns
function getGlobalStatusByClass(checkboxClass) {
    let status = "Unknown";
    // Find the checked checkbox with this class and grab its label's text
    let $checkedBox = $(`.${checkboxClass}:checked`);
    if ($checkedBox.length > 0) {
        status = $checkedBox.next('label').text().trim();
    }
    return status;
}