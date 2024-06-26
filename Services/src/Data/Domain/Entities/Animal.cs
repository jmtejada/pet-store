﻿using PetStore.Services.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Services.Domain.Entities
{
    public class Animal
    {
        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublicId { get; set; }

        public int Age { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Breed { get; set; }
        public string? Sex { get; set; }
        public int Weight { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}