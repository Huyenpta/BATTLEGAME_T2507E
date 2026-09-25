using System.Net;
using System.Text.Json;
using BattleGameFunction.Data;
using BattleGameFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace BattleGameFunction.Functions;

public class RegisterPlayer
{
    private readonly BattleGameDbContext _context;

    public RegisterPlayer(BattleGameDbContext context)
    {
        _context = context;
    }

    [Function("registerplayer")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "registerplayer")]
        HttpRequestData req)
    {
        var player = await JsonSerializer.DeserializeAsync<Player>(
            req.Body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (player == null)
        {
            var badResponse =
                req.CreateResponse(HttpStatusCode.BadRequest);

            await badResponse.WriteStringAsync(
                "Invalid player data");

            return badResponse;
        }

        player.PlayerId = Guid.NewGuid();

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        var response =
            req.CreateResponse(HttpStatusCode.Created);

        await response.WriteAsJsonAsync(player);

        return response;
    }
}