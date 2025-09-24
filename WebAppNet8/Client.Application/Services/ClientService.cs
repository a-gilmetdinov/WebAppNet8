namespace WebAppNet8.Client.Application.Services;
using Domain.Entities;
using System.Collections.Concurrent;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client> GetClientById(long id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task AddClient(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClient(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClient(long id)
    {
        var client = await GetClientById(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(int addedCount, List<Client> duplicates)> AddClientsBatch(List<Client> clients)
    {
        var existingIds = await _context.Clients.Select(c => c.ClientId).ToListAsync();
        var duplicates = new List<Client>();
        var uniqueClients = clients.Where(c => !existingIds.Contains(c.ClientId)).ToList();

        var tasks = uniqueClients.Select(async client =>
        {
            await AddClient(client);
        });

        await Task.WhenAll(tasks);

        duplicates = clients.Where(c => existingIds.Contains(c.ClientId)).ToList();
        return (uniqueClients.Count, duplicates);
    }
}