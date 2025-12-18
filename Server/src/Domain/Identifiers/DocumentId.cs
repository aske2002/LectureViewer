namespace backend.Domain.Identifiers;

public class DocumentPageId : StronglyTypedId<DocumentPageId>
{
    public DocumentPageId(Guid value) : base(value) {}
}
public class DocumentId : StronglyTypedId<DocumentId>
{
    public DocumentId(Guid value) : base(value) {}
}


public class DocumentKeywordId : StronglyTypedId<DocumentKeywordId>
{
    public DocumentKeywordId(Guid value) : base(value) {}
}
