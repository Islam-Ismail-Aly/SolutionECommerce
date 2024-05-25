using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketoo.Application.Utilities
{
    public class RoleControllerSwaggerAttributes
    {
        public const string CreateRoleSummary = "Creates a new role.";
        public const string CreateRoleResponse201 = "Role created successfully.";
        public const string CreateRoleResponse400 = "If the provided role data is invalid.";

        public const string GetAllRolesSummary = "Retrieves all roles.";
        public const string DeleteRoleSummary = "Deletes a role by ID.";

        public const string GetAllRolesResponse200 = "A list of roles.";

        public const string DeleteRoleResponse200 = "Role deleted successfully.";
        public const string DeleteRoleResponse400 = "Invalid role ID provided.";
        public const string DeleteRoleResponse404 = "Role not found.";
        public const string DeleteRoleResponse500 = "An error occurred while deleting the role.";

        public const string GetRoleByNameSummary = "Retrieves a role by Name.";
        public const string GetRoleByNameResponse200 = "If the role is found.";
        public const string GetRoleByNameResponse404 = "If the role is not found.";
    }

}
