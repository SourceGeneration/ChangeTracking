namespace SourceGeneration.ChangeTracking;

public abstract class PropertyModifiersTestObjectBase
{
    public virtual int OverrideProperty { get; set; }
    public virtual int SealedProperty { get; set; }
    public virtual required int SealedRequiredProperty { get; set; }
    public int NewProperty { get; set; }

}
[ChangeTracking]
public partial class PropertyModifiersTestObject : PropertyModifiersTestObjectBase
{
    public virtual partial int VirtualProperty { get; set; }

    public override partial int OverrideProperty { get; set; }
    public sealed override partial int SealedProperty { get; set; }
    public sealed override required partial int SealedRequiredProperty { get; set; }

    internal partial int InternalProperty { get; set; }
    protected partial int ProtectedProperty { get; set; }
    protected internal partial int ProtectedInternalProperty { get; set; }
    internal protected partial int InternalProtectedProperty { get; set; }

    private partial int PrivateProperty { get; set; }

    public partial string InternalSetProperty { get; internal set; }
    public partial string PrivateSetProperty { get; private set; }
    public partial string ProtectedSetProperty { get; protected set; }
    public partial string ProtectedInternalSetProperty { get; protected internal set; }

    public partial string ReadOnlyProperty { get; }
    public partial string WriteOnlyProperty { set; }
    public partial string InternalGetProperty { internal get; set; }
    public partial string PrivateGetProperty { private get; set; }
    public partial string ProtectedGetProperty { protected get; set; }
    public partial string ProtectedInternalGetProperty { protected internal get; set; }


    //public new partial int NewProperty { get; set; }

}