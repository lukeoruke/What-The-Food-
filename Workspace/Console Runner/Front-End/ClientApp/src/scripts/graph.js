/**
 * Render graph from chart.js
 */

//Create array that will store all graph data objects
let labelObjects = [];

const GRAPH = document.getElementById('myChart');
const barGraph = new Chart(GRAPH, {
    type: 'bar',
    data: {
        datasets: [{
            data: [15, 15, 15],
        }],
        labels: ['item1', 'item2', 'item3']
    }
})

const servings = [3, 2, 6]
const servingSize = [4, 6, 2]
const push = document.getElementById('push');
const select = document.getElementById('labelItems');
var labelItems = document.getElementById("labelItems");
labelItems.addEventListener("change", function () {
    console.log(labelItems.value)
    populateBarGraph(servingSize);
});
var selectionLabels = []
for (i = 0; i < labelItems.length; i++) {
    selectionLabels.push(labelItems[i].value);
}





function populateBarGraph(data) {
    barGraph.data.datasets[0].data = data

    //const pushValue = document.getElementById('pushValue');
    //const pushLabel = document.getElementById('pushLabel');
    //barGraph.data.datasets[0].data.push(pushValue.value);
    //barGraph.data.labels.push(pushLabel.value);
    //console.log(barGraph.data.datasets[0].data);
    barGraph.update();
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
        




