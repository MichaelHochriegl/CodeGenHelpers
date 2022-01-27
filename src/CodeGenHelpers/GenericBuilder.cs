using Microsoft.CodeAnalysis;

namespace CodeGenHelpers
{
    public class GenericBuilder<T>
        where T : BuilderBase<T>
    {
        internal GenericBuilder(string name, T parent)
        {
            Name = name;
            Parent = parent;
        }

        public T Parent { get; }

        public string Type { get; internal set; }
        public string Name { get; internal set; }
        public string Constraint { get; internal set; }
        public bool HasConstraint { get; internal set; }

        public GenericBuilder<T> WithType(string typeName)
        {
            Type = typeName;
            return this;
        }

        public GenericBuilder<T> WithType(INamedTypeSymbol symbol)
        {
            Parent.AddNamespaceImport(symbol);
            return WithType(symbol.Name);
        }

        public GenericBuilder<T> WithConstraint() =>
            WithConstraint("default");

        public GenericBuilder<T> WithConstraint(string constraint)
        {
            Constraint = constraint;
            HasConstraint = true;
            return this;
        }

        public GenericBuilder<T> WithConstraint(object value) =>
            WithConstraint($"{value}");

        public GenericBuilder<T> WithNullDefault() =>
            WithConstraint("null");

        public string ToGenericNameString() => $"<{Name}>";
        public string ToGenericConstraintString() => $"where {Name} : {Constraint}";

        public override string ToString()
        {
            var output = $"<{Name}>";
            if (HasConstraint)
                output += $" where {Name} : {Constraint}";

            return output;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is ParameterBuilder<T> builder &&
                   builder.Type == Type && builder.Name == Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
