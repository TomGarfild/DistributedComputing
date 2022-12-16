using Grpc.Core;

namespace Lab8_b.Server.Services
{
    public class StudioService : Studio.StudioBase
    {
        private readonly ILogger<StudioService> _logger;
        private readonly Lab7_b.Studio _studio;
        public StudioService(ILogger<StudioService> logger, Lab7_b.Studio studio)
        {
            _logger = logger;
            _studio = studio;
            _studio.Start();
        }

        public override Task<AddArtistReply> AddArtist(AddArtistRequest request, ServerCallContext context)
        {
            var id = _studio.AddArtist(request.Name);
            return Task.FromResult(new AddArtistReply
            {
                Id = id.ToString()
            });
        }

        public override Task<GetArtistsReply> GetArtists(GetArtistsRequest request, ServerCallContext context)
        {
            var artists = _studio.GetAllArtists().Select(a => a.Id);
            return Task.FromResult(new GetArtistsReply
            {
                Ids = string.Join(',', artists)
            });
        }

        public override Task<DeleteArtistReply> DeleteArtist(DeleteArtistRequest request, ServerCallContext context)
        {
            _studio.DeleteArtist(Guid.Parse(request.Id));
            return Task.FromResult(new DeleteArtistReply
            {
                Message = "ok"
            });
        }

        public override Task<UpdateArtistReply> UpdateArtist(UpdateArtistRequest request, ServerCallContext context)
        {
            _studio.UpdateArtist(Guid.Parse(request.Id), request.Name);
            return Task.FromResult(new UpdateArtistReply
            {
                Message = "ok"
            });
        }

        public override Task<AddAlbumReply> AddAlbum(AddAlbumRequest request, ServerCallContext context)
        {
            var id = _studio.AddAlbum(request.Name, request.Genre, request.Year, Guid.Parse(request.ArtistId));
            return Task.FromResult(new AddAlbumReply
            {
                Id = id.ToString()
            });
        }

        public override Task<CountAlbumsByArtistReply> CountAlbums(CountAlbumsByArtistRequest request, ServerCallContext context)
        {
            var count = _studio.CountAlbumsForArtist(Guid.Parse(request.ArtistId));
            return Task.FromResult(new CountAlbumsByArtistReply
            {
                Count = count
            });
        }

        public override Task<GetAlbumsByArtistReply> GetAlbumsByArtist(GetAlbumsByArtistRequest request, ServerCallContext context)
        {
            var albumIds = _studio.GetAlbumsForArtist(Guid.Parse(request.ArtistId)).Select(a => a.Id);
            return Task.FromResult(new GetAlbumsByArtistReply
            {
                Ids = string.Join(',', albumIds)
            });
        }
    }
}