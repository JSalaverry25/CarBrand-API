namespace CarBrand.Application
{
    public class CarBrandNameAlreadyExistsException : Exception
    {
        public CarBrandNameAlreadyExistsException(string name)
            : base($"A car brand named '{name}' already exists.")
        {
        }
    }
}
