namespace NZWalks.UI.Models;

public class LoginResponseDTO
{
    public string JwtToken { get; set; } = string.Empty;
}

public class ImageResponseDTO
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? FileDescription { get; set; }
    public string FilePath { get; set; } = string.Empty;
}
