using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for editing user's role.
    /// </summary>
    public class EditUserRoleViewModel
    {

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string RoleName { get; set; }

    }
}