

var previousViewName = '';
var currentViewName = '';
var time = 0;


//Search localstorage for time and name of view to fetch call
//Save current time and name of new view to localstorage
function placeLocalStorage(previousViewName, currentViewName, time) {
    //If localstorage doesn't contain any information, place viewNames and time
    if (localStorage.getItem(this.previousViewName != null) {
        localStorage.setItem(this.previousViewName, previousViewName);
    }
    if (localStorage.getItem(this.currentViewName != null){
        localStorage.setItem(this.currentViewName, currentViewName);
    }
    if (localStorage.getItem(this.time != null)) {
        localStorage.setItem(this.time, Date().getTime());
    }


});


(async () => {
    var jwt = localStorage.getItem('JWT');
    //viewName: string
    //time: int
    await fetch('http://localhost:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt, previousViewName: previousViewName, currentViewName: currentViewName, time: time, }))
        .then(response => response.text())
        .then((response) => {
            if (response === "False") {
                window.location.replace("https://localhost:49199/login.html");
            }
        })
    

})();


