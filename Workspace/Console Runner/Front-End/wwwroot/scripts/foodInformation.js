function populateFoodInfo() {
    var jsonData = localStorage.getItem('foodInfo');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    console.log(jsonConst);
    console.log(jsonConst.ProductName);
    var foodInfo = "";
    for (data in jsonConst) {
        if (data != 'IngredientAlternateName' || data != 'IngredientDescription')
            foodInfo += data + ": " + jsonConst[data] + "\n";
    }
    document.getElementById("info").innerText = foodInfo;
    document.getElementById("foodName").innerText = jsonConst.ProductName;
    document.getElementById("barcodeNumber").innerText = jsonConst.Barcode;
    //document.getElementById("flaggedItems").innerText = jsonConst.Barcode;
}
