

(async () => {
    var jwt = localStorage.getItem('JWT');

    await fetch('http://47.151.24.23:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt }))
        .then(response => response.text())
        .then((response) => {
            if (response === "False") {
                console.log("Not verified");
                window.location.replace("http://whatthefood.xyz/login.html");
            }
        })
})();