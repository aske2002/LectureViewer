
public static class MultiPartFormContentExtensions
{
    public static MultipartFormDataContent ToMultipart<T>(this T obj, MultipartFormDataContent? form = null, bool camelCase = true)
    {
        form ??= new MultipartFormDataContent();
        var props = typeof(T).GetProperties();

        foreach (var p in props)
        {
            var value = p.GetValue(obj);
            if (value != null)
            {
                var name = camelCase ? char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1) : p.Name;
                form.Add(new StringContent(value.ToString() ?? ""), name);
            }
        }

        return form;
    }
}