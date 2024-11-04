using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Api.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
       [DataType(dataType: DataType.EmailAddress)]
        public string UserName { get; set; }
         [Required]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
    }
}