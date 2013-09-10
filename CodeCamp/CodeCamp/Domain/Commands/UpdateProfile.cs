using System;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
    public class UpdateProfile : Command<Result> {
        public UpdateProfile() {}

        public UpdateProfile(User user) {}

        protected override Result Execute() {
            throw new NotImplementedException();
        }
    }
}