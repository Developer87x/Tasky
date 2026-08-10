namespace Tasky.Services.Identities.Application.Security;

/// <summary>
/// Strongly-typed permission constants following Zero Trust principles.
/// Every business action requires an explicit permission.
/// Permissions are the primary authorization mechanism; roles are for organizational grouping only.
/// </summary>
public static class Permissions
{
    /// <summary>Users management permissions</summary>
    public static class Users
    {
        /// <summary>Permission to read user profiles and details</summary>
        public const string Read = "Users.Read";

        /// <summary>Permission to create new users</summary>
        public const string Create = "Users.Create";

        /// <summary>Permission to update user information</summary>
        public const string Update = "Users.Update";

        /// <summary>Permission to delete users</summary>
        public const string Delete = "Users.Delete";

        /// <summary>Permission to assign roles to users</summary>
        public const string AssignRole = "Users.AssignRole";

        /// <summary>Permission to remove roles from users</summary>
        public const string RemoveRole = "Users.RemoveRole";

        /// <summary>Permission to activate users</summary>
        public const string Activate = "Users.Activate";

        /// <summary>Permission to deactivate users</summary>
        public const string Deactivate = "Users.Deactivate";

        /// <summary>Permission to reset user passwords</summary>
        public const string ResetPassword = "Users.ResetPassword";
    }

    /// <summary>Roles management permissions</summary>
    public static class Roles
    {
        /// <summary>Permission to read role definitions</summary>
        public const string Read = "Roles.Read";

        /// <summary>Permission to create new roles</summary>
        public const string Create = "Roles.Create";

        /// <summary>Permission to update role information</summary>
        public const string Update = "Roles.Update";

        /// <summary>Permission to delete roles</summary>
        public const string Delete = "Roles.Delete";

        /// <summary>Permission to assign permissions to roles</summary>
        public const string AssignPermissions = "Roles.AssignPermissions";
    }

    /// <summary>Permissions management permissions</summary>
    public static class PermissionManagement
    {
        /// <summary>Permission to read permission definitions</summary>
        public const string Read = "PermissionManagement.Read";

        /// <summary>Permission to create new permissions</summary>
        public const string Create = "PermissionManagement.Create";

        /// <summary>Permission to update permission information</summary>
        public const string Update = "PermissionManagement.Update";

        /// <summary>Permission to delete permissions</summary>
        public const string Delete = "PermissionManagement.Delete";
    }

    /// <summary>Projects management permissions</summary>
    public static class Projects
    {
        /// <summary>Permission to read projects</summary>
        public const string Read = "Projects.Read";

        /// <summary>Permission to create new projects</summary>
        public const string Create = "Projects.Create";

        /// <summary>Permission to update project information</summary>
        public const string Update = "Projects.Update";

        /// <summary>Permission to delete projects</summary>
        public const string Delete = "Projects.Delete";

        /// <summary>Permission to archive projects</summary>
        public const string Archive = "Projects.Archive";
    }

    /// <summary>Tasks management permissions</summary>
    public static class Tasks
    {
        /// <summary>Permission to read tasks</summary>
        public const string Read = "Tasks.Read";

        /// <summary>Permission to create new tasks</summary>
        public const string Create = "Tasks.Create";

        /// <summary>Permission to update task information</summary>
        public const string Update = "Tasks.Update";

        /// <summary>Permission to delete tasks</summary>
        public const string Delete = "Tasks.Delete";

        /// <summary>Permission to mark tasks as complete</summary>
        public const string Complete = "Tasks.Complete";
    }

    /// <summary>System administration permissions</summary>
    public static class Administration
    {
        /// <summary>Full administrative access - use sparingly and only for platform administrators</summary>
        public const string FullAccess = "FullAccess";

        /// <summary>Permission to audit system logs and activities</summary>
        public const string Audit = "Administration.Audit";

        /// <summary>Permission to manage system configuration</summary>
        public const string ManageConfiguration = "Administration.ManageConfiguration";
    }

    /// <summary>
    /// Get all permission constants as a flattened collection.
    /// Useful for seeding default permissions and validation.
    /// </summary>
    public static IReadOnlyList<string> GetAll() =>
    [
        Users.Read,
        Users.Create,
        Users.Update,
        Users.Delete,
        Users.AssignRole,
        Users.RemoveRole,
        Users.Activate,
        Users.Deactivate,
        Users.ResetPassword,

        Roles.Read,
        Roles.Create,
        Roles.Update,
        Roles.Delete,
        Roles.AssignPermissions,

        PermissionManagement.Read,
        PermissionManagement.Create,
        PermissionManagement.Update,
        PermissionManagement.Delete,

        Projects.Read,
        Projects.Create,
        Projects.Update,
        Projects.Delete,
        Projects.Archive,

        Tasks.Read,
        Tasks.Create,
        Tasks.Update,
        Tasks.Delete,
        Tasks.Complete,

        Administration.FullAccess,
        Administration.Audit,
        Administration.ManageConfiguration
    ];
}
