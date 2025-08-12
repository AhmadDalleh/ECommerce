using ECommerce.Customers.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for updating customer password information
    /// Used when changing existing customer passwords
    /// </summary>
    public class UpdateCustomerPasswordDto
    {
        /// <summary>
        /// ID of the customer whose password is being updated
        /// Required: Must specify which customer to update
        /// </summary>
        [Required(ErrorMessage = "Customer ID is required")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// New password for the customer
        /// Required: Must provide a new password
        /// Business rule: Password should meet security requirements
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Current password for verification
        /// Required: Must verify current password before allowing change
        /// Business rule: Security measure to prevent unauthorized changes
        /// </summary>
        [Required(ErrorMessage = "Current password is required for verification")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Password format for the new password
        /// Required: Must specify how the password is stored
        /// </summary>
        [Required(ErrorMessage = "Password format is required")]
        public PasswordFormat PasswordFormatId { get; set; }

        /// <summary>
        /// Salt for password hashing (if applicable)
        /// Optional: Used for additional security in password hashing
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// When the password was updated
        /// Auto-set: Tracks when the password change occurred
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;
    }
}
