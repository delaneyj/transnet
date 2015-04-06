namespace TransNET.Types
{
    public struct Symbol
    {
        internal readonly string name;

        public Symbol(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return $"${name}";
        }
    }
}