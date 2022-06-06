﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Taskie.Domain.Dto.Task
{
    public class TaskUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "Título da tarefa deve ter no máximo {1} caracteres.")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
