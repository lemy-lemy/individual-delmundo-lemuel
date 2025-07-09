using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Core
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set;}

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
