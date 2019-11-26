//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using Xunit;

//namespace Clubee.API.Tests.Entities
//{
//    public class EstablishmentTest
//    {
//        //[Fact]
//        //public void Should_create_entity_correctly()
//        //{
//        //    // Act.
//        //    Establishment establishment = new Establishment(
//        //        "Vitrola Bar",
//        //        "www.vitrolabar.com.br",
//        //        "www.vitrolabar.com.br",
//        //        "Melhor bar da cidade",
//        //        new GeoJson2DGeographicCoordinates(
//        //            -47.5541214, 
//        //            7.5425421
//        //        ),
//        //        string.Empty,
//        //        new[] 
//        //        { 
//        //            EstablishmentTypeEnum.Bar, 
//        //            EstablishmentTypeEnum.CasaShows 
//        //        },
//        //        new []
//        //        {
//        //            new Availability(
//        //                DayOfWeekEnum.Friday,
//        //                new TimeSpan(20, 0, 0),
//        //                new TimeSpan(4, 0, 0)
//        //            ),
//        //            new Availability(
//        //                DayOfWeekEnum.Saturday,
//        //                new TimeSpan(20, 0, 0),
//        //                new TimeSpan(5, 0, 0)
//        //            )
//        //        }
//        //    );

//        //    // Assert.
//        //    establishment.Name.Should().NotBeNullOrEmpty();
//        //    establishment.Availabilities.Should().HaveCount(2);
//        //    establishment.EstablishmentTypes.Should().HaveCount(2);
//        //}


//        [Fact]
//        public void a()
//        {
//            var datas = new[]
//            {
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 22, 22, 0, 0), // x
//                    DataFinal = new DateTime(2019, 11, 23, 10, 0, 0)
//                },
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 22, 8, 0, 0),
//                    DataFinal = new DateTime(2019, 11, 22, 22, 0, 0)
//                },
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 23, 20, 0, 0), // x
//                    DataFinal = new DateTime(2019, 11, 24, 4, 0, 0)
//                },
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 24, 20, 0 ,0),
//                    DataFinal = new DateTime(2019, 11, 25, 4,0,0)
//                }, 
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 22, 20, 0 ,0),
//                    DataFinal = new DateTime(2019, 11, 24, 10, 0 ,0)
//                },
//                new Duracao
//                {
//                    DataInicial = new DateTime(2019, 11, 23, 10, 0, 0),
//                    DataFinal = new DateTime(2019, 11, 23, 15, 0 , 0)
//                }
//            };

//            DateTime inicio = new DateTime(2019, 11, 23);
//            DateTime fim = new DateTime(2019, 11, 23, 23, 59, 59);

//            //Expression<Func<batata, bool>> filtro = data => (data.dataFinal >= inicio
//            //    && data.dataInicial <= inicio)
//            //    || (data.dataInicial <= fim
//            //        && data.dataFinal >= fim)
//            //    || (data.dataInicial >= inicio
//            //        && data.dataFinal <= fim);

//            //var b = datas.Where(filtro.Compile());

//            var b = datas.ApplyDateRange(inicio, fim);

//            Console.WriteLine(b);
//        }

//        public class Duracao
//        {
//            [InitialDate]
//            public DateTime DataInicial { get; set; }

//            [EndDate]
//            public DateTime DataFinal { get; set; }
//        }
//    }

//    public static class IEnumerableExtensions
//    {
//        public static IEnumerable<TResult> ApplyDateRange<TResult>(
//            this IEnumerable<TResult> items,
//            DateTime initialDate,
//            DateTime endDate
//            )
//        {
//            Type objectType = typeof(TResult);
//            ParameterExpression parameter = Expression.Parameter(objectType);

//            ConstantExpression initialDateConstant = Expression.Constant(initialDate);
//            ConstantExpression endDateConstant = Expression.Constant(endDate);

//            PropertyInfo initialDateProperty = objectType.GetProperties()
//                .FirstOrDefault(x => x.GetCustomAttribute<InitialDateAttribute>() != null);
//            PropertyInfo endDateProperty = objectType.GetProperties()
//                .FirstOrDefault(x => x.GetCustomAttribute<EndDateAttribute>() != null);

//            MemberExpression initialDateMember = Expression.MakeMemberAccess(parameter, initialDateProperty);
//            MemberExpression endDateMember = Expression.MakeMemberAccess(parameter, endDateProperty);

//            Expression baseExpression = Expression.Equal(
//                Expression.Constant(0), Expression.Constant(1));

//            // data.dataFinal >= inicio && data.dataInicial <= inicio
//            baseExpression = Expression.OrElse(
//                baseExpression,
//                Expression.AndAlso(
//                    Expression.MakeBinary(
//                        ExpressionType.GreaterThanOrEqual,
//                        endDateMember,
//                        initialDateConstant
//                    ),
//                    Expression.MakeBinary(
//                        ExpressionType.LessThanOrEqual,
//                        initialDateMember,
//                        initialDateConstant
//                    )
//                )
//            );

//            // data.dataInicial <= fim && data.dataFinal >= fim
//            baseExpression = Expression.OrElse(
//                baseExpression,
//                Expression.AndAlso(
//                    Expression.MakeBinary(
//                        ExpressionType.LessThanOrEqual,
//                        initialDateMember,
//                        endDateConstant
//                    ),
//                    Expression.MakeBinary(
//                        ExpressionType.GreaterThanOrEqual,
//                        endDateMember,
//                        endDateConstant
//                    )
//                )
//            );

//            // data.dataInicial >= inicio && data.dataFinal <= fim
//            baseExpression = Expression.OrElse(
//                baseExpression,
//                Expression.AndAlso(
//                    Expression.MakeBinary(
//                        ExpressionType.GreaterThanOrEqual,
//                        initialDateMember,
//                        initialDateConstant
//                    ),
//                    Expression.MakeBinary(
//                        ExpressionType.LessThanOrEqual,
//                        endDateMember,
//                        endDateConstant
//                    )
//                )
//            );

//            LambdaExpression lambdaExpression = Expression.Lambda(
//                baseExpression,
//                new[] { parameter }
//            );

//            Expression<Func<TResult, bool>> typedExpression = (Expression<Func<TResult, bool>>)lambdaExpression;

//            return items.Where(
//                typedExpression.Compile()
//            );
//        }
//    }

//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
//    public class InitialDateAttribute : Attribute { }

//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
//    public class EndDateAttribute : Attribute { }
//}
