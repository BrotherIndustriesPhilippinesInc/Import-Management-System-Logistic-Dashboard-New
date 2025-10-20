
let user = JSON.parse(localStorage.getItem("user"));
let username = user["Full_Name"];
let department = user["Department"];
let section = user["Section"];

$("#userName").text(username);
$("#userDepartment").text(department);
$("#userSection").text(section);