async function scrapeRatings() {
    console.log('Attempting to get product rating...');
    let foodName = document.getElementById('foodName').innerText;
    const formData = new FormData();
    formData.append('foodName', foodName);
    //JSON.stringify(formData);
    console.log(foodName);


    // HTTP Post Request
    await fetch('http://localhost:49200/api/GetProductRating', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200
    });
}

async function getProductRating(e) {
    e.preventDefault();

    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetFoodProductFromBarCode?' + '1111')
        .then(async response => console.log(JSON.stringify(await response.json())))
        .then(data => console.log(data));

    let rating;
    // HTTP Get Request
    await fetch('http://localhost:49200/api/GetProductRating')
        .then(async function (response) {
            rating = JSON.stringify(await response.json())
            const jsonConst = JSON.parse(rating);
            console.log(jsonConst);
            console.log(jsonConst.starRating);
            console.log(jsonConst.rating);
            console.log(jsonConst.ratingCount);
            rating = 'Rating: ' + jsonConst.starRating + ' (' + jsonConst.rating + ') ' + jsonConst.ratingCount + ' ratings';
            document.getElementById("rating").innerText = rating;

        });



}
