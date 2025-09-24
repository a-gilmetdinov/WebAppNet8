namespace WebAppNet8.Client.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    // Получение клиента по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(long id)
    {
        var client = await _clientService.GetClientById(id);
        if (client == null)
            return NotFound();
        return Ok(client);
    }

    // Получение всех клиентов
    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _clientService.GetAllClients();
        return Ok(clients);
    }

    // Создание нового клиента
    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] Client client)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _clientService.AddClient(client);
        return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
    }

    // Обновление клиента
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(long id, [FromBody] Client client)
    {
        if (id != client.ClientId)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        await _clientService.UpdateClient(client);
        return NoContent();
    }

    // Удаление клиента
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(long id)
    {
        await _clientService.DeleteClient(id);
        return NoContent();
    }

    // Массовое добавление клиентов
    [HttpPost("batch")]
    public async Task<IActionResult> AddClientsBatch([FromBody] List<Client> clients)
    {
        if (clients == null || clients.Count < 10)
            return BadRequest("Необходимо отправить минимум 10 клиентов");

        var (addedCount, duplicates) = await _clientService.AddClientsBatch(clients);
        return Ok(new 
        {
            addedCount = addedCount,
            duplicates = duplicates
        });
    }
}
