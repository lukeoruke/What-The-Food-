/// <reference path="C:\Users\Gabe\Source\Repos\lukeoruke\What-The-Food-\Workspace\Console Runner\UnitTest\unit test\jasmine\News.js" />

describe("News", function () {
    var news;
    var bias = 3;
    //This will be called before running each spec
    beforeEach(function () {
        news = new News();
    });

    describe("running unit test for news", function () {

        it("should be able to increment bias counter", function () {
            expect(news.incrementHealthFilter(bias)).toEqual(bias + 1);
        });
        it("should be able to decrement bias counter", function () {
            expect(news.decrementHealthFilter(bias)).toEqual(bias - 1);
        });

        it("should be able to store food news", function () {
            expect(news.storeFoodNews(bias)).toBeTruthy();
        });

        it("should be able to store health news", function () {
            expect(news.storeHealthNews(bias)).toBeTruthy();
        });
        it("should be able to store health news", function () {
            expect(news.storeHealthNews(bias)).toBeTruthy();
        });
        it("should be able to display news", function () {
            expect(news.displayNews()).toBeTruthy();
        });


    });
});