using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Console_Runner.FoodService;

namespace UnitTest
{
    public class FoodUpdatesShould
    {
        [Fact]
        public async void ThrowIfUsingParentFoodUpdate()
        {
            // arrange
            IFoodGateway foodAccess = new MemFoodGateway();
            IFoodUpdateGateway updateAccess = new MemFoodUpdateGateway();
            FoodDBOperations foodDBOperations = new FoodDBOperations(foodAccess, updateAccess);
            FoodItem testItem = new FoodItem("barcode", "productName", "companyName", "pic");
            DateTime dateTime = DateTime.Now;
            DateTime someDate = DateTime.Parse("2000-03-01");
            FoodUpdate foodUpdate = new FoodUpdate(testItem, dateTime, "update message");
            FoodRecall foodRecall = new FoodRecall(testItem, dateTime, "recall message",
                                                   new[] { "location" },
                                                   new[] { 100 },
                                                   new[] { someDate });

            // act
            bool recallAdded = await foodDBOperations.AddFoodUpdateAsync(foodRecall);

            // assert
            Assert.True(recallAdded);
            await Assert.ThrowsAsync<ArgumentException>(() => foodDBOperations.AddFoodUpdateAsync(foodUpdate));
        }

        [Fact]
        public async void RetrieveFoodUpdatesProperly()
        {
            // arrange
            IFoodGateway foodAccess = new MemFoodGateway();
            IFoodUpdateGateway updateAccess = new MemFoodUpdateGateway();
            FoodDBOperations foodDBOperations = new FoodDBOperations(foodAccess, updateAccess);
            FoodItem testItem = new FoodItem("barcode", "productName", "companyName", "pic");
            FoodItem otherTestItem = new FoodItem("otherbarcode", "productName", "companyName", "pic");
            DateTime dateTime = DateTime.Now;
            DateTime someDate = DateTime.Parse("2000-03-01");
            FoodRecall foodRecall = new FoodRecall(testItem, 
                                                   dateTime, 
                                                   "recall message",
                                                   new[] { "location" },
                                                   new[] { 100 },
                                                   new[] { someDate });
            FoodRecall otherFoodRecall = new FoodRecall(otherTestItem, 
                                                   dateTime, 
                                                   "recall message",
                                                   new[] { "location" },
                                                   new[] { 100 },
                                                   new[] { someDate });
            FoodIngredientChange foodIngChange = new FoodIngredientChange(testItem,
                                                                          dateTime,
                                                                          "change message",
                                                                          new[] { new Ingredient("added ingredient", "ai", "some ing") },
                                                                          new[] { new Ingredient("removed ingredient", "ri", "another ing") });

            // act
            await foodDBOperations.AddFoodUpdateAsync(foodRecall);
            await foodDBOperations.AddFoodUpdateAsync(otherFoodRecall);
            await foodDBOperations.AddFoodUpdateAsync(foodIngChange);
            List<FoodUpdate> recallTest = await foodDBOperations.GetAllUpdatesForBarcodeAsync(testItem.Barcode);
            FoodUpdate? retrievedRecall = recallTest.Where(fu => fu.Id == foodRecall.Id).FirstOrDefault();
            FoodUpdate? retrievedChange = recallTest.Where(fu => fu.Id == foodIngChange.Id).FirstOrDefault();

            // assert
            Assert.NotNull(retrievedRecall);
            Assert.NotNull(retrievedChange);
            Assert.IsType<FoodRecall>(retrievedRecall);
            Assert.IsType<FoodIngredientChange>(retrievedChange);
            FoodRecall castedRetrievedRecall = (FoodRecall)retrievedRecall!;
            FoodIngredientChange castedRetrievedChange = (FoodIngredientChange)retrievedChange!;

            // check if foodRecall matches with retrieved recall
            Assert.Equal(foodRecall.FoodItemBarcode, castedRetrievedRecall.FoodItemBarcode);
        }

        [Fact]
        public async void RemoveFoodUpdatesProperly()
        {
            // arrange
            IFoodGateway foodAccess = new MemFoodGateway();
            IFoodUpdateGateway updateAccess = new MemFoodUpdateGateway();
            FoodDBOperations foodDBOperations = new FoodDBOperations(foodAccess, updateAccess);
            FoodItem testItem = new FoodItem("barcode", "productName", "companyName", "pic");
            FoodItem otherTestItem = new FoodItem("otherbarcode", "productName", "companyName", "pic");
            DateTime dateTime = DateTime.Now;
            DateTime someDate = DateTime.Parse("2000-03-01");
            FoodRecall foodRecall = new FoodRecall(testItem,
                                                   dateTime,
                                                   "recall message",
                                                   new[] { "location" },
                                                   new[] { 100 },
                                                   new[] { someDate });
            FoodRecall otherFoodRecall = new FoodRecall(testItem,
                                                        dateTime,
                                                        "other recall message",
                                                        new[] { "location" },
                                                        new[] { 100 },
                                                        new[] { someDate });
            
            // act
            await foodDBOperations.AddFoodUpdateAsync(foodRecall);
            await foodDBOperations.AddFoodUpdateAsync(otherFoodRecall);

            // foodRecall is removed, so otherFoodRecall should remain
            await foodDBOperations.RemoveFoodUpdateByIdAsync(foodRecall.Id);
            FoodUpdate? retrievedUpdate = (await foodDBOperations.GetAllUpdatesForBarcodeAsync(testItem.Barcode)).FirstOrDefault();

            // assert
            Assert.NotNull(retrievedUpdate);
            Assert.IsType<FoodRecall>(retrievedUpdate);
            FoodRecall castedUpdate = (FoodRecall)retrievedUpdate!;
            Assert.Equal(otherFoodRecall.Message, castedUpdate.Message);

        }
    }
}
