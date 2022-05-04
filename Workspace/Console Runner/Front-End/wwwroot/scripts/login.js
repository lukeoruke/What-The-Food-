async function sendLogin(e) {
    e.preventDefault();

    console.log('Attempting to login...');
    let email = document.getElementById('email').value;
    let password = document.getElementById('password').value;

    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    console.log(email);
    console.log(password);


    var postResponse;
    // HTTP Post Request\
    await fetch('http://localhost:49202/api/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        postResponse = response.status; // returns 200;
    });

    if (postResponse == 200) {
        // HTTP Get Request
        await fetch('http://localhost:49202/api/AccountLogin').then(async response => localStorage.setItem('JWT', JSON.stringify(await response.json()))).then(data => console.log(data));
        
        var jsonData = localStorage.getItem('JWT');
        var obj = JSON.parse(jsonData);
        obj= JSON.stringify(obj.token);
    }


        




    alert('Successfully sent login request!');
}
