
//Nutrition label from food objects will go into list, which will then be
//populated into the graph
var names = [], calories = [], servings = [], servingSize = [],
totalFat = [], saturatedFat = [], transFat = [], cholesterol = [], sodium = [],
totalCarb = [], fiber = [], totalSugar = [], addedSugar = [], protein = [];

//Create dictionary that will store all graph data objects from JSON
var labelObjects = {
    'ProductName': names,
    'Calories': calories,
    'Servings':servings,
    'ServingSize':servingSize,
    'TotalFat':totalFat,
    'SaturatedFat':saturatedFat,
    'TransFat':transFat,
    'Cholesterol':cholesterol,
    'Sodium':sodium,
    'TotalCarbohydrate':totalCarb,
    'DietaryFiber':fiber,
    'TotalSugar':totalSugar,
    'AddedSugar':addedSugar,
    'Protein': protein
};
const GRAPH = document.getElementById('myChart');
const barGraph = new Chart(GRAPH, {
    type: 'bar',
    data: {
        datasets: [{
            data: labelObjects['Calories'],
        }],
        labels: labelObjects['ProductName']
    }
})

const push = document.getElementById('push');
push.addEventListener('click', getFoodObject);

async function getFoodObject(e) {
    e.preventDefault();

    console.log('Attempting to Scan...');
    let barcode = document.getElementById('pushValue').value;
    const formData = new FormData();
    formData.append('barcode', barcode);
    //JSON.stringify(formData);
    console.log(barcode);

    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode?' + barcode)
        .then(async response => localStorage.setItem('foodInfo', JSON.stringify(await response.json())))
        .then(data => console.log(data))
        .then(populateDictionary())
    
    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
}
    
function populateDictionary() {
    var jsonData = localStorage.getItem('foodInfo');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    console.log(jsonConst);
    console.log(jsonConst.ProductName);
    

}

var labelItems = document.getElementById("labelItems");
labelItems.addEventListener("change", function () {
    for (var key in labelObjects)
        if (key === labelItems.value) {
            populateBarGraph(labelObjects[key]);
            barGraph.update();
        }
});

function populateBarGraph(data) {
    barGraph.data.datasets[0].data = data

    //const pushValue = document.getElementById('pushValue');
    //const pushLabel = document.getElementById('pushLabel');
    //barGraph.data.datasets[0].data.push(pushValue.value);
    //barGraph.data.labels.push(pushLabel.value);
    //console.log(barGraph.data.datasets[0].data);
}



async function getHistory(e) {
    e.preventDefault();

    await fetch('http://localhost:49200/api/GetHistory',{
        method: 'GET'
    })
}
        




