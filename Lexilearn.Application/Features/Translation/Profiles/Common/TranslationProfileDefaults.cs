using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;

namespace Lexilearn.Application.Features.Translation.Profiles.Common;

public static class TranslationProfileDefaults
{
    public static async Task ClearAsync(
        IAsyncRepository<TranslationProfile> repository,
        int userId,
        int? exceptId)
    {
        var defaults = await repository.GetMany(p => p.UserId == userId && p.IsDefault && p.IsActive);
        foreach (var other in defaults)
        {
            if (exceptId.HasValue && other.Id == exceptId.Value)
                continue;

            other.IsDefault = false;
            other.LastModifiedBy = userId;
            await repository.UpdateAsync(other);
        }
    }
}
