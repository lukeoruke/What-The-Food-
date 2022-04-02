async function getFoodObject(e) {
    e.preventDefault();
    let barcode = document.getElementById("barcode").values;
    const formData = new FormData();
    formData.append('barcode', barcode);

    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCodeController', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });

    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCodeController')
        .then(data => console.log(data.text()))
        .then(response => console.log(response));
}