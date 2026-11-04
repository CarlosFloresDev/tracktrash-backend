namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class JwtIdentityOptions
{
    public const string SectionName = "Authentication";

    public bool Demo { get; init; }
    public string Authority { get; init; } = "";
    public string Audience { get; init; } = "authenticated";
    public string RoleMetadataClaim { get; init; } = "app_metadata";
    public string RoleProperty { get; init; } = "tracktrash_role";
    public int RefreshIntervalMinutes { get; init; } = 30;
}
