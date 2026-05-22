namespace SEIHTransfert.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetHospitalId(this HttpContext context)
    {
        if (!context.Items.TryGetValue("HospitalId", out var value))
            throw new UnauthorizedAccessException("HospitalId missing");

        if (value is not Guid hospitalId)
            throw new UnauthorizedAccessException("Invalid HospitalId");

        return hospitalId;
    }
}