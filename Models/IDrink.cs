
public interface IDrink
{
    Guid? Id { get; set; }
    string Name { get; set; }
    string Type { get; set; }
    decimal Price { get; set; }
}