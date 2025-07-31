using System.ComponentModel.DataAnnotations;

public class QuenMatKhauViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string MaOTP { get; set; }
}
