

namespace WebApplication1.Types{
    public class DrinkType : ObjectType<Drink>
    {
        protected override void Configure(IObjectTypeDescriptor<Drink> descriptor)
        {
            descriptor.Field(d => d.Id).Type<IdType>();
            descriptor.Field(d => d.Name).Type<NonNullType<StringType>>();
            descriptor.Field(d => d.Type).Type<NonNullType<StringType>>();
            descriptor.Field(d => d.Price).Type<NonNullType<DecimalType>>();
        }
    }

}