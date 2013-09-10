using System.Collections.Generic;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;

namespace CodeCamp.ViewModels.Account {
    public class IndexViewModel : UpdateProfile {
        public IEnumerable<Session> SubmittedSessions { get; set; }
    }
}