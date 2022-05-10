(async () => {
    var jwt = localStorage.getItem('JWT');

    fetch('http://localhost:49202/api/ValidateAdminLoggedIn?' + new URLSearchParams({ token: jwt }))
        .then(response => response.text())
        .then((response) => {
            if (response === "false") {
                window.location.replace("https://localhost:49199/login.html");
            }
        })
})();
