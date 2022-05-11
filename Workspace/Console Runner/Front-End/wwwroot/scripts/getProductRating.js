// Function to send foodName to the back-end
async function postFoodName() {
    //console.log('Attempting to get product rating...');
    let foodName = document.getElementById('foodName').innerText;
    const formData = new FormData();
    formData.append('foodName', foodName);
    //console.log(foodName);

    // HTTP Post Request
    await fetch('http://47.151.24.23:49202/api/GetProductRating', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        //console.log(response.status); // returns 200
    });
}

// Function to receive the productRating from the back-end
async function getProductRating(e) {
    e.preventDefault();

    let rating;
    // HTTP Get Request
    await fetch('http://47.151.24.23:49202/api/GetProductRating')
        .then(async function (response) {
            rating = JSON.stringify(await response.json())
            const jsonConst = JSON.parse(rating);
            //console.log(jsonConst);
            rating = 'Rating: ' + jsonConst.starRating + ' (' + jsonConst.rating + ') ' + jsonConst.ratingCount + ' ratings';
            //console.log(jsonConst.ratingCount);
            document.getElementById("rating").innerText = rating;
        });
}
