﻿namespace DreamFactory.Demo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DreamFactory.Api;
    using DreamFactory.Model.Database;
    using DreamFactory.Model.System.User;
    using DreamFactory.Rest;

    public class SystemUserTest : IRunnable
    {
        private const string NewEmail = "user@mail.com";

        public async Task RunAsync(IRestContext context)
        {
            ISystemUserApi userApi = context.Factory.CreateSystemUserApi();

            IEnumerable<UserResponse> users = (await userApi.GetUsersAsync(new SqlQuery())).ToList();
            Console.WriteLine("GetUsersAsync(): {0}", users.Select(x => x.Name).ToStringList());

            UserResponse user = users.SingleOrDefault(x => x.Email == NewEmail);
            if (user != null)
            {
                await DeleteUser(user, userApi);
            }

            UserRequest newUser = new UserRequest
            {
                FirstName = "Andrei",
                LastName = "Smirnov",
                Name = "pinebit",
                Email = NewEmail,
                IsActive = true
            };

            users = await userApi.CreateUsersAsync(new SqlQuery(), newUser);
            user = users.Single(x => x.Email == NewEmail);
            Console.WriteLine("CreateUsersAsync(): {0}", context.ContentSerializer.Serialize(user));

            newUser.Id = user.Id;
            newUser.Name = "Andrei Smirnov";
            user = (await userApi.UpdateUsersAsync(new SqlQuery(), newUser)).Single(x => x.Email == NewEmail);
            Console.WriteLine("UpdateUsersAsync(): new name={0}", user.Name);

            await DeleteUser(user, userApi);
        }

        private static async Task DeleteUser(UserResponse user, ISystemUserApi userApi)
        {
            Debug.Assert(user.Id.HasValue, "User ID must be set");
            await userApi.DeleteUsersAsync(new SqlQuery(), user.Id.Value);
            Console.WriteLine("DeleteUsersAsync():: id={0}", user.Id);
        }
    }
}
