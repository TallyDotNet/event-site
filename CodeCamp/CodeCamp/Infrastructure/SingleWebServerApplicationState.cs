﻿using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using CodeCamp.Domain;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using CodeCamp.Domain.Queries;
using Raven.Client;

namespace CodeCamp.Infrastructure {
    public class SingleWebServerApplicationState : IApplicationState {
        const string CurrentUserKey = "CurrentUserKey";
        const string CurrentRegistrationStatusKey = "CurrentRegistrationStatusKey";
        const string CurrentEventKey = "CurrentEventKey";

        readonly IApplicationBus bus;
        readonly IDocumentSession docSession;
        readonly HttpContextBase httpContext;

        public SingleWebServerApplicationState(IApplicationBus bus, IDocumentSession docSession, HttpContextBase httpContext) {
            this.bus = bus;
            this.docSession = docSession;
            this.httpContext = httpContext;
        }

        public User User {
            get {
                try {
                    var user = httpContext.Session[CurrentUserKey] as User;
                    if(user != null) {
                        return user;
                    }

                    var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if(authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value)) {
                        return null;
                    }

                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var userId = authTicket.UserData;

                    if(string.IsNullOrEmpty(userId)) {
                        return null;
                    }

                    user = docSession.Load<User>(userId);

                    if(user != null) {
                        SetUser(user);
                    }

                    return user;
                } catch {
                    return null;
                }
            }
        }

        public RegistrationStatus RegistrationStatus {
            get {
                if(CurrentEvent == null) {
                    return RegistrationStatus.NoEventScheduled;
                }

                if(User == null) {
                    return RegistrationStatus.NotRegistered;
                }

                var existing = httpContext.Session[CurrentRegistrationStatusKey];

                if(existing == null) {
                    var reg = bus.Query(new GetUserRegistration(CurrentEvent.Id, User.Id));
                    var status = reg != null ? RegistrationStatus.Registered : RegistrationStatus.NotRegistered;
                    httpContext.Session[CurrentRegistrationStatusKey] = status;
                    return status;
                }

                return (RegistrationStatus)existing;
            }
            set {
                httpContext.Session[CurrentRegistrationStatusKey] = value;
            }
        }

        public Event CurrentEvent {
            get {
                var currentEvent = httpContext.Application.Get(CurrentEventKey) as Event;
                if(currentEvent == null) {
                    currentEvent = bus.Query(new CurrentEvent());
                    httpContext.Application.Set(CurrentEventKey, currentEvent);
                }

                return currentEvent;
            }
        }

        public void ChangeCurrentEvent(Event currentEvent) {
            currentEvent.IsCurrent = true;
            httpContext.Application.Set(CurrentEventKey, currentEvent);
            httpContext.Session.Remove(CurrentRegistrationStatusKey);
        }

        public string Environment {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        public void Login(User user, bool persist) {
            SetUser(user);
            FormsAuthentication.SetAuthCookie(user.Id, persist);
        }

        public void Logout() {
            httpContext.Session.Clear();
            httpContext.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        void SetUser(User user) {
            httpContext.Session[CurrentUserKey] = user;

            if(httpContext.User != null && httpContext.User.Identity.Name == user.Username) {
                return;
            }

            httpContext.User = new GenericPrincipal(new GenericIdentity(user.Username), user.Roles.ToArray());
        }
    }
}