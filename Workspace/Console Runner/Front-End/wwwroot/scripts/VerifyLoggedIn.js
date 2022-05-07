

(async () => {
    var jsonData = JSON.parse(localStorage.getItem('JWT'));
    var jwt = JSON.stringify(jsonData.token);
    await fetch('http://localhost:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt }))
        .then(response => response.text())
        .then((response) => {
            console.log(response)
            if (response == "false") {
                window.location.replace("https://localhost:49199/login.html");
            }
        })


})();
