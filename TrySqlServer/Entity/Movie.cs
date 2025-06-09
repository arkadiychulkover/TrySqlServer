using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySqlServer.Entity
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ReleaseYear { get; set; } = 2025;
        public int UserId { get; set; }
        public User Users { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Description: {Description}, ReleaseYear: {ReleaseYear}";
        }
    }
}
