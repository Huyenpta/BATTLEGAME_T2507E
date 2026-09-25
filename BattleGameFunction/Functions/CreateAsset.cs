using System.Net;
using System.Text.Json;
using BattleGameFunction.Data;
using BattleGameFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace BattleGameFunction.Functions;

public class CreateAsset
{
    private readonly BattleGameDbContext _context;

    public CreateAsset(BattleGameDbContext context)
    {
        _context = context;
    }

    [Function("createasset")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "createasset")]
        HttpRequestData req)
    {
        var asset = await JsonSerializer.DeserializeAsync<Asset>(
            req.Body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (asset == null)
        {
            var badResponse =
                req.CreateResponse(HttpStatusCode.BadRequest);

            await badResponse.WriteStringAsync("Invalid asset data");

            return badResponse;
        }

        asset.AssetId = Guid.NewGuid();

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        var response =
            req.CreateResponse(HttpStatusCode.Created);

        await response.WriteAsJsonAsync(asset);

        return response;
    }
}