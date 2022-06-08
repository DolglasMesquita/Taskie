using System;
using System.ComponentModel.DataAnnotations;
using Taskie.Domain.Entities.Enums;

namespace Taskie.Domain.Entities
{
    public class TaskEntity : BaseEntity
    {
        [Required]
        [StringLength(80)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public PriorityEnum Priority { get; set; }
<<<<<<< HEAD
        public bool Finished { get; set; } = false;
        public bool FinishedInTime { get; set; }
        public DateTime Deadline { get; set; }
=======

        public DateTime? Finished { get; set; } = null;

        public bool? FinishedInTime { get; set; } = null;

        [Required]
        public DateTime Deadline { get; set; }

>>>>>>> develop
        public string UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
