using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamCloud.Product.Models.Client;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DreamCloud.Product.Models.DependencyInjection
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection ApplyModelValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<ProductToAdd>, ProductToAddValidator<ProductToAdd>>();

            return services;
        }
    }
}
