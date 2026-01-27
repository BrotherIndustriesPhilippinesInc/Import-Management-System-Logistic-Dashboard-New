export async function CriteriaControls() {
    $(".main-checkbox").on("change", function () {
        let $cell = $(this).closest("td"); // use closest(), it's faster/safer than parents()
        let text = $cell.find("label").text().trim(); // Get text from LABEL, not the whole TD

        // Define the classes we want to manage
        const colorClasses = "normal-background critical-background serious-background";

        // Always remove old color classes first so they don't stack
        $cell.removeClass(colorClasses);

        if ($(this).is(":checked")) {
            if (text === "Normal") {
                $cell.addClass("normal-background");
            } else if (text === "Critical") {
                $cell.addClass("critical-background");
            } else if (text === "Serious") {
                $cell.addClass("serious-background");
            }
        }
        // No need for an 'else' to add 'filter-inputs' back 
        // because we didn't nuke the existing layout classes!
    });

    checkboxEvent("vessel_Updates");
    checkboxEvent("importDelivery_Updates");
    checkboxEvent("manilaPortUtilization_Updates");
    checkboxEvent("batangasPortUtilization_Updates");
    checkboxEvent("manilaBerthStatus_Updates");
    checkboxEvent("batangasBerthStatus_Updates");
}

function checkboxEvent(className) {
    $("." + className + "").on("click", function () {
        let checkboxes = $("." + className + "");
        let currentCheckbox = $(this);
        //Uncheck all checkboxes except this one
        checkboxes.each(function () {
            if ($(this).attr("id") != currentCheckbox.attr("id")) {
                $(this).prop("checked", false);
                //trigger change event
                $(this).trigger("change");
            }
        });

        datatablesCriteriaControls(className);

    });
}

function datatablesCriteriaControls(className) {
    switch (className) {
        case "vessel_Updates":

            datatablesDataSwitch(className, "#vessel-status-table");
            break;
        case "importDelivery_Updates":
            datatablesDataSwitch(className, "#import-delivery-table");
            break;
        case "manilaPortUtilization_Updates":
            datatablesDataSwitch(className, "#port-utilization-manila-table");
            break;
        case "batangasPortUtilization_Updates":
            datatablesDataSwitch(className, "#port-utilization-batangas-table");
            break;
        case "manilaBerthStatus_Updates":
            datatablesDataSwitch(className, "#berthing-status-manila-table");
            break;
        case "batangasBerthStatus_Updates":
            datatablesDataSwitch(className, "#berthing-status-batangas-table");
            break;
    }
}

function datatablesDataSwitch(className, dataTablesId) {
    let criteria = $("." + className + ":checked").parents("td").text();

    if (criteria.toLowerCase() == "normal") {

        //on datatables only show rows with criteria of normal
        $(dataTablesId).DataTable().column(7).search("normal").draw();

    } else if (criteria.toLowerCase() == "critical") {
        $(dataTablesId).DataTable().column(7).search("critical").draw();

    } else if (criteria.toLowerCase() == "serious") {
        $(dataTablesId).DataTable().column(7).search("serious").draw();

    } else {
        $(dataTablesId).DataTable().column(7).search("").draw();
    }
}