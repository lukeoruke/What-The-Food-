/**
 * Render graph from chart.js
 */

const GRAPH = document.getElementById("testGraph");


//Create array that will store all graph data objects
let graphs = [];

//let barGraph = new Chart(GRAPH, {
//    type: 'bar',
//    data: {
//        datasets: [{
//            data: [20, 10],
//        }],
//        labels: ['a', 'b']
//    }
//})

//console.log(GRAPH);
//const push = document.getElementById('push');
//push.addEventListener('click', populateBarGraph);


function populateBarGraph() {
    const pushValue = document.getElementById('pushValue');
    barGraph.data.datasets[0].data.push(pushValue.value);
    console.log(barGraph.data.datasets[0].data.push(pushValue.value));
    

}

async function getFoodObject(e) {
    e.preventDefault();

    console.log('Attempting to Scan...');
    let barcode = document.getElementById('barcode').value;
    const formData = new FormData();
    formData.append('barcode', barcode);
    //JSON.stringify(formData);
    console.log(barcode);

    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode?' + barcode)
        .then(async response => localStorage.setItem('foodInfo', JSON.stringify(await response.json())))
        .then(data => console.log(data));
    
    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
}

async function getHistory(e) {
    e.preventDefault();

    await fetch('http://localhost:49200/api/GetHistory',{
        method: 'GET'
    })
}
        




