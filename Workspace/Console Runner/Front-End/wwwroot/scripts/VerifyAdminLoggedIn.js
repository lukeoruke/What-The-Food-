(async () => {
    var jwt = localStorage.getItem('JWT');

    fetch('http://47.151.24.23:49202/api/ValidateAdminLoggedIn?' + new URLSearchParams({ token: jwt }))
        .then(response => response.text())
        .then((response) => {
            if (response === "false") {
                window.location.replace("http://whatthefood.xyz/login.html");
            }
        })
})();
