// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var date = new Date();
var currentYear = date.getFullYear();
var currentYearLabel = document.getElementById("CurrentYear");
currentYearLabel.innerText=currentYear;