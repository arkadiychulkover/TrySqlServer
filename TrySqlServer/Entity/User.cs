using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySqlServer.Entity
{
    public class User
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Email: {Email}";
        }
    }
}
