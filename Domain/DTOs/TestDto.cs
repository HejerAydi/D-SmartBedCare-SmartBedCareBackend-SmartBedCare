using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TestDTO
    {
        [Key]
        [Column("id")]
        public int? id { get; set; }

        [Column("designation")]
        public string? designation { get; set; }
    }
}
