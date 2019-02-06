using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Extensions;

namespace TravelApp.Models
{
    public class User:ObservableObject, IDataErrorInfo
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        public string PhotoLink { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<Trip> Trips { get; set; }

        public string SaltValue { get; set; }
        public string HashValue { get; set; }

        public string Error => throw new NotImplementedException();
        public string this[string columnName] => this.Validate(columnName);
    }
}
