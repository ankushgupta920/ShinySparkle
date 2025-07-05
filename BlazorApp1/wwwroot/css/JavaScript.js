document.addEventListener("DOMContentLoaded", function () {
    var dropdowns = document.querySelectorAll(".dropdown-toggle");
    dropdowns.forEach(function (dropdown) {
        new bootstrap.Dropdown(dropdown);
    });
});

