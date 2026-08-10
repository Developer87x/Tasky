using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.BuildingBlocks.Constants
{
    public static class Permissions
    {
        public static class Roles
        {
            public const string Administrators = "Administrator";
            public const string Users ="Users";
            public const string Projects= "Projects";
        }

        public static class Permission
        {
            public const string Read ="Read";
            public const string Write ="Write";
            public const string Delete ="Delete";
            public const string Edit ="Edit";
            public const string FullAccess= "FullAccess";
        }
    }
}