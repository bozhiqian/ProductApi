using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCloud.Product.Api.Data
{
    public class ErrorsList : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value) => Add(new KeyValuePair<string, string>(key, value));
    }
}
