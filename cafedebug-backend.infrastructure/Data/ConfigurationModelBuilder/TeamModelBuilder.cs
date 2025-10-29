﻿using cafedebug_backend.domain.Podcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Data.ConfigurationModelBuilder;

public class TeamModelBuilder : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(b => b.UrlGitHub)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(b => b.UrlInstagram)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(b => b.UrlLinkedin)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(b => b.UrlImage)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(b => b.Job)
            .IsRequired()
            .HasColumnType("varchar(50)");
    }
}