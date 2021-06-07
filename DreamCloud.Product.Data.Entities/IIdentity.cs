using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCloud.Product.Data.Entities
{
    public interface IIdentity<out T>
    {
        T Id { get; }
    }
}
