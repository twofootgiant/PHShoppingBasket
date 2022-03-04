namespace ShoppingBasket
{
    public class Product : IProduct
    {
        // Note : these strings can be moved to config file to support multiple locals etc
        public const string CodeValueError =
            "Invalid Code specified. Code must be a 2 character string \"<char><int>\".";
        public const string NameValueError = "Invalid Name specified. A non-null and non-empty name must be specified.";
        public const string PriceValueError = "Invalid Price specified. Price must be a positive pence value.";
        public const string DuplicateProductError =
            "Unable to add product. A product with the same code '{0}' already exists.";

        public char Category { get; }
        private readonly short categoryIndex;
        private readonly string name;
        private readonly int priceInPence;

        public static IProduct Create(string code, string name, int priceInPence, out string errorMsg)
        {
            errorMsg = string.Empty;
            var categoryChar = default(char);
            short categoryIndexVal = 0;

            if (string.IsNullOrEmpty(code) || code.Length != 2 )
            {
                errorMsg = CodeValueError;
            }

            if(string.IsNullOrEmpty(errorMsg) && !char.TryParse(code[0].ToString(), out categoryChar) | !char.IsLetter(code[0]) |
                !short.TryParse(code[1].ToString(), out categoryIndexVal))
            {
                errorMsg = CodeValueError;
            }

            if (string.IsNullOrEmpty(errorMsg) && string.IsNullOrEmpty(name))
            {
                errorMsg = NameValueError;
            }

            // Assumption : We support freebies, maybe as part of an offer
            if (string.IsNullOrEmpty(errorMsg) && priceInPence < 0)
            {
                errorMsg = PriceValueError;
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return null;
            }

            var newProduct = new Product(categoryChar, categoryIndexVal, name, priceInPence);

            // Improvement: Could be worth checking for different products that have the same code if the data in the DB may not be clean

            return newProduct;
        }

        private Product(char category, short categoryIndex, string name, int priceInPence)
        {
            Category = category;
            this.categoryIndex = categoryIndex;
            this.name = name;
            this.priceInPence = priceInPence;
        }

        public string GetCode()
        {
            return $"{Category}{categoryIndex}";
        }

        public string GetName()
        {
            return name;
        }

        public int GetPriceInPence()
        {
            return priceInPence;
        }

        public bool Equals(IProduct other)
        {
            return other != null && GetCode() == other.GetCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Category.GetHashCode() * 397) ^ categoryIndex.GetHashCode();
            }
        }
    }
}
