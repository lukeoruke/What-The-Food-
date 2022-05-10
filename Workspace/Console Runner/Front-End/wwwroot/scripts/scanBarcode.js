var jwt = localStorage.getItem('JWT');

async function getFoodObject(e) {
    e.preventDefault();

    console.log('Attempting to Scan...');
    let barcode = document.getElementById('barcode').value;
    const formData = new FormData();
    formData.append('barcode', barcode);
    //JSON.stringify(formData);
    console.log(barcode);

    // HTTP Get Request
    await fetch('http://47.151.24.23:49202/api/GetFoodProductFromBarCode?' + new URLSearchParams({
        barcode: barcode, token: jwt
    }))
        .then(async response => localStorage.setItem('foodInfo', JSON.stringify(await response.json())))
        .then(data => console.log(data));

    // HTTP Post Request
    await fetch('http://47.151.24.23:49202/api/GetFoodProductFromBarCode', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });

    window.location.replace("/foodInformation.html");
}