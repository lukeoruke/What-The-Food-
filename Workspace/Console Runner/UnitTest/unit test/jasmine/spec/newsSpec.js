/// <reference path="C:\Users\Luke\Documents\GitHub\What-The-Food-\Workspace\Console Runner\Front-End\wwwroot\unit test\jasmine\News.js" />

describe("News", function () {
    var news;
    var bias = 3;
    var healthFilterLevel = 0;
    var healthFilterLevel2 = 2;
    //var index
    //This will be called before running each spec
    beforeEach(function () {
        news = new News();
    });
    //test all JS functions that will be used 
    describe("running unit test for news", function () {
        it("should be able to pick category of news to display", function () {
            expect(news.pickCategory(healthFilterLevel)).toEqual("storeFood");
        });
        it("should be able to pick category of news to display", function () {
            expect(news.pickCategory(healthFilterLevel2)).toEqual("bothNews");
        });

        it("should be able to store food news", function () {
            expect(news.storeFoodNews(bias)).toBeTruthy();
        });

        it("should be able to store health news", function () {
            expect(news.storeHealthNews(bias)).toBeTruthy();
        });

        it("should be able to display news", function () {
            expect(news.displayNews()).toBeTruthy();
        });
        it("should be able to categorize food: health ver", function () {
            expect(news.getCategory(0)).toEqual("increment");
        });
        it("should be able to categorize food: food ver", function () {
            expect(news.getCategory(1)).toEqual("decrement");
        });
        it("should be able to get Health Filter", function () {
            expect(news.getHealthFilter()).toBeTruthy();
        });
        it("should be able to increment bias counter", function () {
            expect(news.incrementHealthFilter(bias)).toEqual(bias + 1);
        });
        it("should be able to decrement bias counter", function () {
            expect(news.decrementHealthFilter(bias)).toEqual(bias - 1);
        });

    });
});