using System.Reflection;
using YACP.Models.Exceptions;

namespace YACP.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Determine if a property's type is string, int or double
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsValueProperty(this PropertyInfo? propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new InvalidPropertyException("Property cannot be null");
            }

            return propertyInfo.IsDoubleProperty() || propertyInfo.IsIntProperty() || propertyInfo.IsStringProperty();
        }

        internal static bool IsDoubleProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return type == typeof(double) || type == typeof(double?);
        }

        internal static bool IsIntProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return type == typeof(int) || type == typeof(int?);
        }

        internal static bool IsStringProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            return type == typeof(string);
        }

        /// <summary>
        /// Determine if a property's type is bool
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPropertyException"></exception>
        public static bool IsOptionProperty(this PropertyInfo? propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new InvalidPropertyException("Property cannot be null");
            }

            var type = propertyInfo.PropertyType;

            return type == typeof(bool);
        }

        /// <summary>
        /// Determine if a property's type is IEnumerable, object[], List
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPropertyException"></exception>
        public static bool IsSequenceProperty(this PropertyInfo? propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new InvalidPropertyException("Property cannot be null");
            }

            var type = propertyInfo.PropertyType;

            return propertyInfo.IsEnumerableProperty() || propertyInfo.IsListProperty() ||
                   propertyInfo.IsArrayProperty();
        }

        private static bool IsEnumerableProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.IsStringEnumerableProperty() || propertyInfo.IsDoubleEnumerableProperty() ||
                   propertyInfo.IsIntEnumerableProperty();
        }

        internal static bool IsIntEnumerableProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(IEnumerable<int>);
        }

        internal static bool IsDoubleEnumerableProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(IEnumerable<double>);
        }

        internal static bool IsStringEnumerableProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(IEnumerable<string>);
        }

        private static bool IsListProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.IsStringListProperty() || propertyInfo.IsIntListProperty() ||
                   propertyInfo.IsDoubleListProperty();
        }

        internal static bool IsStringListProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(List<string>);
        }

        internal static bool IsIntListProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(List<int>);
        }

        internal static bool IsDoubleListProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(List<double>);
        }

        private static bool IsArrayProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.IsStringArrayProperty()
                   || propertyInfo.IsDoubleArrayProperty()
                   || propertyInfo.IsIntArrayProperty();
        }

        internal static bool IsStringArrayProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(string[]);
        }

        internal static bool IsIntArrayProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(int[]);
        }

        internal static bool IsDoubleArrayProperty(this PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type == typeof(double[]);
        }
    }
}