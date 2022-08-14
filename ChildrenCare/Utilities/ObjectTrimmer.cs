namespace ChildrenCare.Utilities
{
    /// <summary>
    /// Để trim tất cả các string trong object
    /// </summary>
    public static class ObjectTrimmer
    {
        public static object TrimObject(object obj)
        {
            var stringProperties = obj.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof (string));

            foreach (var stringProperty in stringProperties)
            {
                var currentValue = (string) stringProperty.GetValue(obj, null);
                if (currentValue != null) 
                    stringProperty.SetValue(obj, currentValue.Trim(), null);
            }

            return obj;
        }
    }
}