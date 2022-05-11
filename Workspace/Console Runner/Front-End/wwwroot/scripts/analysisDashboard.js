
//Generate Login Trend
const login = document.getElementById("loginTrend").getContext("2d");
console.log(login);
let loginTrend = new Chart(login, {
    type: 'line',
    data: {
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
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
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
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
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
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
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
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
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
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
        labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [65, 59, 80, 81, 56, 55, 40],
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    }

});

async function getUsageAnalysisObject(e)
{
    e.preventDefault();
    //HTTP Get Request
    await fetch('http://localhost:49202/api/GetUsageAnalysisObject?' + new URLSearchParams({
        token: localStorage.getItem('JWT')
    })).then(async response => localStorage.setItem('usageAnalysisObject', JSON.stringify(await response.json())))
            
}

function populateDictionary() {
    var jsonData = localStorage.getItem('usageAnalysisObject');
    const jsonConst = JSON.parse(jsonData)
    console.log(jsonConst)

}