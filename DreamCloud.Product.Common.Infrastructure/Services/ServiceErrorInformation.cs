using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCloud.Product.Common.Infrastructure.Services
{
    public class ServiceErrorInformation
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
