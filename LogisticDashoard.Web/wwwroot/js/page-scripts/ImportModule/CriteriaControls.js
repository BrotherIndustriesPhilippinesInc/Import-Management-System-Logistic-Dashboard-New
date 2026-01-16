export async function CriteriaControls() {
    $(".main-checkbox").on("change", function () {
        if ($(this).is(":checked")) {
            //console.log($(this).parents("td").text());
            if ($(this).parents("td").text() == "Normal") {
                $(this).parents("td").addClass("normal-background");
            } else if ($(this).parents("td").text() == "Critical") {
                $(this).parents("td").addClass("critical-background");
            } else if ($(this).parents("td").text() == "Serious") {
                $(this).parents("td").addClass("serious-background");
            }
        } else {
            $(this).parents("td").removeClass();
            $(this).parents("td").addClass("filter-inputs text-start");
        }
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
            //datatablesDataSwitch(className, "#vessel-status-table");
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