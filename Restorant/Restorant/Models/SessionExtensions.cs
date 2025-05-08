using System.Text.Json;
namespace Restorant.Models
{
    public static class SessionExtensions
    {
      public static void Set<T>(this ISession session,string key, T value) 
      {
            session.SetString(key, JsonSerializer.Serialize(value));
      }
      public static T Get<T>(this ISession session, string Key)
        {
            var json = session.GetString(Key);
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            else
            {
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}
