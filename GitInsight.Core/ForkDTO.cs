namespace GitInsight.Core;

public record ForkDTO(string name, string html_url, OwnerDTO owner);

public record OwnerDTO(string login, string avatar_url);