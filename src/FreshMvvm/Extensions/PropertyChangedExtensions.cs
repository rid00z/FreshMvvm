using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FreshMvvm.Extensions
{
    public static class PropertyChangedExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <param name="properties"></param>
        public static void WhenAny<T, TProperty>(this T source, Action<string> action, params Expression<Func<T, TProperty>>[] properties) where T : INotifyPropertyChanged
        {
            Expression<Func<T, TProperty>>[] propertiesCopy = properties;

            Dictionary<string, Func<T, TProperty>> propertyNames = propertiesCopy
                .ToDictionary(expression => expression.GetPropertyInfo().Name, expression => expression.Compile());

            source.PropertyChanged += (s, e) =>
            {
                if (propertyNames.ContainsKey(e.PropertyName))
                {
                    action(e.PropertyName);
                }
            };
        }

        /// <summary>
        /// Gets property information for the specified <paramref name="property"/> expression.
        /// </summary>
        /// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
        /// <typeparam name="TValue">Type of the property's value.</typeparam>
        /// <param name="property">The expression from which to retrieve the property information.</param>
        /// <returns>Property information for the specified expression.</returns>
        /// <exception cref="ArgumentException">The expression is not understood.</exception>
        public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var body = property.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Expression is not a property", nameof(property));

            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("Expression is not a property", nameof(property));

            return propertyInfo;
        }
    }
}

