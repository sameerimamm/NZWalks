﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be a minimum of 3 characters")]
        [MaxLength(3,ErrorMessage ="Code has to be a max of 3 chars")]
        public string Code { get; set; }

        [Required]

        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
