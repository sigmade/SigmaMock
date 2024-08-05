using System.Linq.Expressions;

namespace TestExpression;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Program
{
    public static void Main()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Price = 10.5m },
            new Product { Id = 2, Name = "Product2", Price = 20.0m },
            new Product { Id = 3, Name = "Product3", Price = 30.0m },
        };

        // Фильтр по цене
        var filteredProducts = FilterProducts(products, "Price", 20.0m);

        foreach (var product in filteredProducts)
        {
            Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
        }
    }

    public static IEnumerable<Product> FilterProducts(IEnumerable<Product> products, string propertyName, object value)
    {
        // Создаем параметр "p" для лямбды p => p.Property
        var parameter = Expression.Parameter(typeof(Product), "p");

        // Создаем доступ к свойству, например p.Price
        var property = Expression.Property(parameter, propertyName);

        // Создаем константу для сравнения
        var constant = Expression.Constant(value);

        // Создаем выражение сравнения, например p.Price > value
        var comparison = Expression.GreaterThan(property, constant);

        // Создаем лямбду p => p.Price > value
        var lambda = Expression.Lambda<Func<Product, bool>>(comparison, parameter);

        // Применяем лямбду для фильтрации
        return products.AsQueryable().Where(lambda).ToList();
    }
}