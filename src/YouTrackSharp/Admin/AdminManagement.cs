﻿#region License

// Distributed under the BSD License
//   
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//   
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//   

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Admin
{
    public class AdminManagement
    {
        private readonly IConnection _connection;

        public AdminManagement(IConnection connection)
        {
            _connection = connection;
        }

        public State GetStateBundle(string bundleName)
        {
            return _connection.Get<State>($"admin/customfield/stateBundle/{bundleName}");
        }

        public void CreateUser(string loginName, string fullName, string email, string jabber, string password)
        {
            Func<string, string> encoder = System.Web.HttpUtility.UrlEncode;

            var jabberDetails = string.Empty;
            if (!string.IsNullOrWhiteSpace(jabber))
            {
                jabberDetails = $"&jabber={encoder(jabber)}";
            }

            var data = new ExpandoObject();

            _connection.Post($"admin/user?login={encoder(loginName)}&fullName={encoder(fullName)}&email={encoder(email)}{jabberDetails}&password={encoder(password)}", data);
        }

        public void AddUserToGroup(string loginName, string group)
        {
            Func<string, string> encoder = System.Web.HttpUtility.UrlEncode;
            var data = new ExpandoObject();

            _connection.Post($"admin/user/{loginName}/group/{group}", data);
        }
    }
}
