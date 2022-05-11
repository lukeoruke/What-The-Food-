
localStorage.clear();
sessionStorage.clear();
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
    // HTTP Post Request
    await fetch('http://47.151.24.23:49202/api/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(response => response.json())
        .then((response) => {
            console.log(response)
            if (response.token !== "") {
                localStorage.setItem('JWT', response.token);
                window.location.replace("http://whatthefood.xyz/index.html");
            } else {
                alert('Invalid login');
            }
        })


        //.then(async response => localStorage.setItem('JWT', JSON.stringify(await response.json()))).then(data => console.log(data));










}
