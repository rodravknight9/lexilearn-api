using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Domain;
using Mapster;

namespace Lexilearn.Application.Mappings
{
    public class MappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateDeckCommand, Deck>();

        }
    }
}
