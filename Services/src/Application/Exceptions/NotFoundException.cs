namespace PetStore.Services.Application.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }
}