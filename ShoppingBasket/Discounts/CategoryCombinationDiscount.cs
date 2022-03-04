using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Discounts
{
    public class CategoryCombinationDiscount : Discount
    {
        private readonly HashSet<CategoryItems> categoryItems;
        private readonly decimal percentageDiscount;

        public CategoryCombinationDiscount(IEnumerable<IBasketItem> basketItems, string code, string name, IReadOnlyCollection<char> categoryCombinationOfInterest, int percentageDiscount)
        {
            Code = code;
            Name = name;
            BasketItems = basketItems;
            this.percentageDiscount = percentageDiscount;

            if (categoryCombinationOfInterest == null) return;

            categoryItems = new HashSet<CategoryItems>(categoryCombinationOfInterest.Count);
            foreach (var c in categoryCombinationOfInterest)
            {
                categoryItems.Add(new CategoryItems(c));
            }
        }

        public override long GetTotalDiscountInPence()
        {
            var numberOfMatchingCombinations = GetNoOfMatchedInstances();

            var totalDiscount = 0L;
            foreach (var categoryItem in categoryItems)
            {
                // Assumption: Discount the first x items encountered in each category
                // Improvement: This could be expanded to sort by price before discounting 
                var remainingItemsToDiscount = numberOfMatchingCombinations;

                foreach (var basketItem in categoryItem.BasketItems)
                {
                    if (remainingItemsToDiscount > basketItem.Quantity)
                    {
                        totalDiscount += Convert.ToInt64(basketItem.Product.GetPriceInPence() * basketItem.Quantity * percentageDiscount / 100);
                        remainingItemsToDiscount -= basketItem.Quantity;
                    }
                    else
                    {
                        totalDiscount += Convert.ToInt64(basketItem.Product.GetPriceInPence() * remainingItemsToDiscount * percentageDiscount / 100);
                        remainingItemsToDiscount = 0;
                    }

                    if (remainingItemsToDiscount <= 0)
                    {
                        break;
                    }
                }
            }

            return totalDiscount;
        }

        public override int GetNoOfMatchedInstances()
        {
            GroupMatchingCategoryItems();

            // The number of complete sets is the count of items in the smallest category group
            return categoryItems.Min(c => c.NoOfItemsInCategory());
        }

        private void GroupMatchingCategoryItems()
        {
            if (BasketItems == null) return;

            foreach (var basketItem in BasketItems)
            {
                var matchingCategory =
                    categoryItems.FirstOrDefault(m => m.Category.Equals(basketItem.Product.Category));

                matchingCategory?.BasketItems.Add(basketItem);
            }
        }

        private class CategoryItems : IEquatable<CategoryItems>
        {
            public char Category { get; private set; }
            public List<IBasketItem> BasketItems { get; private set; }

            public CategoryItems(char category)
            {
                Category = category;
                BasketItems = new List<IBasketItem>();
            }

            public int NoOfItemsInCategory()
            {
                return BasketItems.Sum(b => b.Quantity);
            }

            public bool Equals(CategoryItems other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Category == other.Category;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((CategoryItems)obj);
            }

            public override int GetHashCode()
            {
                return Category.GetHashCode();
            }
        }
    }
}