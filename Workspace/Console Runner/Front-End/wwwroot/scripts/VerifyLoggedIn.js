

(async () => {
    var jwt = localStorage.getItem('JWT');

    await fetch('http://localhost:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt }))
        .then(response => response.text())
        .then((response) => {
            if (response === "False") {
                window.location.replace("https://localhost:49199/login.html");
            }
        })


})();
