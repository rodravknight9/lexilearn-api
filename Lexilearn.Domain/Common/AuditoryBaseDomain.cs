namespace Lexilearn.Domain.Common;

public class AuditoryBaseDomain : BaseDomainModel
{
    public DateTime? LastModifiedDate { get; set; }
    public int? LastModifiedBy { get; set; }
    public bool IsActive { get; set; }
}