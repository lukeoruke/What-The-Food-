async function getFoodObject(e) {
    e.preventDefault();
    let barcode = document.getElementById("barcode").values;
    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCodeController', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });