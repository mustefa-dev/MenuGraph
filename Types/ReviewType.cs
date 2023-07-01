// namespace WebApplication1.Types;
//
// public class ReviewType : ObjectType<IReview>
// {
//     protected override void Configure(IObjectTypeDescriptor<IReview> descriptor)
//     {
//         descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
//         descriptor.Field(r => r.DrinkId).Type<NonNullType<IdType>>();
//         descriptor.Field(r => r.Rating).Type<NonNullType<IntType>>();
//         descriptor.Field(r => r.Comment).Type<StringType>();
//         descriptor.Field(r => r.Drink).Ignore();
//     }
// }
//
