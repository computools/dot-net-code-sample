using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace *********.**********.BusinessServices
{
    public static class ExpressionHelper
    {
        private static readonly MethodInfo containsMethod;
        private static readonly MethodInfo startsWithMethod; 
        static ExpressionHelper()
        {
            containsMethod = typeof(string).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1);
            startsWithMethod = typeof(string).GetMethods().First(m => m.Name == "StartsWith" && m.GetParameters().Length == 1);
        }

        public static Expression<Func<T, bool>> AddContains<T>(this Expression<Func<T, string>> selector, string value)
        {
            var body = selector.GetBody().AsString();
            var x = Expression.Call(body, containsMethod, Expression.Constant(value));

            LambdaExpression e = Expression.Lambda(x, selector.Parameters.ToArray());
            return (Expression<Func<T, bool>>)e;
        }

        public static Expression<Func<T, bool>> AddStartsWith<T>(this Expression<Func<T, string>> selector, string value)
        {
            var body = selector.GetBody().AsString();
            var x = Expression.Call(body, startsWithMethod, Expression.Constant(value));

            LambdaExpression e = Expression.Lambda(x, selector.Parameters.ToArray());
            return (Expression<Func<T, bool>>)e;
        }

        private static Expression GetBody(this LambdaExpression expression)
        {
            Expression body;
            if (expression.Body is UnaryExpression)
                body = ((UnaryExpression)expression.Body).Operand;
            else
                body = expression.Body;

            return body;
        }

        private static Expression AsString(this Expression expression)
        {
            if (expression.Type == typeof(string))
                return expression;

            MethodInfo toString = typeof(SqlFunctions).GetMethods().First(m => m.Name == "StringConvert" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(double?));
            var cast = Expression.Convert(expression, typeof(double?));
            return Expression.Call(toString, cast);
        }

        public static IQueryable<TEntity> ContainsOrStartWithQuery<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, string>> selector, string search)
        {
            if (search.StartsWith("*"))
            {
                search = search.Substring(1);
                query = query.Where(selector.AddContains<TEntity>(search));
            }
            else
            {
                query = query.Where(selector.AddStartsWith<TEntity>(search));
            }
            return query;
        }

        public static Expression<Func<TEntity, int>> GetPrimaryKeySelector<TEntity>()
        {
            var property = typeof(TEntity).GetProperty("PrimaryKeySelector", BindingFlags.Static | BindingFlags.Public);
            return property.GetValue(null) as Expression<Func<TEntity, int>>;
        }

        public static Expression<Func<TEntity, bool>> Compare<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> selector, TProperty value)
        {
            var entity = selector.Parameters.First();
            var expression = Expression.Equal(selector.Body, Expression.Constant(value));
            return Expression.Lambda<Func<TEntity, bool>>(expression, entity);
        }

        public static Expression<Func<TEntity, bool>> PkFilterExpression<TEntity>(int id)
        {
            var pkSelector = GetPrimaryKeySelector<TEntity>();
            return Compare(pkSelector, id);
        }

        public static Expression<Func<T, bool>> GetInvertedBoolExpression<T>(string propertyName)
        {
            var instance = Expression.Parameter(typeof(T), "x");

            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Property(instance, propertyName)), instance);
        }

        public static void SetPropertyValue<T, P>(T target, Expression<Func<T, P>> memberLamda, P value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            var property = memberSelectorExpression?.Member as PropertyInfo;
            property?.SetValue(target, value, null);
        }

        public static void SetPropertyValue<T>(T target, string propertyName, object value)
        {
            typeof(T).GetProperty(propertyName).SetValue(target, value);
        }

        public static IQueryable<TEntity> CompareDates<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, DateTime?>> selector, string search)
        {
            if (search.Contains('>'))
            {
                var date = GetDate(search, '>');
                if (date.HasValue)
                {
                    query = query.Where(selector.GreaterThan<TEntity>(date.Value.Date));
                }

            }
            else if (search.Contains('<'))
            {
                var date = GetDate(search, '<');
                if (date.HasValue)
                {
                    query = query.Where(selector.LessThan<TEntity>(date.Value.Date));
                }
            }
            else
            {
                DateTime date;
                var parsed = DateTime.TryParse(search, out date);
                if (parsed)
                {
                    query = query.Where(selector.Equal<TEntity>(date.Date));
                }

            }
            return query;
        }

        private static Expression<Func<T, bool>> GreaterThan<T>(this Expression<Func<T, DateTime?>> selector, DateTime value)
        {
            var convertedExpression = Expression.Convert(selector.GetBody(), typeof(DateTime));
            var expression = Expression.GreaterThan(convertedExpression, Expression.Constant(value, typeof(DateTime)));

            return GetExpressionFromBinary(expression, selector);
        }

        private static Expression<Func<T, bool>> LessThan<T>(this Expression<Func<T, DateTime?>> selector, DateTime value)
        {
            var convertedExpression = Expression.Convert(selector.GetBody(), typeof(DateTime));
            var expression = Expression.LessThan(convertedExpression, Expression.Constant(value, typeof(DateTime)));

            return GetExpressionFromBinary(expression, selector);
        }

        private static Expression<Func<T, bool>> Equal<T>(this Expression<Func<T, DateTime?>> selector, DateTime value)
        {
            var convertedExpression = Expression.Convert(selector.GetBody(), typeof(DateTime));
            var expression = Expression.Equal(convertedExpression, Expression.Constant(value, typeof(DateTime)));
            return GetExpressionFromBinary(expression, selector);
        }

        private static Expression<Func<T, bool>> GetExpressionFromBinary<T>(BinaryExpression expression, Expression<Func<T, DateTime?>> selector)
        {
            LambdaExpression e = Expression.Lambda(expression, selector.Parameters.ToArray());
            return (Expression<Func<T, bool>>)e;
        }


        private static DateTime? GetDate(string value, char splitter)
        {
            DateTime date;
            var result = value.Split(splitter)[1];
            var parsed = DateTime.TryParse(result, out date);
            if (parsed)
            {
                return date;
            }
            return null;
        }
    }
}