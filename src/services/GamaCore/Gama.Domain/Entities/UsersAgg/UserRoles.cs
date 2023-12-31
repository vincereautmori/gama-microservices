﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Gama.Domain.Entities.UsersAgg
{
    public class UserRoles
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [NotMapped]
        public Role? Role { get; set; }

        [NotMapped]
        public User? User { get; set; }
    }
}
