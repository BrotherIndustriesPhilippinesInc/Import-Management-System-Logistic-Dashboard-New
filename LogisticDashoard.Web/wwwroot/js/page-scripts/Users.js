$(function () {
    // 1. You MUST add the 'async' keyword here
    $("#btnSave").on("click", async function (e) {
        e.preventDefault(); // Stop the form from submitting normally

        let biphId = $("#biphId").val();

        // 2. ERROR: .val() on a checkbox returns "on", not true/false.
        // You need .is(':checked') or .prop('checked')
        let isAdmin = $("#isAdmin").is(":checked");

        // 3. Since you are using await, try to use try/catch blocks instead of .then() chains. 
        // It's cleaner.
        try {
            // Also, why are you sending params in the URL for a POST? 
            // Put them in the body if you can. But I'll stick to your URL style for now.
            const response = await fetch(`${API_BASE_URL}/api/Users?employeeNumber=${biphId}&isAdmin=${isAdmin}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                // You were sending just the ID in the body? That's redundant if it's in the URL.
                // sending empty body for now if params are in query string
                body: JSON.stringify({})
            });

            const data = await response.json();

            if (response.ok) { // Check response.ok (status 200-299)
                Swal.fire({
                    icon: "success",
                    title: "User created!",
                    text: "User created successfully",
                    showConfirmButton: false,
                    timer: 1000
                }).then(() => {
                    // go back to list
                    window.location.href = "/Users";
                });

            } else {
                Swal.fire({
                    icon: "error",
                    title: "Creation failed!",
                    text: data.message || "Something went wrong", // Show server error if available
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        } catch (error) {
            console.error(error);
            Swal.fire({
                icon: "error",
                title: "Network Error",
                text: "Could not reach the server.",
            });
        }
    });

    $("#btnDelete").on("click", async function (e) {
        e.preventDefault(); // Stop the form from submitting normally

        //Get from url parameter employeeNumber
        let biphId = getUrlParameter('employeeNumber');

        // 3. Since you are using await, try to use try/catch blocks instead of .then() chains.
        // It's cleaner.
        try {
            // Also, why are you sending params in the URL for a POST?
            // Put them in the body if you can. But I'll stick to your URL style for now.
            const response = await fetch(`${API_BASE_URL}/api/Users/Delete?employeeNumber=${biphId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                // You were sending just the ID in the body? That's redundant if it's in the URL.
                // sending empty body for now if params are in query string
                body: JSON.stringify({})
            });

            const data = await response.json();

            if (response.ok) { // Check response.ok (status 200-299)
                Swal.fire({
                    icon: "success",
                    title: "Delete successful!",
                    text: "User deleted successfully",
                    showConfirmButton: false,
                    timer: 1000
                });

                setTimeout(function () {
                    window.location.href = "/Users";
                }, 1000);
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Delete failed!",
                    text: data.message || "Something went wrong", // Show server error if available
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        } catch (error) {
            console.error(error);
            Swal.fire({
                icon: "error",
                title: "Network Error",
                text: "Could not reach the server.",
            });
        }
    });

    // Open modal when Is Admin icon/button is clicked
    $(document).on("click", ".admin-toggle-btn", function () {
        // Extra guard - block kahit ma-late ang disable ng loadUser.js
        if (window.isCurrentUserAdmin === false) {
            return;
        }

        const id = $(this).data("id");            // local Users.Id
        const portalId = $(this).data("portal-id"); // PortalId
        const fullName = $(this).data("full-name");
        const isAdmin = $(this).data("is-admin");

        $("#adminModalUserName").text(fullName);
        $("#adminModalId").val(id);
        $("#adminModalPortalId").val(portalId);
        $("#adminStatusSelect").val(isAdmin.toString());

        const adminModal = new bootstrap.Modal(document.getElementById("adminModal"));
        adminModal.show();
    });

    // Save the new admin status
    $(document).on("click", "#btnSaveAdminStatus", async function () {
        const id = $("#adminModalId").val();
        const portalId = $("#adminModalPortalId").val();
        const newIsAdmin = $("#adminStatusSelect").val() === "true";

        const bodyPayload = id
            ? { id: parseInt(id), portalId: parseInt(portalId), isAdmin: newIsAdmin }
            : { portalId: parseInt(portalId), isAdmin: newIsAdmin };

        try {
            let response;

            if (id) {
                // May local record na -> UPDATE (POST, hindi PUT, para maiwasan ang WebDAV block)
                response = await fetch(`${API_BASE_URL}/api/Users/UpdateAdmin`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        id: parseInt(id),
                        portalId: parseInt(portalId),
                        isAdmin: newIsAdmin
                    })
                });
            } else {
                // Walang local record pa -> AUTO-CREATE (same as before)
                response = await fetch(`${API_BASE_URL}/api/Users/CreateLocalRecord`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        portalId: parseInt(portalId),
                        isAdmin: newIsAdmin
                    })
                });
            }

            bootstrap.Modal.getInstance(document.getElementById("adminModal")).hide();

            if (response.ok) {
                Swal.fire({
                    icon: "success",
                    title: "Authority updated!",
                    text: "User authority updated successfully",
                    showConfirmButton: false,
                    timer: 1000
                }).then(() => {
                    $("#users-table").DataTable().ajax.reload(null, false);
                });
            } else {
                const data = await response.json().catch(() => ({}));
                Swal.fire({
                    icon: "error",
                    title: "Update failed!",
                    text: data.message || "Something went wrong",
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        } catch (error) {
            console.error(error);
            Swal.fire({
                icon: "error",
                title: "Network Error",
                text: "Could not reach the server.",
            });
        }
    });

    // Source - https://stackoverflow.com/a
    // Posted by Mujammil H Kazi, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-01-22, License - CC BY-SA 4.0

    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
        return false;
    };

});