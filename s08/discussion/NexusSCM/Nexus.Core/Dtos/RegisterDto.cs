﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Core.Dtos
{
   public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
            [Required]
         public string Password { get; set; }
    }
}
