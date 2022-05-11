async function getUsageAnalysisObject() {
    //HTTP Get Request
    await fetch('http://localhost:49202/api/GetUsageAnalysisObject?' + new URLSearchParams({
        token: localStorage.getItem('JWT')
    })).then(async response => localStorage.setItem('usageAnalysisObject', JSON.stringify(await response.json())))

}

var jsonData = localStorage.getItem('usageAnalysisObject');
const jsonConst = JSON.parse(jsonData)
console.log(jsonConst)

//Generate Login Trend
const login = document.getElementById("loginTrend").getContext("2d");
console.log(login);
let loginTrend = new Chart(login, {
    type: 'line',
    data: {
        labels: Object.keys(jsonConst.logins),
        datasets: [{
            label: 'Logins',
            data: jsonConst.logins,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});


//Generate Signup Trend
const signup = document.getElementById("signupTrend").getContext("2d");
console.log(signup);
let signupTrend = new Chart(signup, {
    type: 'line',
    data: {
        labels: Object.keys(jsonConst.signups),
        datasets: [{
            label: 'Signups',
            data: jsonConst.signups,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

//Generate Most Viewed Pages
const mostView = document.getElementById("mostViewed").getContext("2d");
console.log(mostView);
let mostViewed = new Chart(mostView, {
    type: 'bar',
    data: {
        labels: Object.keys(jsonConst.mostViewedPages),
        datasets: [{
            label: 'Viewed Page',
            data: jsonConst.mostViewedPages,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

//Generate Highest Average Page Duration
const highAveragePage = document.getElementById("averageDuration").getContext("2d");
console.log(highAveragePage);
let averageDuration = new Chart(highAveragePage, {
    type: 'bar',
    data: {
        labels: Object.keys(jsonConst.highestAverageDurationPages),
        datasets: [{
            label: 'Average Duration in a page',
            data: jsonConst.highestAverageDurationPages,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

//Generate Most Scanned Barcodes
const mostScannedBarcodes = document.getElementById("scannedBarcodes").getContext("2d");
console.log(mostScannedBarcodes);
let scannedBarcodes = new Chart(mostScannedBarcodes, {
    type: 'bar',
    data: {
        labels: Object.keys(jsonConst.mostScannedBarcodes),
        datasets: [{
            label: 'Most Scanned Barcode',
            data: jsonConst.mostScannedBarcodes,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

//Generate Most Flagged Ingredients
const mostFlaggedIngredients = document.getElementById("flaggedIngredients").getContext("2d");
console.log(mostFlaggedIngredients);
let flaggedIngredients = new Chart(mostFlaggedIngredients, {
    type: 'bar',
    data: {
        labels: Object.keys(jsonConst.mostFlaggedIngredients),
        datasets: [{
            label: 'Most Flagged Ingredients',
            data: jsonConst.mostFlaggedIngredients,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

var intervalId = window.setInterval(function () {
    getUsageAnalysisObject();
    loginTrend.update();
    signupTrend.update();
    mostViewed.update();
    averageDuration.update();
    scannedBarcodes.update();
    flaggedIngredients.update();
}, 1000)