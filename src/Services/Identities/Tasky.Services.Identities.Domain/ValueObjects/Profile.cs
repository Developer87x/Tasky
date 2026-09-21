using Tasky.BuildingBlocks.Core.Models;

namespace Tasky.Services.Identities.Domain.ValueObjects;

public class Profile :ValueObject
{
    public Profile(string? firstName,string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        return
        [
            FirstName!,
            LastName!
        ];
    }
}