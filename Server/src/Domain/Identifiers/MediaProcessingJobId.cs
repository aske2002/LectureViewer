namespace backend.Domain.Identifiers;

public class MediaProcessingJobId : StronglyTypedId<MediaProcessingJobId>
{
    public MediaProcessingJobId(Guid value) : base(value) {}
}
