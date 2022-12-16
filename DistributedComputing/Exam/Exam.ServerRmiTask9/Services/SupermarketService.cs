using Grpc.Core;
using SupermarketServ = Exam.Server.Supermarket;

namespace Exam.ServerRmiTask9.Services
{
    public class SupermarketService : Supermarket.SupermarketBase
    {
        private readonly ILogger<SupermarketService> _logger;
        private readonly SupermarketServ _supermarket;
        public SupermarketService(ILogger<SupermarketService> logger, SupermarketServ supermarket)
        {
            _logger = logger;
            _supermarket = supermarket;
        }

        public override Task<GetByNameResponse> GetByName(GetByNameRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetByNameResponse()
            {
                Ids = string.Join(',', _supermarket.GetByName(request.Name).Select(p => p.Id))
            });
        }

        public override Task<GetByNameBelowOrEqPriceResponse> GetByNameBelowOrEqPrice(GetByNameBelowOrEqPriceRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetByNameBelowOrEqPriceResponse()
            {
                Ids = string.Join(',', _supermarket.GetByNameBelowOrEqPrice(request.Name, request.Price).Select(p => p.Id))
            });
        }

        public override Task<GetOverdueResponse> GetOverdue(GetOverdueRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetOverdueResponse()
            {
                Ids = string.Join(',', _supermarket.GetOverdue(new DateTime(request.Year, request.Month, request.Day)).Select(p => p.Id))
            });
        }
    }
}