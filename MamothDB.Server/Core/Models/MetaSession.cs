﻿using System;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a session.
    /// </summary>
    public class MetaSession
    {
        public Guid SessionId { get; set; }
        public Guid LoginId { get; set; }
        public string Username { get; set; }
        public MetaTransaction CurrentTransaction { get; set; }

        public void CommitImplicitTransaction()
        {
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.IsImplicit)
                {
                    CurrentTransaction.Commit();
                }
            }
        }

        public static MetaSession FromPayload(Mamoth.Common.Payload.Model.Session login)
        {
            return new MetaSession
            {
                LoginId = login.LoginId,
                SessionId = login.SessionId,
                Username = login.Username
            };
        }

        public static Mamoth.Common.Payload.Model.Session ToPayload(MetaSession login)
        {
            return new Mamoth.Common.Payload.Model.Session
            {
                LoginId = login.LoginId,
                SessionId = login.SessionId,
                Username = login.Username
            };
        }

    }
}
