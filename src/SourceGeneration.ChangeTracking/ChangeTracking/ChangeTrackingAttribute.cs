using System;

namespace SourceGeneration.ChangeTracking;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ChangeTrackingAttribute : Attribute { }
