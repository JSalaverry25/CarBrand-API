namespace CarBrand.Application
{
    public class DuplicateCarBrandException : Exception
    {
        public DuplicateCarBrandException(string name)
            : base($"A car brand named '{name}' already exists.")
        {
        }
    }
}
