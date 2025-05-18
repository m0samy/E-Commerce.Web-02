﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand)
                   .WithMany()
                   .HasForeignKey(p => p.BrandId);

            builder.HasOne(P => P.ProductType)
                    .WithMany()
                    .HasForeignKey(P => P.TypeId);

            builder.Property(P => P.Price)
                    .HasColumnType("decimal(10,2)");
        }
    }
}
