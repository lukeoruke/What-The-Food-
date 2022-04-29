
//Nutrition label from food objects will go into list, which will then be
//populated into the graph
var names = ['item1'], calories = [15], servings = [10], servingSize = [5],
totalFat = [12], saturatedFat = [], transFat = [], cholesterol = [], sodium = [],
totalCarb = [], fiber = [], totalSugar = [], addedSugar = [], protein = [];

//Create dictionary that will store all graph data objects
var labelObjects = {
    'names': names,
    'calories': calories,
    'servings':servings,
    'servingSize':servingSize,
    'totalFat':totalFat,
    'saturatedFat':saturatedFat,
    'transFat':transFat,
    'cholesterol':cholesterol,
    'sodium':sodium,
    'totalCarb':totalCarb,
    'fiber':fiber,
    'totalSugar':totalSugar,
    'addedSugar':addedSugar,
    'protein': protein
};
const GRAPH = document.getElementById('myChart');
const barGraph = new Chart(GRAPH, {
    type: 'bar',
    data: {
        datasets: [{
            data: labelObjects['calories'],
        }],
        labels: labelObjects['names']
    }
})



const push = document.getElementById('push');
const select = document.getElementById('labelItems');
var labelItems = document.getElementById("labelItems");
labelItems.addEventListener("change", function () {
    
    for (var key in labelObjects)
        if (key === labelItems.value) {
            populateBarGraph(labelObjects[key]);
            barGraph.update();
        }

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
        




