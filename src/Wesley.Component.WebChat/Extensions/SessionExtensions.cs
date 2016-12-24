using Microsoft.AspNetCore.Http;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Extensions
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            T result = default(T);
            byte[] byteArray = null;
            if (session.TryGetValue(key, out byteArray))
            {
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    result = Serializer.Deserialize<T>(memoryStream);
                }
            }
            return result;
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(memoryStream, value);
                    byte[] byteArray = memoryStream.ToArray();
                    session.Set(key, byteArray);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
