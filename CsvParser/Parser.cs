using System.Text;

namespace CsvParser;
public class Parser
{
    public List<T> ParseToDto<T>(Stream stream) where T : class, new()
    {
        List<T> result = new List<T>();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("windows-1251")))
        {
            while (!sr.EndOfStream)
            {
                var row = sr.ReadLine();
                var tables = row.Split(';', ',');
                T dto = new T();
                var props = dto.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var attr = (IndexAttribute)prop.GetCustomAttributes(typeof(IndexAttribute), true)[0];
                    var value = tables[attr.Index].Split('=')[1].Trim('"');
                    var typedValue = Convert.ChangeType(value, prop.PropertyType);

                    prop.SetValue(dto, typedValue);
                }

                result.Add(dto);
            }
        }

        return result;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
class IndexAttribute : Attribute
{
    public int Index { get; private set; }

    public IndexAttribute(int value)
    {
        Index = value;
    }
}