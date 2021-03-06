
//Nutrition label from food objects will go into list, which will then be
//populated into the graph
var names = [], calories = [], servings = [], servingSize = [],
totalFat = [], saturatedFat = [], transFat = [], cholesterol = [], sodium = [],
totalCarb = [], fiber = [], totalSugar = [], addedSugar = [], protein = [];

//jwt
var jwt = localStorage.getItem('JWT');

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
            label: Object.keys(labelObjects)[1],
            data: labelObjects.Calories,
            backgroundColor: 'rgb(255, 99, 132)',
            borderColor: 'rgb(255, 99, 132)',
        }],
        labels: labelObjects.ProductName
    },
    options: {
        plugins: {
            legend: {
                display: true,
                labels: {
                    color: 'rgb(255, 99, 132)',
                },
                title: {
                    text: 'Calories'
                }
            }
        }
    }
})

const push = document.getElementById('push');
push.addEventListener('click', getFoodObject);
async function getFoodObject(e) {
    e.preventDefault();

    
    let barcode = document.getElementById('pushValue').value;
    const formData = new FormData();
    formData.append('barcode', barcode);
    //JSON.stringify(formData);
    

    // HTTP Get Request
    await fetch('http://47.151.24.23:49202/api/GetFoodProductFromBarCode?' + new URLSearchParams({
        barcode: barcode, token: jwt
    }))
        .then(async response => localStorage.setItem('foodInfo', JSON.stringify(await response.json())))
        .then(populateDictionary)
}
    
function populateDictionary() {
    var jsonData = localStorage.getItem('foodInfo');
    const jsonConst = JSON.parse(jsonData);
    labelObjects.ProductName.push(jsonConst.ProductName);
    labelObjects.Calories.push(jsonConst.Calories);
    labelObjects.Servings.push(jsonConst.Servings);
    labelObjects.ServingSize.push(jsonConst.ServingSize);
    labelObjects.TotalFat.push(jsonConst.TotalFat);
    labelObjects.SaturatedFat.push(jsonConst.SaturatedFat);
    labelObjects.TransFat.push(jsonConst.TransFat);
    labelObjects.Cholesterol.push(jsonConst.Cholesterol);
    labelObjects.Sodium.push(jsonConst.Sodium);
    labelObjects.TotalCarbohydrate.push(jsonConst.TotalCarbohydrate);
    labelObjects.DietaryFiber.push(jsonConst.DietaryFiber);
    labelObjects.TotalSugar.push(jsonConst.TotalSugars);
    labelObjects.AddedSugar.push(jsonConst.AddedSugar);
    labelObjects.Protein.push(jsonConst.Protein);
    barGraph.update();
    localStorage.removeItem('foodInfo');
 };
    


var labelItems = document.getElementById("labelItems");
labelItems.addEventListener("change", function () {
    for (var key in labelObjects)
        if (key === labelItems.value) {
            populateBarGraph(labelObjects[key]);
            barGraph.data.datasets[0].label = key;
            barGraph.update();
        }
});

function populateBarGraph(data) {
    barGraph.data.datasets[0].data = data;
}



async function getHistory(e) {
    e.preventDefault();

    await fetch('http://47.151.24.23:49202/api/GetHistory',{
        method: 'GET'
    })
}
        




