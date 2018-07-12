using NUnit.Framework;
using System.Collections.Generic;

namespace csharp
{
    [TestFixture]
    public class GildedRoseTest
    {
        private const string BACKSTAGE_PASS = "Backstage passes to a TAFKAL80ETC concert";

        [Test]
        public void LegendaryItem_QualityDoesNotDecrease() {
            // Arrange
            GildedRose app = new GildedRose(createItemList("Sulfuras, Hand of Ragnaros", 20, 33));
    
            // Act
            app.UpdateQuality();
    
            // Assert
            Assert.AreEqual(33, app.Items[0].Quality, "Quality is not decreased for this legendary item");
        }
    
        [Test]
        public void LegendaryItem_NeverHasToBeSold() {
            GildedRose app = new GildedRose(createItemList("Sulfuras, Hand of Ragnaros", 1, 17));
            app.UpdateQuality();
            Assert.AreEqual(1, app.Items[0].SellIn, "Sellin is not decreased for this legendary item");
        }
    
        [Test]
        public void GenericItem_SellinDecreasesEachUpdate() {
            GildedRose app = new GildedRose(createItemList("generic item", 8, 10));
            app.UpdateQuality();
            Assert.AreEqual(7, app.Items[0].SellIn, "Item sellin date should decrease by 1 each day");
        }
    
        [Test]
        public void GenericItem_SellinDateGoesNegative() {
            GildedRose app = new GildedRose(createItemList("generic item", 0, 25));
            app.UpdateQuality();
            Assert.AreEqual(-1, app.Items[0].SellIn, "Sellin date will go negative once sellin date is reached");
        }
    
        [Test]
        public void GenericItem_QualityDecreasesBeforeSellinDate() {
            GildedRose app = new GildedRose(createItemList("generic item", 5, 10));
            app.UpdateQuality();
            Assert.AreEqual(9, app.Items[0].Quality, "Item quality should only decrease by 1 each day");
        }
    
        [Test]
        public void GenericItem_QualityDecreasesTwiceAsFastAfterSellinDate() {
            GildedRose app = new GildedRose(createItemList("generic item", 0, 10));
            app.UpdateQuality();
            Assert.AreEqual(8, app.Items[0].Quality, "When sellin date is 0 then quality decreases twice as fast");
        }
    
        [Test]
        public void GenericItem_QualityNeverGoesNegative() {
            GildedRose app = new GildedRose(createItemList("generic item", 0, 0));
            app.UpdateQuality();
            Assert.AreEqual(0, app.Items[0].Quality, "Quality will not go negative once it is zero");
        }
    
        [Test]
        public void AgedBrie_QualityIncreases() {
            GildedRose app = new GildedRose(createItemList("Aged Brie", 5, 30));
            app.UpdateQuality();
            Assert.AreEqual(31, app.Items[0].Quality, "Aged Brie increases quality with age");
        }
    
        [Test]
        public void AgedBrie_QualityIsCappedAt50() {
            GildedRose app = new GildedRose(createItemList("Aged Brie", 5, 50));
            app.UpdateQuality();
            Assert.AreEqual(50, app.Items[0].Quality, "Quality has an upper limit that is not exceeded");
        }
    
        [Test]
        public void AgedBrie_QualityIncreases_EvenAfterSellInDate() {
            GildedRose app = new GildedRose(createItemList("Aged Brie", -1, 20));
            app.UpdateQuality();
            Assert.AreEqual(22, app.Items[0].Quality, "Aged Brie improves twice as fast after sellin date (BUG?)");
        }
    
        [Test]
        public void AgedBrie_QualityIsCappedAt50_EvenWhenReallyOld() {
            GildedRose app = new GildedRose(createItemList("Aged Brie", -99, 50));
            app.UpdateQuality();
            Assert.AreEqual(50, app.Items[0].Quality, "Quality has an upper limit, even when cheese is old");
        }
    
        [Test]
        public void BackstagePass_QualityIncreasesEachDay() {
            GildedRose app = new GildedRose(createItemList(BACKSTAGE_PASS, 30, 23));
            app.UpdateQuality();
            Assert.AreEqual(24, app.Items[0].Quality, "Backstage Pass increases quality with age");
        }
    
        [Test]
        public void BackstagePass_QualityIncreasesMoreAsConcertNears() {
            GildedRose app = new GildedRose(createItemList(BACKSTAGE_PASS, 10, 40));
            app.UpdateQuality();
            Assert.AreEqual(42, app.Items[0].Quality, "Backstage Pass quality increases more when concert is near");
        }
    
        [Test]
        public void BackstagePass_QualityIncreasesMuchMoreWhenConcertIsClose() {
            GildedRose app = new GildedRose(createItemList(BACKSTAGE_PASS, 5, 40));
            app.UpdateQuality();
            Assert.AreEqual(43, app.Items[0].Quality, "Backstage Pass quality increases even more when concert is almost here");
        }
    
        [Test]
        public void BackstagePass_QualityIsCappedAt50() {
            GildedRose app = new GildedRose(createItemList(BACKSTAGE_PASS, 40, 50));
            app.UpdateQuality();
            Assert.AreEqual(50, app.Items[0].Quality, "Quality has an upper limit that is not exceeded");
        }
    
        [Test]
        public void BackStagePass_QualityDropsToZeroWhenConcertPasses() {
            GildedRose app = new GildedRose(createItemList(BACKSTAGE_PASS, 0, 50));
            app.UpdateQuality();
            Assert.AreEqual(0, app.Items[0].Quality, "Backstage Pass is worthless when concert has passed");
        }

        //NEW BEHAVIOR
        // conjured items
    
        private List<Item> createItemList(string itemName, int sellIn, int quality) {
            return new List<Item> { new Item { Name = itemName, SellIn = sellIn, Quality = quality } };
        }        
    }
}
