using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDiSpotShop.Shared
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Sucess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
