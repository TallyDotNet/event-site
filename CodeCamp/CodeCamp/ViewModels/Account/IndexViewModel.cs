using System.Collections.Generic;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;

namespace CodeCamp.ViewModels.Account {
    public class IndexViewModel : UpdateAccount {
        public IEnumerable<Session> SubmittedSessions { get; set; }
    }
}