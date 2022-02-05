﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Taskie.Domain.Entities
{
    public class UserEntity : IdentityUser
    {
        public bool Active { get; set; } = true;
        public DateTime LastLogin { get; set; }
        public int AvatarId { get; set; }
        public AvatarEntity Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        public DateTime? UpdatedAt { get; set; }

    }
}
