using System;
using System.Collections.Generic;
using System.Text;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {
        Group AddGroup(Group @group);

        Group UpdateGroup(Group @group);

        List<Group> FetchAll();
    }
}
