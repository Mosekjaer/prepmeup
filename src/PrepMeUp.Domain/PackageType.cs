namespace PrepMeUp.Domain;

public class PackageType
{
    private List<PackageContent> _contents = [];

    public Guid Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public PackageTypeStatus Status { get; private set; }
    public IReadOnlyList<PackageContent> Contents => _contents;

    private PackageType()
    {
    }

    public static PackageType Create(string name, IEnumerable<PackageContent> contents)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(contents);

        var contentList = contents.ToList();
        EnsureValidContents(contentList);

        return new PackageType
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Status = PackageTypeStatus.Active,
            _contents = contentList
        };
    }

    public void MarkReplaced()
    {
        if (Status == PackageTypeStatus.Replaced)
        {
            throw new InvalidOperationException("The package type is already replaced.");
        }

        Status = PackageTypeStatus.Replaced;
    }

    private static void EnsureValidContents(List<PackageContent> contents)
    {
        if (contents.Count == 0)
        {
            throw new ArgumentException("A package type needs at least one content.", nameof(contents));
        }

        if (contents.Select(content => content.Item.Id).Distinct().Count() != contents.Count)
        {
            throw new ArgumentException("An item can only appear once in a package type.", nameof(contents));
        }
    }
}
