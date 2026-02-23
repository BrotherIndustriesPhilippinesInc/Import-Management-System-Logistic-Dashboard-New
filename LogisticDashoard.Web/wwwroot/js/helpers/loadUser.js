
//Authorization
$(async function () {


    let user = JSON.parse(localStorage.getItem("user"));
    let username = user["fullName"];
    let department = user["emsDepartment"];
    let section = user["section"];

    let userInfo;

    $("#userName").text(username);
    $("#userDepartment").text(department);
    $("#userSection").text(section); 


    await fetch(`http://apbiphbpswb01:1117/api/Users/GetPortalUserCredentials?employeeNumber=${user["employeeNumber"]}`, { method: "GET" })
        .then((response) => response.json()).then((data) => { userInfo = data; });


    if (userInfo["isAdmin"]) {

        // 1. Wrap YOUR exact code in a reusable function
        function lockEverythingDown() {
            //HOME
            $("#savePortStatus").removeClass("d-flex").addClass("d-none");
            $("#announcement-header").removeClass("d-flex").addClass("d-none");

            $(".edit-vessel-status").removeClass("d-flex").addClass("d-none");
            $(".delete-vessel-status").removeClass("d-flex").addClass("d-none");

            //SEA FREIGHT
            $(".sea-freight-controls").removeClass("d-flex").addClass("d-none");

            //AIR FREIGHT
            $(".air-freight-controls").removeClass("d-flex").addClass("d-none");

            //Sailing Schedule
            $(".sailing-schedule").removeClass("d-flex").addClass("d-none");

            // DataTables specific lockdown (Targets the main table AND the sticky clones)
            $("#sailing-schedule-table, .DTFC_Cloned").find("tbody td").attr("contenteditable", "false");
            $("#sailing-schedule-table, .DTFC_Cloned").find("tbody :input").prop("disabled", true);

            //Port utilization
            $("#addPort").removeClass("d-flex").addClass("d-none");

            $("#portListA-table tbody td").attr("contenteditable", "false");
            $("#portListA-table tbody :input").prop("disabled", true);

            $("#portListB-table tbody td").attr("contenteditable", "false");
            $("#portListB-table tbody :input").prop("disabled", true);

            //Berthing Status
            $("#addBerth").removeClass("d-flex").addClass("d-none");

            $("#berthListA-table tbody td").attr("contenteditable", "false");
            $("#berthListA-table tbody :input").prop("disabled", true);

            $("#berthListB-table tbody td").attr("contenteditable", "false");
            $("#berthListB-table tbody :input").prop("disabled", true);

            //Delivery Leadtime
            //Got included with sailing schedule

            //Incoterms
            //disable select
            $(".incoterms-select").prop("disabled", true);

            //Mode of shipment
            //Got included with sea freight

            //Vessel route maps
            $("#vesselRouteMapCreate").removeClass("d-flex").addClass("d-none");
            $(".vesselRouteMapControl").removeClass("d-flex").addClass("d-none");
            $(".vessel-route-edit").removeClass("d-flex").addClass("d-none");

            //PH Port Map
            $("#phportmap-create").removeClass("d-flex").addClass("d-none");
            $(".phPortMap-edit").removeClass("d-flex").addClass("d-none");

            //Courier Information
            $(".instructions-create").removeClass("d-flex").addClass("d-none");
            $(".carrierInformation-control").removeClass("d-flex").addClass("d-none");
            $(".dhl-control").removeClass("d-flex").addClass("d-none");
            $(".instructions-control").removeClass("d-flex").addClass("d-none");

            //Logistic cost
            //Got included with sea freight

            //Philippine Holidays
            $(".phHoliday-control ").removeClass("d-flex").addClass("d-none");

            //Administrators
            $(".user-control").removeClass("d-flex").addClass("d-none");
            

        }

        // 2. Run it immediately (covers standard F5 reloads)
        lockEverythingDown();

        // 3. The DataTables Hook: Run it every time a table finishes loading, sorting, or paginating!
        $(document).on('draw.dt', function () {
            lockEverythingDown();
        });

        // 4. The Ultimate Hammer: Just to be absolutely sure your buttons hide on navigation
        setInterval(lockEverythingDown, 500);
    }
});
        