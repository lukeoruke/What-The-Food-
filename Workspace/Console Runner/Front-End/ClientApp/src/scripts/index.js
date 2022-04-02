//implement API
const API = 'https://newsapi.org/v2/top-headlines?country=us&category=health&apiKey=5ad1255abe2a4621b94e10301d1b19cf'

//fetch(API)
//    .then((data) => data.json())  //makes a promise
//    .then((articles) => generateHTML(articles)) //this uses it

//const generateHtml = (data) => {
//    data.articles.forEach(article => {
//        let li = document.createElement('li');
//        let a = document.createElement('a');
//        a.setAttribute('href', article.url);
//        a.setAttribute('target', _blank)
//        a.textContent = article.title;
//    })
//}

//implement API
//const API = 'https://newsapi.org/v2/top-headlines?country=us&category=health&apiKey=5ad1255abe2a4621b94e10301d1b19cf'
//const newsList = document.querySelector('.headerBox');
//fetch(API)
//    .then((data) => data.json())  //makes a promise
//    .then((articles) => {
//        let title = document.getElementById('title');
//        console.log(title);
//        document.querySelector('body').appendChild(title);
//    });

fetch(API)
    .then((data) => {
        return data.json();
    }).then((completedata) => {
        let data1 = "";
        console.log(completedata);
        completedata.articles.map((values) => {
            data1 = `<div class = "newsBox">
                <div class = "imageBox">
                    <img src = ${values.urlToImage} class ="center">
                </div>
                    <h1 class = "headlineBox">${values.title}</h1>
                    <p class = "textBox">${values.description}</p>
                </div>`
        });
        document.getElementById("cards").innerHTML = data1;
    }).catch((err) => {
        console.log("ERROR:"+ err);
    })