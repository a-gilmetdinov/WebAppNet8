namespace WebAppNet8.Client.Application.Services.Interfaces;
using Domain.Entities;
public interface IClientService
{
    Task<Client> GetClientById(long id);
    Task<IEnumerable<Client>> GetAllClients();
    Task AddClient(Client client);
    Task UpdateClient(Client client);
    Task DeleteClient(long id);
    Task<(int addedCount, List<Client> duplicates)> AddClientsBatch(List<Client> clients);
}