
localStorage.clear();
sessionStorage.clear();
sessionStorage.setItem("currentViewName", "login.html");
sessionStorage.setItem("viewTimestamp", Date.now());

async function sendLogin(e) {
    e.preventDefault();

    console.log('Attempting to login...');
    let email = document.getElementById('email').value;
    let password = document.getElementById('password').value;

    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);


    var postResponse;
    // HTTP Post Request\
    await fetch('http://localhost:49202/api/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(response => response.json())
        .then((response) => {
            console.log(response)
            if (response.token !== "") {
                localStorage.setItem('JWT', response.token);
                window.location.replace("https://localhost:49199/index.html");
            } else {
                alert('Invalid login');
            }
        })


        //.then(async response => localStorage.setItem('JWT', JSON.stringify(await response.json()))).then(data => console.log(data));










}
