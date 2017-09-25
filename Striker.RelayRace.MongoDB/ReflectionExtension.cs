using System.Reflection;

namespace Striker.RelayRace.MongoDB
{
    internal static class ReflectionExtensions
    {
        public static T GetPropertyValue<T>(this object source, string propertyName)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            return (T)propertyInfo.GetValue(source);
        }

        public static T GetFieldValue<T>(this object source, string fieldName)
        {
            var fieldInfo = source.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            return (T)fieldInfo.GetValue(source);
        }

        public static void SetPropertyValue(this object source, string propertyName, object propertyValue)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(source, propertyValue);
            }
        }

        public static void SetFieldValue(this object source, string fieldName, object fieldValue)
        {
            var fieldInfo = source.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(source, fieldValue);
            }
        }
    }
}
