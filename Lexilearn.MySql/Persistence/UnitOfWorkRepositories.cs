using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.MySql.Repository;

namespace Lexilearn.MySql.Persistence
{
    public partial class UnitOfWork
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly IPracticeSessionRepository _practiceSessionRepository;
        private readonly IPracticeSessionCardsRepository _practiceSessionCardsRepository;

        public ICardRepository CardRepository => _cardRepository ?? new CardRepository(_context);
        public IDeckRepository DeckRepository => _deckRepository ?? new DeckRepository(_context);
        public IPracticeSessionRepository PracticeSessionRepository
            => _practiceSessionRepository ?? new PracticeSessionRepository(_context);

        public IPracticeSessionCardsRepository PracticeSessionCardsRepository
            => _practiceSessionCardsRepository ?? new PracticeSessionCardsRepository(_context);
    }
}
