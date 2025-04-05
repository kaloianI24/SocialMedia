// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('img').on('dragstart contextmenu', function (e) {
        e.preventDefault();
        return false;
    });
});