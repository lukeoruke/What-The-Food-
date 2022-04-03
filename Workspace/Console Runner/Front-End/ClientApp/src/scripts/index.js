//implement API
//const API = 'https://newsapi.org/v2/top-headlines?country=us&category=health&apiKey=5ad1255abe2a4621b94e10301d1b19cf'


//news one
//fetch(API)
//    .then((data) => {
//        return data.json();
//    }).then((completedata) => {
//        let data1 = "";
//        console.log(completedata);
//        completedata.articles.map((values) => {
//            data1 = `<div class = "newsBox">
//                <div class = "imageBox">
//                    <img src = ${values.urlToImage} class ="center">
//                </div>
//                    <h1 class = "headlineBox">${values.title}</h1>
//                    <p class = "textBox">${values.description}</p>
//                </div>`
//        });
//        document.getElementById("cards").innerHTML = data1;
//    }).catch((err) => {
//        console.log("ERROR:"+ err);
//    })


//const API = 'https://api.nytimes.com/svc/search/v2/articlesearch.json?fq=section_name:(%22Food%22)&sort=newest&api-key=TK2GSBMDBO2kHDUABQFh4tlpE0Bu8cuf'
//Version 1
//let news = document.getElementById("news");

//fetch(API)
//    .then(response => response.json())
//    .then(data => {
//        console.log(data);
//        data.response.docs.map(article => {
//            console.log(article.headline.main);
//            let a = document.createElement("a");
//            a.setAttribute('href', article.web_url);
//            console.log(a);
//            a.innerHTML = article.headline.main;
//            news.appendChild(a);
//        })
//        console.log(news);
//    })

//Version 2

let counter = 0;
fetch(API)
    .then(response => response.json())
    .then(data => {
        console.log(data);
        data.response.docs.slice(0, 5).map(article => {
            console.log(article.headline.main);
            let a = document.createElement("a");
            a.setAttribute('href', article.web_url);
            //console.log(article.web_url);
            a.innerHTML = article.headline.main;
            console.log(a);


            let p = document.createElement("p");
            p.innerHTML = article.lead_paragraph;

            let img = document.getElementById("imgID" + counter).src =
                ("https://static01.nyt.com/" + article.multimedia[0].url);
            //img.setAttribute('src', ("https://static01.nyt.com/" + article.multimedia[0].url));
            console.log(img);
            //q.innerHTML = "https://static01.nyt.com/images" + article.multimedia[0].url;
            //console.log(article.multimedia[0].url);
            //console.log(q);

            let headlineText = document.getElementById("headlineText" + counter);
            let text = document.getElementById("text" + counter);
            headlineText.appendChild(a);
            text.appendChild(p);
            //image.append(q);
            counter++;
            console.log(counter);
        });
    })
    //.then(data => {
    //    console.log(data);
    //    data.response.docs.slice(0, 5).map(article => {


    //        let p = document.createElement("p");
    //        p.innerHTML = article.lead_paragraph;



    //        let text = document.getElementById("text" + counter);
    //        text.appendChild(a);

    //        text.appendChild(p);
    //        counter++;
    //        console.log(counter);
    //    });
    //})



//https://static01.nyt.com/images

//https://static01.nyt.com/images

