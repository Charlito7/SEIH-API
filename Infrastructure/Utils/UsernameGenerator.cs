namespace Infrastructure.Utils;

public static class UsernameGenerator
{
    public static string CreateUsername(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name must not be empty.");

        var random = new Random();
        var digits = random.Next(0, 999999).ToString("D6"); // Always 6 digits (with leading zeros if needed)

        var firstInitial_f = char.ToLower(firstName[0]);
        var firstInitial_l = char.ToLower(lastName[0]);

        return $"{firstInitial_f}{firstInitial_l}{digits}";
    }
}