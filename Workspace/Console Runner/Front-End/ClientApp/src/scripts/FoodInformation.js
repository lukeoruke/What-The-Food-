async function getFoodObject(e) {
    e.preventDefault();

    console.log('Attempting to Scan...');
    let barcode = document.getElementById('barcode').value;
    const formData = new FormData();
    formData.append('barcode', barcode);
    //JSON.stringify(formData);
    console.log(barcode);



    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));

    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
}