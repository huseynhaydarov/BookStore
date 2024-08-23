﻿namespace ReviewService.Application.Common.DTOs
{
    public class CreateReviewDto
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}