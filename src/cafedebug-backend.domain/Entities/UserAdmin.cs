﻿using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Entities
{
    /// <summary>
    /// Entity user admin application
    /// </summary>
    public class UserAdmin : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string HashedPassword { get; set; }
    }
}
