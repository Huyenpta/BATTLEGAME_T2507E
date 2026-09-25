using System.Net;
using BattleGameFunction.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;

namespace BattleGameFunction.Functions;

public class GetAssetsByPlayer
{
    private readonly BattleGameDbContext _context;

    public GetAssetsByPlayer(BattleGameDbContext context)
    {
        _context = context;
    }

    [Function("getassetsbyplayer")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "getassetsbyplayer")]
        HttpRequestData req)
    {
        
        var data = await _context.PlayerAssets
            .AsNoTracking()
            .OrderBy(x => x.Player.PlayerName)
            .Select(x => new
            {
                PlayerName = x.Player.PlayerName,
                Level = x.Player.Level,
                Age = x.Player.Age,
                AssetName = x.Asset.AssetName
            })
            .ToListAsync();

        
        var result = data.Select((x, index) => new
        {
            No = index + 1,
            x.PlayerName,
            x.Level,
            x.Age,
            x.AssetName
        });

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(result);

        return response;
    }
}