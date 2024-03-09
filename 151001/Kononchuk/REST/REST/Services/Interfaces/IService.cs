namespace REST.Services.Interfaces;

public interface IService<TRequest, TResponse>
{
    TResponse? Create(TRequest dto);
    
    TResponse? GetById(long id);
    List<TResponse> GetAll();

    TResponse? Update(long id, TRequest dto);

    bool Delete(long id);
}